using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// W, A, S, D �Է� �̵�
// ĳ���� ��Ʈ�ѷ� : '�����̽���'-��������

// ����2 : �÷��̾ ������ ������ hp�� damage��ŭ ����.

// ����3 : ���� �÷��̾� hp(%)�� hp �����̴��� ����.

// ����4 : ���� ������ ���� ��, Hit Image�� �״ٰ� ����.
// ����5 : hp�� 0�̵� ��� hit Image�� ���İ��� 255�� �����.

// ����6 : GameManager�� 'Ready'���¿��� �÷��̾�, �� ��� ������ �� ����.
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

    // 2. hp
    public int playerHp = 10;

    // 3. maxHP
    int maxHp = 10;
    // Slider
    public Slider hpSlider;

    // 4. Hit Image ���ӿ�����Ʈ
    public GameObject hitImg;

    // 5. ���� �ð�, hitImage ����ð�
    float currentTime;
    public float hitImageEndTime = 3f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        maxHp = playerHp;
    }

    void Update()
    {
        // 3. ���� �÷��̾� hp�� hp�����̴��� ����
        hpSlider.value = (float)playerHp / (float)maxHp;

        // GameManager���� 'Start' ���°� �ƴ϶�� ���� �Ұ�.
        if (GameManager.Instance.status != GameManager.GameStatus.Start)
        {
            return;
        }

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

    // 2. hp�� damage��ŭ ����.
    public void DamageAction(int damage)
    {
        playerHp -= damage;

        // hitImage ���� Ű��
        if (playerHp > 0)
        {
            StartCoroutine(PlayerHitEff());
        }
        // HP�� 0�� �� ���, 
        else
        {
            StartCoroutine(DeadEff());
        }
    }

    IEnumerator PlayerHitEff()
    {
        // hitImage Ȱ��ȭ
        hitImg.SetActive(true);

        // 0.5�� ���
        yield return new WaitForSeconds(0.2f);

        // hitImage ��Ȱ��ȭ
        hitImg.SetActive(false);
    }

    // HitImage�� ���İ��� ���� ������ 255�� ������ش�.
    IEnumerator DeadEff()
    {
        // hitImage Ȱ��ȭ
        hitImg.SetActive(true);
        Color hitImgColor = hitImg.GetComponent<Image>().color;

        while (true)
        {
            currentTime += Time.deltaTime;

            yield return null;

            hitImgColor.a = Mathf.Lerp(0, 1, currentTime / hitImageEndTime);

            hitImg.GetComponent<Image>().color = hitImgColor;

            if(currentTime > hitImageEndTime)
            {
                currentTime = 0;
                break;
            }
        }
    }
}
