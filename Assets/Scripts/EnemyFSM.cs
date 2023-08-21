using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : ���� FSM ���̾�׷��� ���� ���۽�Ų��.
// ����2: �÷��̾���� �Ÿ��� ���� ���� ���¸� �����Ѵ�.
// ����2-2 : Idle�� ���, ���� ���� �̳��� �÷��̾ �ִٸ� Move ���·� ����.
// ����2-2 : Move�� ���, �÷��̾ ���󰣴�. ���� ���� �̳���� Attack ���·� ����.
// ����2-3 : Attack�� ���, �÷��̾ �����Ѵ�. ���� ������ ����� Move ���·� ����.
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

    void Start()
    {
        // ���� ��, ���� ���´� ��� ����.
        enemyState = EnemyState.Idle;

        // �÷��̾� Ʈ������ �޾ƿ���
        player = GameObject.Find("Player").transform;

        // �� ������Ʈ ��Ʈ�ѷ� �޾ƿ���
        characterController = GetComponent<CharacterController>();
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
                Damaged();
                break;
        }
        
    }

    private void Idle()
    {
        // �÷��̾���� �Ÿ� ����
        float distanceToPlayer = (player.position - transform.position).magnitude;
        //             = Vector3.Distance(transform.position, player.position);

        // �÷��̾���� �Ÿ��� Ư�� �Ÿ� �̳��� �Ǹ�,
        if(distanceToPlayer < findDistance)
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

        // �÷��̾���� �Ÿ��� ���� ���� ���̸�
        if (distanceToPlayer > attackDistance)
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

            // ���� �ð����� �÷��̾ �����Ѵ�
            if (currentTime > attackDelay)
            {
                print("!!!!�÷��̾ ������!!!!");

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
        throw new NotImplementedException();
    }
    private void Damaged()
    {
        throw new NotImplementedException();
    }
}

