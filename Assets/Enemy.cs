using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 0.0025f;
    public Rigidbody2D rb;
    [SerializeField] public float attackDamage = 10f;


    public BossPattern[] patterns;
    private float timeSinceLastPattern = 0f;
    private float patternChangeInterval = 3f; // 패턴 변경 간격 (예: 3초)

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

        timeSinceLastPattern += Time.deltaTime;

        // 일정 간격으로 패턴 변경
        
        if (timeSinceLastPattern >= patternChangeInterval)
        {
            ChangePattern();
            timeSinceLastPattern = 0f; // 초기화
        }
        
    }

    private void ChangePattern()
    {
        // 현재 실행 중인 패턴을 완료한 후에 다음 패턴을 선택하고 실행
        int selectedPatternIndex = Random.Range(0, patterns.Length);
        BossPattern selectedPattern = patterns[selectedPatternIndex];
        selectedPattern.ExecutePattern();
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
            other.gameObject.GetComponent<Health>().UpdateHealth(-attackDamage);
            if (other.gameObject.GetComponent<Health>().health == 0f)
            {
                LevelManager.manager.GameOver();
                Destroy(other.gameObject);
                target = null;
            }
        }
    }
}
