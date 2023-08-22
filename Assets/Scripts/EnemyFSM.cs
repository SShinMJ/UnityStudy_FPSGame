using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ���� : ���� FSM ���̾�׷��� ���� ���۽�Ų��.
// ����2: �÷��̾���� �Ÿ��� ���� ���� ���¸� �����Ѵ�.
// ����2-2 : Idle�� ���, ���� ���� �̳��� �÷��̾ �ִٸ� Move ���·� ����.
// ����2-2 : Move�� ���, �÷��̾ ���󰣴�. ���� ���� �̳���� Attack ���·� ����.
// ����2-3 : Attack�� ���, �÷��̾ �����Ѵ�. ���� ������ ����� Move ���·� ����.
// ����2-4 : Return�� ���, ���� ���� ��ġ�� ���ư���. ����ġ�� ���ư� ��� Idle ���·� ����.
// ����2-5 : Damaged�� ���, �÷��̾��� ������ ������ �÷��̾��� hitDamage��ŭ hp ����.
// ����3: �ǰ�(���̰� �ε���) ����� Enemy��� Enemy���� ������ ������.
// ����4: �� hp(%)�� hp �����̴��� ����.
public class EnemyFSM : MonoBehaviour
{
    // 1. ���� ���� ����(���, �̵�, ����, ����ġ, �ǰ�, ����)
    // enum : ������
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

    // 2-1. �÷��̾ �Ѿư��� �����ϴ� �Ÿ�
    public float findDistance = 7;
    // �÷��̾� Ʈ������(�÷��̾ ���󰡱� ����)
    Transform player;

    // 2-2. �̵� �ӵ�
    public float moveSpeed = 2f;
    // ���� �̵��� ���� ��Ʈ�ѷ�
    CharacterController characterController;
    // ���� ����
    public float attackDistance = 2f;

    // 2-3. ���� ����(�ð�)
    float currentTime = 0;
    public float attackDelay = 2f;
    // ���� ������
    public int attackPower = 1;

    // 2-4. �ʱ� ��ġ
    Vector3 originPos;
    // �ִ� �̵� ����
    public float maxMoveDistance = 20f;
    // ����ġ ���� ����
    public float minRetrunDistance = 0.3f;

    // 2-5. �� hp
    public int enemyHp = 3;

    // 4. maxHP
    int maxHp = 3;
    // Slider
    public Slider hpSlider;

    void Start()
    {
        // ���� ��, ���� ���´� ��� ����.
        enemyState = EnemyState.Idle;

        // �÷��̾� Ʈ������ �޾ƿ���
        player = GameObject.Find("Player").transform;

        // �� ������Ʈ ��Ʈ�ѷ� �޾ƿ���
        characterController = GetComponent<CharacterController>();

        // �ʱ� ��ġ ����
        originPos = transform.position;

        maxHp = enemyHp;
    }

    void Update()
    {
        // �� ���� ���� ����
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

        // 4. ���� �� hp�� hp�����̴��� ����
        hpSlider.value = (float)enemyHp / (float)maxHp;
    }

    private void Idle()
    {
        // �÷��̾���� �Ÿ� ����
        float distanceToPlayer = (player.position - transform.position).magnitude;
        //             = Vector3.Distance(transform.position, player.position);

        // �÷��̾���� �Ÿ��� Ư�� �Ÿ� �̳��� �Ǹ�,
        if (distanceToPlayer < findDistance)
        {
            // ���� ���¸� Move�� �ٲ��ش�.
            print("�� ������ȯ : ��� > ����");
            enemyState = EnemyState.Move;
        }
    }

