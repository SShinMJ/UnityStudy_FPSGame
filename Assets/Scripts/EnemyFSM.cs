using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 목적 : 적의 FSM 다이어그램에 따라 동작시킨다.
// 목적2: 플레이어와의 거리에 따라 동작 상태를 변경한다.
// 목적2-2 : Idle의 경우, 일정 범위 이내에 플레이어가 있다면 Move 상태로 변경.
// 목적2-2 : Move의 경우, 플레이어를 따라간다. 공격 범위 이내라면 Attack 상태로 변경.
// 목적2-3 : Attack의 경우, 플레이어를 공격한다. 공격 범위를 벗어나면 Move 상태로 변경.
// 목적2-4 : Return의 경우, 기존 생성 위치로 돌아간다. 원위치로 돌아간 경우 Idle 상태로 변경.
// 목적2-5 : Damaged의 경우, 플레이어의 공격을 받으면 플레이어의 hitDamage만큼 hp 감소.
// 목적3: 피격(레이가 부딪힌) 대상이 Enemy라면 Enemy에게 데미지 입히기.
// 목적4: 적 hp(%)를 hp 슬라이더에 적용.
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
    // 공격 데미지
    public int attackPower = 1;

    // 2-4. 초기 위치
    Vector3 originPos;
    // 최대 이동 범위
    public float maxMoveDistance = 20f;
    // 원위치 오차 범위
    public float minRetrunDistance = 0.3f;

    // 2-5. 적 hp
    public int enemyHp = 3;

    // 4. maxHP
    int maxHp = 3;
    // Slider
    public Slider hpSlider;

    void Start()
    {
        // 시작 시, 적의 상태는 대기 상태.
        enemyState = EnemyState.Idle;

        // 플레이어 트랜스폼 받아오기
        player = GameObject.Find("Player").transform;

        // 적 오브젝트 컨트롤러 받아오기
        characterController = GetComponent<CharacterController>();

        // 초기 위치 저장
        originPos = transform.position;

        maxHp = enemyHp;
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
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        // 4. 현재 적 hp를 hp슬라이더에 적용
        hpSlider.value = (float)enemyHp / (float)maxHp;
    }

    private void Idle()
    {
        // 플레이어와의 거리 측정
        float distanceToPlayer = (player.position - transform.position).magnitude;
        //             = Vector3.Distance(transform.position, player.position);

        // 플레이어와의 거리가 특정 거리 이내가 되면,
        if (distanceToPlayer < findDistance)
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

        // 초기 위치에서 적과의 거리
        float distanceToOriginPos = (originPos - transform.position).magnitude;

        // 플레이어를 따라가다 초기 위치에서 일정 거리를 벗어나면
        if (distanceToOriginPos > maxMoveDistance)
        {
            enemyState = EnemyState.Return;
            print("적 상태전환 : 따라감 > 돌아감");
        }
        // 플레이어와의 거리가 공격 범위 밖이면
        else if (distanceToPlayer > attackDistance)
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
            // 바로 공격
            currentTime = attackDelay;
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

            // 일정 시간마다
            if (currentTime > attackDelay)
            {
                // 플레이어를 공격한다. 플레이어의 hp가 attackPower만큼 감소한다.
                player.GetComponent<PlayerMovement>().DamageAction(attackPower);

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
        // 플레이어와의 거리 측정
        float distanceToPlayer = (player.position - transform.position).magnitude;

        // 초기 위치에서 적과의 거리
        float distanceToOriginPos = (originPos - transform.position).magnitude;

        // 초기 위치로 돌아가지 못했다면 계속해서 원위치로 이동
        if (distanceToOriginPos > minRetrunDistance)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        // 원위치로 돌아왔다면
        else
        {
            // 대기(Idle)로 상태 변환
            print("적 상태전환 : 원위치 > 대기");
            enemyState = EnemyState.Idle;
        }
    }

    // 플레이어의 공격을 받으면
    private void Damaged()
    {
        // 피격 모션 0.5초 재생


        // 피격 상태 처리를 위한 코루틴 실행
        StartCoroutine(DamageProcess());
    }

    // 플레이어의 hitDamage만큼 hp 감소.
    public void DamageAction(int damage)
    {
        // 이미 적이 피격되가나 사망 상태라면 데미지 주기x
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
            return;

        enemyHp -= damage;

        if (enemyHp > 0)
        {
            print("적 상태전환 : 피격당함 > 따라감");
            enemyState = EnemyState.Damaged;
            Damaged();
        }
        else
        {
            print("적 상태전환 : 피격당함 > 죽음");
            enemyState = EnemyState.Die;
            Die();
        }
    }

    // 데미지 처리용
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 대기.
        yield return new WaitForSeconds(0.5f);

        // 피격 모션이 끝나면 적 동작 상태를 Move로 바꾼다.
        print("적 상태전환 : 피격당함 > 따라감");
        enemyState = EnemyState.Move;
    }


    void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    // 2초 후에 적 오브젝트(자신) 제거
    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(2f);

        print("사망");
        Destroy(gameObject);
    }
}
