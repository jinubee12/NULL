using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : Enemy
{
    private float chargeSpeed = 5f; // ���� �ӵ�
    private float chargeDistance = 5f; // ���� �Ÿ�
    private float chargeDuration = 1f; // ���� ���� �ð�

    // ���� ������
    public enum BossPatterns
    {
        Idle,
        Charge,
        // �ٸ� ���ϵ� �߰� ����
    }

    private BossPatterns currentPattern = BossPatterns.Idle;

    private void Update()
    {
        // ���� ���࿡ �ʿ��� ���۵��� ����
        ExecutePattern();
    }

    public void ExecutePattern()
    {
        switch (currentPattern)
        {
            case BossPatterns.Idle:
                // Idle ���� ����
                // (��: Ư�� �ð� ���� ������ �ֱ�)
                Idle();
                break;

            case BossPatterns.Charge:
                // Charge ���� ����
                ChargeTowardsPlayer();
                break;

            // �ٸ� ���ϵ� �߰� ����

            default:
                break;
        }
    }

    private void Idle()
    {
        // Idle ���� ���� ����
        // (��: Ư�� �ð� ���� ������ �ֱ�)
    }

    private void ChargeTowardsPlayer()
    {
        // ���� ���� ���
        Vector2 chargeDirection = ((Vector2)target.position - rb.position).normalized;

        // ���� �Ÿ���ŭ ����
        float elapsedTime = 0f;
        while (elapsedTime < chargeDuration)
        {
            // �̵�
            rb.velocity = chargeDirection * chargeSpeed;

            // �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            // yield ��ȯ ���� �� ������ ���
            // (���� ���ӿ����� Coroutine�� ����ϴ� ���� �� ȿ������ �� �ֽ��ϴ�)
            // ��: yield return null;
        }

        // ���� �� ������ �ֱ�
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
