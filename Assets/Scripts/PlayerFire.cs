using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

// ���� : ���콺 ������ ��ư�� ���� ��ź�� Ư�� ��ġ�� �����ϰ�, �������� �߻�.
// ����2: ���콺 ���� ��ư�� ���� �ü� �������� ���� �߻�.

// ����3: �̵� Blend Tree�� �Ķ���� ���� 0�� ��, Attack Trigger ����.
//                        (Blend Tree�� �������� �� tigger�� Ű�� ������ �� ����)

// ����4: Ű���� Ư�� Ű �Է����� ���� ���(�Ϲ�/����) ��ȯ

public class PlayerFire : MonoBehaviour
{
    // ��ź ���� ������Ʈ
    public GameObject bombPrf;
    // �߻� ��ġ
    public GameObject firePosition;
    public float firePower = 5f;

    // �� �ǰ� ȿ��
    public GameObject hitEffect;
    // ����Ʈ ��ƼŬ �ý���
    ParticleSystem particleSys;
    // �ǰ� ������
    public int weaponPower = 1;

    // 3. �ڽ� ������Ʈ�� �ִϸ�����
    Animator animator;

    // 4. ������ ������ ����(���� ����)
    public enum WeaponMode
    {
        Normal,
        Sniper
    }
    WeaponMode weaponMode = WeaponMode.Normal;

    // �� ���� Ȯ�� ����
    bool isZoomMode = false;

    // ���� ��� Ȯ�� �ؽ�Ʈ UI
    public TMP_Text weaponModeTxt;

    private void Start()
    {
        particleSys = hitEffect.GetComponent<ParticleSystem>();

        // 3. �ڽ� ������Ʈ�� �ִϸ�����
        animator = GetComponentInChildren<Animator>();

        // ���� ��� �ʱ�ȭ
        weaponModeTxt.text = "Normal Mode";
    }

    void Update()
    {
        // GameManager���� 'Start' ���°� �ƴ϶�� ���� �Ұ�.
        if (GameManager.Instance.status != GameManager.GameStatus.Start)
        {
            return;
        }

        // ���콺 ������ ��ư�� Ŭ���ϸ�
        if (Input.GetMouseButtonDown(1))  // ���콺 ���� : 0, ������ : 1, �� : 2
        {
            // 4. Ű���� Ư�� Ű �Է����� ���� ���(�Ϲ�/����) ��ȯ
            // ���� ���(����)�� ���� 
            switch (weaponMode)
            {
                // ��� ��� : ���콺 ������ Ŭ�� �� ��ź�� ������.
                case WeaponMode.Normal:
                    GameObject bombObj = Instantiate(bombPrf);
                    // ��ź �߻� ��ġ�� fireposition ������ �Ͽ� Ư�� ��ġ�� �����ǰ� �Ѵ�.
                    bombObj.transform.position = firePosition.transform.position;

                    // ��ź ������Ʈ�� rigidBody ���� ������ ��, ��(����)�� ���Ѵ�.
                    Rigidbody rigidbody = bombObj.GetComponent<Rigidbody>();
                    // ī�޶��� ����(forward)�������� firePower�� ���� ����ä, ������ ������ �޴´�.(Impulse: ����)
                    rigidbody.AddForce(Camera.main.transform.forward * firePower, ForceMode.Impulse);
                    break;

                // �������� ��� : ���콺 ������ Ŭ�� �� ȭ���� ��(Ȯ��) �ȴ�.
                case WeaponMode.Sniper:
                    // �ܸ�� ���°� �ƴ϶��
                    if(!isZoomMode)
                    {
                        // �÷��̾� ī�޶��� �þ߰��� 15�� ������.
                        Camera.main.fieldOfView = 15;
                        isZoomMode = true;
                    }
                    // �ܸ�� ���¶��
                    else
                    {
                        Camera.main.fieldOfView = 60;
                        isZoomMode = false;
                    }
                    break;
            }

        }

        // ���콺 ���� ��ư�� Ŭ���ϸ�
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(animator.GetFloat("MoveMotion"));
            // Blend Tree�� �Ķ���� ���� 0�̸�,
            if (animator.GetFloat("MoveMotion") <= 0.1)
            {
                // �ǰ� �ִϸ��̼� ����
                animator.SetTrigger("Attack");
            }

            // ����ĳ������ �����ϰ� �߻� ��ġ�� �߻� ������ �����Ѵ�.
            // Ray�� ����ü�� �����Ǿ� �ִ�.�������� �ڷ����� ���� �� ������ ���� �� �ִ�.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // ����ĳ������ �ε��� ����� ������ ������ �� �ִ� ������ �����.
            RaycastHit hitInfo = new RaycastHit();

            // ���̸� �߻��ϰ� (hitInfo�� �ε��� ��ü�� ������ true�� ��ȯ)
            if(Physics.Raycast(ray, out hitInfo))
            {
                // �ε��� ��ü�� ������ �� ��ġ�� �ǰ� ȿ���� (���� ���� ��������)�����.
                // hitInfo.point : ���� �΋H�� ��ġ
                hitEffect.transform.position = hitInfo.point;
                // hitInfo.normal : �ε��� �κ��� normal ����. ƨ���� ���� ������ �������ش�.
                hitEffect.transform.forward = hitInfo.normal;

                // �ǰ� ����Ʈ�� ����Ѵ�.
                particleSys.Play();

                // �ǰ� ����� ���̶�� ������ �ֱ�
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM enemyFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    enemyFSM.DamageAction(weaponPower);
                }
            }
        }

        // 4. Ű���� ���� 1�� Ű�ٿ� : ������-��ָ��
        //    Ű���� ���� 2�� Ű�ٿ� : ������-�������۸��
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponModeTxt.text = "Normal Mode";
            weaponMode = WeaponMode.Normal;

            // ī�޶� FoV�� ó�� ���·� �ʱ�ȭ
            Camera.main.fieldOfView = 60;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponModeTxt.text = "Sniper Mode";
            weaponMode = WeaponMode.Sniper;
        }
    }
}
