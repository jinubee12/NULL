using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    [SerializeField] private float rotateSpeed = 0.025f;
    public Rigidbody2D rb;
    [SerializeField] private float attackDamage = 10f;

    private SpriteRenderer spriteRenderer;
    private Sprite enemySprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite enemySprite = Resources.Load<Sprite>("generated_image");
        Resources.UnloadAsset(enemySprite);
        if (enemySprite != null)
        {
            spriteRenderer.sprite = enemySprite;
        }
        else
        {
            Debug.LogError("Failed to load enemy sprite!");
        }
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
