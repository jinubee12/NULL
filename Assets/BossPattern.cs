using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : Enemy
{
    private float chargeSpeed = 5f; // 돌진 속도
    private float chargeDistance = 5f; // 돌진 거리
    private float chargeDuration = 1f; // 돌진 지속 시간

    // 패턴 열거형
    public enum BossPatterns
    {
        Idle,
        Charge,
        // 다른 패턴들 추가 가능
    }

    private BossPatterns currentPattern = BossPatterns.Idle;

    private void Update()
    {
        // 패턴 실행에 필요한 동작들을 정의
        ExecutePattern();
    }

    public void ExecutePattern()
    {
        switch (currentPattern)
        {
            case BossPatterns.Idle:
                // Idle 패턴 동작
                // (예: 특정 시간 동안 가만히 있기)
                Idle();
                break;

            case BossPatterns.Charge:
                // Charge 패턴 동작
                ChargeTowardsPlayer();
                break;

            // 다른 패턴들 추가 가능

            default:
                break;
        }
    }

    private void Idle()
    {
        // Idle 패턴 동작 구현
        // (예: 특정 시간 동안 가만히 있기)
    }

    private void ChargeTowardsPlayer()
    {
        // 돌진 방향 계산
        Vector2 chargeDirection = ((Vector2)target.position - rb.position).normalized;

        // 일정 거리만큼 돌진
        float elapsedTime = 0f;
        while (elapsedTime < chargeDuration)
        {
            // 이동
            rb.velocity = chargeDirection * chargeSpeed;

            // 시간 업데이트
            elapsedTime += Time.deltaTime;

            // yield 반환 없이 한 프레임 대기
            // (실제 게임에서는 Coroutine을 사용하는 것이 더 효과적일 수 있습니다)
            // 예: yield return null;
        }

        // 돌진 후 가만히 있기
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
