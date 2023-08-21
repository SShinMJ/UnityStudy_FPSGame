using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적 : 적의 FSM 다이어그램에 따라 동작시킨다.
// 목적2: 플레이어와의 거리에 따라 동작 상태를 변경한다.
// 목적2-2 : Idle의 경우, 일정 범위 이내에 플레이어가 있다면 Move 상태로 변경.
// 목적2-2 : Move의 경우, 플레이어를 따라간다. 공격 범위 이내라면 Attack 상태로 변경.
// 목적2-3 : Attack의 경우, 플레이어를 공격한다. 공격 범위를 벗어나면 Move 상태로 변경.
public class EnemyFSM : MonoBehaviour
{
    // 1. 적의 현재 상태(대기, 이동, 공격, 원위치, 피격, 죽음)
    // enum : 열거형
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState enemyState;

    // 2-1. 플레이어를 쫓아가기 시작하는 거리
    public float findDistance = 7;
    // 플레이어 트랜스폼(플레이어를 따라가기 위해)
    Transform player;

    // 2-2. 이동 속도
    public float moveSpeed = 2f;
    // 적의 이동을 위한 컨트롤러
    CharacterController characterController;
    // 공격 범위
    public float attackDistance = 2f;

    // 2-3. 공격 간격(시간)
    float currentTime = 0;
    public float attackDelay = 2f;

    void Start()
    {
        // 시작 시, 적의 상태는 대기 상태.
        enemyState = EnemyState.Idle;

        // 플레이어 트랜스폼 받아오기
        player = GameObject.Find("Player").transform;

        // 적 오브젝트 컨트롤러 받아오기
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 적 동작 상태 변경
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break; 
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
        }
        
    }

    private void Idle()
    {
        // 플레이어와의 거리 측정
        float distanceToPlayer = (player.position - transform.position).magnitude;
        //             = Vector3.Distance(transform.position, player.position);

        // 플레이어와의 거리가 특정 거리 이내가 되면,
        if(distanceToPlayer < findDistance)
        {
            // 동작 상태를 Move로 바꿔준다.
            print("적 상태전환 : 대기 > 따라감");
            enemyState = EnemyState.Move;
        }
    }

    private void Move()
    {
        // magnitude : (피타고라스 정의에 의한)벡터의 길이를 반환한다. 즉, 거리(크기)를 의미한다.
        float distanceToPlayer = (player.position - transform.position).magnitude;

        // 플레이어와의 거리가 공격 범위 밖이면
        if (distanceToPlayer > attackDistance)
        {
            // 플레이어를 따라간다. (normalized : 방향벡터를 1크기로 평준화하여 단위벡터(방향벡터)로 만듦)
            Vector3 dir = (player.position - transform.position).normalized;
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        // 플레이어와의 거리가 공격 범위 안이면
        else if (distanceToPlayer < attackDistance)
        {
            // 동작 상태를 Attack으로 바꿔준다.
            print("적 상태전환 : 따라감 > 공격");
            enemyState = EnemyState.Attack;
        }
    }

    private void Attack()
    {
        // 플레이어와의 거리 측정
        float distanceToPlayer = (player.position - transform.position).magnitude;
        //             = Vector3.Distance(transform.position, player.position);

        // 플레이어와의 거리가 공격 범위 내라면
        if (distanceToPlayer < attackDistance)
        {
            currentTime += Time.deltaTime;

            // 일정 시간마다 플레이어를 공격한다
            if (currentTime > attackDelay)
            {
                print("!!!!플레이어를 공격함!!!!");

                currentTime = 0;
            }
        }
        // 공격 범위 밖이라면
        else
        {
            // Move 상태로 전환
            print("적 상태전환 : 공격 > 따라감");
            enemyState = EnemyState.Move;
        }
    }

    private void Return()
    {
        throw new NotImplementedException();
    }
    private void Damaged()
    {
        throw new NotImplementedException();
    }
}