    private void Move()
    {
        // magnitude : (��Ÿ��� ���ǿ� ����)������ ���̸� ��ȯ�Ѵ�. ��, �Ÿ�(ũ��)�� �ǹ��Ѵ�.
        float distanceToPlayer = (player.position - transform.position).magnitude;

        // �ʱ� ��ġ���� ������ �Ÿ�
        float distanceToOriginPos = (originPos - transform.position).magnitude;

        // �÷��̾ ���󰡴� �ʱ� ��ġ���� ���� �Ÿ��� �����
        if (distanceToOriginPos > maxMoveDistance)
        {
            enemyState = EnemyState.Return;
            print("�� ������ȯ : ���� > ���ư�");
        }
        // �÷��̾���� �Ÿ��� ���� ���� ���̸�
        else if (distanceToPlayer > attackDistance)
        {
            // �÷��̾ ���󰣴�. (normalized : ���⺤�͸� 1ũ��� ����ȭ�Ͽ� ��������(���⺤��)�� ����)
            Vector3 dir = (player.position - transform.position).normalized;
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        // �÷��̾���� �Ÿ��� ���� ���� ���̸�
        else if (distanceToPlayer < attackDistance)
        {
            // ���� ���¸� Attack���� �ٲ��ش�.
            print("�� ������ȯ : ���� > ����");
            enemyState = EnemyState.Attack;
            // �ٷ� ����
            currentTime = attackDelay;
        }
    }

    private void Attack()
    {
        // �÷��̾���� �Ÿ� ����
        float distanceToPlayer = (player.position - transform.position).magnitude;
        //             = Vector3.Distance(transform.position, player.position);

        // �÷��̾���� �Ÿ��� ���� ���� �����
        if (distanceToPlayer < attackDistance)
        {
            currentTime += Time.deltaTime;

            // ���� �ð�����
            if (currentTime > attackDelay)
            {
                // �÷��̾ �����Ѵ�. �÷��̾��� hp�� attackPower��ŭ �����Ѵ�.
                player.GetComponent<PlayerMovement>().DamageAction(attackPower);

                currentTime = 0;
            }
        }
        // ���� ���� ���̶��
        else
        {
            // Move ���·� ��ȯ
            print("�� ������ȯ : ���� > ����");
            enemyState = EnemyState.Move;
        }
    }

    private void Return()
    {
        // �÷��̾���� �Ÿ� ����
        float distanceToPlayer = (player.position - transform.position).magnitude;

        // �ʱ� ��ġ���� ������ �Ÿ�
        float distanceToOriginPos = (originPos - transform.position).magnitude;

        // �ʱ� ��ġ�� ���ư��� ���ߴٸ� ����ؼ� ����ġ�� �̵�
        if (distanceToOriginPos > minRetrunDistance)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        // ����ġ�� ���ƿԴٸ�
        else
        {
            // ���(Idle)�� ���� ��ȯ
            print("�� ������ȯ : ����ġ > ���");
            enemyState = EnemyState.Idle;
        }
    }

    // �÷��̾��� ������ ������
    private void Damaged()
    {
        // �ǰ� ��� 0.5�� ���


        // �ǰ� ���� ó���� ���� �ڷ�ƾ ����
        StartCoroutine(DamageProcess());
    }

    // �÷��̾��� hitDamage��ŭ hp ����.
    public void DamageAction(int damage)
    {
        // �̹� ���� �ǰݵǰ��� ��� ���¶�� ������ �ֱ�x
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
            return;

        enemyHp -= damage;

        if (enemyHp > 0)
        {
            print("�� ������ȯ : �ǰݴ��� > ����");
            enemyState = EnemyState.Damaged;
            Damaged();
        }
        else
        {
            print("�� ������ȯ : �ǰݴ��� > ����");
            enemyState = EnemyState.Die;
            Die();
        }
    }

    // ������ ó����
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ���.
        yield return new WaitForSeconds(0.5f);

        // �ǰ� ����� ������ �� ���� ���¸� Move�� �ٲ۴�.
        print("�� ������ȯ : �ǰݴ��� > ����");
        enemyState = EnemyState.Move;
    }


    void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    // 2�� �Ŀ� �� ������Ʈ(�ڽ�) ����
    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(2f);

        print("���");
        Destroy(gameObject);
    }
}
