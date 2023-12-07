using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public Rigidbody2D rb;
    public Weapon weapon;
    private bool isMoving;
    private float rollDuration = 0.35f;
    private float rollCooldown = 1f;
    public float rollSpeed = 25f;
    private bool isRolling = false;
    public float rollSpeedMultiplier = 2f;

    Vector2 moveDir;
    Vector2 mousePos;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX == 0 && moveY == 0)
        {
            isMoving = false;
            animator.SetFloat("XY", moveX);
        }
        else
        {
            isMoving = true;
            animator.SetFloat("XY", moveX);
        }

        
        animator.SetBool("isMoving", isMoving);

        moveDir = new Vector2(moveX, moveY).normalized;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weapon.SetTarget(mousePos);


        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            StartCoroutine(Roll());
        }
        
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * (isRolling ? moveSpeed * rollSpeedMultiplier : moveSpeed);
    }

    private IEnumerator Roll()
    {
        isRolling = true;
        animator.SetBool("isRolling", isRolling);
        float originalSpeed = moveSpeed;

        moveSpeed *= rollSpeedMultiplier;
        yield return new WaitForSeconds(rollDuration);

        isRolling = false;
        animator.SetBool("isRolling", isRolling);
        moveSpeed = originalSpeed;
        yield return new WaitForSeconds(rollCooldown); // 구르기 쿨다운 적용
    }
}
