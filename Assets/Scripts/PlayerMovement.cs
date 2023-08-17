using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// W, A, S, D �Է� �̵�
// ĳ���� ��Ʈ�ѷ� : '�����̽���'-��������
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    // ĳ���� ��Ʈ�ѷ�, �߷�, ���� �ӷ� ����
    CharacterController characterController;
    float gravity = -20f;
    float yVelocity = 0;

    // ���� ��
    public float jumpForce = 5f;
    // ���� ���� ����
    public bool isJumping = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // �Է� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        // ������ �����ٸ�(ĳ���Ͱ� �ٴڿ� ��� �ִٸ�) (CollisionFlags.Below : �ٴ�)
        if (isJumping && characterController.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
        }

        // �ٴڿ� ������� ��쿣, ���� �ӵ��� ���� �����Ƿ�
        if (characterController.collisionFlags == CollisionFlags.Below)
        {
            // ���� �ӵ� �ʱ�ȭ
            yVelocity = 0;
        }

        // �����̽���(����) �Է� �� ���� ���°� �ƴ϶��
        if (Input.GetButtonDown("Jump") && !isJumping) // == Input.GetKeyDown(KeyCode.Space)
        {
            yVelocity = jumpForce;
            isJumping = true;
        }

        // �̵� ���� ����
        // ����(����) ��ǥ ���(������Ʈ ��(ȸ������)�� ���� ���� �����δ�)
        Vector3 dir = new Vector3 (h, 0, v);
        // ���(����) ��ǥ ��� => ī�޶��� ������ ����
        dir = Camera.main.transform.TransformDirection(dir);

        // ĳ���� ���� �ӵ��� �߷� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 1) �̵� �ӵ��� �÷��̾� �̵�
        //transform.position += dir * speed * Time.deltaTime;
        // 2) ĳ���� ��Ʈ�ѷ��� �÷��̾� �̵�
        characterController.Move(dir * speed * Time.deltaTime);
    }
}
