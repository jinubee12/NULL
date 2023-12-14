using UnityEngine;
using System.IO;
using OpenAI;

public class Enemy : MonoBehaviour
{ 
    public float speed = 3.0f;
    public Transform target;
    
    [SerializeField] private float rotateSpeed = 0.075f;
    public Rigidbody2D rb;
    [SerializeField] private float attackDamage = 10f;

    private SpriteRenderer spriteRenderer;
    private Sprite enemySprite;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChatGPT gpt = FindObjectOfType<ChatGPT>();
        if (gpt == null)
        {
            Debug.LogError("GPT ????? ?? ? ????");
        }
        speed = gpt.fSPEEDin;
        attackDamage = gpt.fSTRONGin;

        string imagePath = Application.dataPath + "/Resources" + "/generated_image.png";
        //"Assets/Resources/generated_image.png"; // ??????????

        Texture2D texture = LoadTextureFromFile(imagePath);
        if (texture != null)
        {
            //???? ???????? ??????
            Color[] pixels = texture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].r >= 0.85f && pixels[i].g >= 0.85f && pixels[i].b >= 0.85f)
                {
                    pixels[i].a = 0;
                }
            }
            texture.SetPixels(pixels);
            texture.Apply();
            //

            Sprite enemySprite = SpriteFromTexture(texture);
            spriteRenderer.sprite = enemySprite;
        }
        else
        {
            Debug.LogError("Failed to load enemy sprite!");
        }
    }

    private Texture2D LoadTextureFromFile(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2); 
        texture.LoadImage(fileData); 
        return texture;
    }

    private Sprite SpriteFromTexture(Texture2D texture)
    {
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, Vector2.one * 0.5f);
        return sprite;
    }



    private void Update()
    {
        if (!target)
            GetTarget();
        else
            RotateTowardsTarget();
    }


    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector2 targetDir = target.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;

        float step = rotateSpeed * Time.deltaTime * 70;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), step);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DealDamageToPlayer(other.gameObject);
        }
    }

    private void DealDamageToPlayer(GameObject player)
    {
        var playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.UpdateHealth(-attackDamage);
            if (playerHealth.health == 0f)
            {
                LevelManager.manager.GameOver();
                Destroy(player);
                target = null;
            }
        }
    }
}
