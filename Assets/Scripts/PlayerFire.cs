using Photon.Pun;
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

// ����5: ���� �߻��� ��, ���� �ð� �Ŀ� ������� �ѱ� ����Ʈ�� Ȱ��ȭ�Ѵ�.

public class PlayerFire : MonoBehaviour, IPunObservable
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

    // 5. �ѱ� ����Ʈ �迭
    public GameObject[] fireFlashEffs;

    // ����1(������), ����2(��������)
    public GameObject crossHair1;
    public GameObject crossHair2;
    public GameObject crossHairZoom;

    // ������, ��������, ����ź �̹���
    public GameObject rifleImg;
    public GameObject sniperImg;
    public GameObject grenadeImge;

    PhotonView photonView;

    private void Start()
    {
        particleSys = hitEffect.GetComponent<ParticleSystem>();

        // 3. �ڽ� ������Ʈ�� �ִϸ�����
        animator = GetComponentInChildren<Animator>();

        // ���� ��� �ʱ�ȭ
        weaponModeTxt.text = "Rifle Mode";

        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        // ���� �Է��� ���
        if (photonView.IsMine)
        {
            // GameManager���� 'Start' ���°� �ƴ϶�� ���� �Ұ�.
            if (GameManager.Instance.status != GameManager.GameStatus.Start)
            {
                return;
            }

            // ���콺 ������ ��ư�� Ŭ���ϸ�
            if (Input.GetMouseButtonDown(1))  // ���콺 ���� : 0, ������ : 1, �� : 2
            {
                // �Էµ� �� ��� �÷��̾�� ���� ��������.
                // RPC�� �ش� �Լ��� ��� Ÿ�ٿ��� �ش� ���� ��������.
                photonView.RPC("SendMouseAction", RpcTarget.All, 1);

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
                        if (!isZoomMode)
                        {
                            // �÷��̾� ī�޶��� �þ߰��� 15�� ������.
                            Camera.main.fieldOfView = 15;
                            isZoomMode = true;

                            crossHairZoom.SetActive(true);
                            crossHair2.SetActive(false);
                        }
                        // �ܸ�� ���¶��
                        else
                        {
                            Camera.main.fieldOfView = 60;
                            isZoomMode = false;

                            crossHairZoom.SetActive(false);
                            crossHair2.SetActive(true);
                        }
                        break;
                }

            }

            // ���콺 ���� ��ư�� Ŭ���ϸ�
            else if (Input.GetMouseButtonDown(0))
            {
                // �Էµ� �� ��� �÷��̾�� ���� ��������.
                photonView.RPC("SendMouseAction", RpcTarget.All, 0);

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
                if (Physics.Raycast(ray, out hitInfo))
                {
                    // �ε��� ��ü�� ������ �� ��ġ�� �ǰ� ȿ���� (���� ���� ��������)�����.
                    // hitInfo.point : ���� �΋H�� ��ġ
                    hitEffect.transform.position = hitInfo.point;
                    // hitInfo.normal : �ε��� �κ��� normal ����. ƨ���� ���� ������ �������ش�.
                    hitEffect.transform.forward = hitInfo.normal;

                    // �ǰ� ����Ʈ�� ����Ѵ�.
                    particleSys.Play();

                    // �ǰ� ����� ���̶�� ������ �ֱ�
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        EnemyFSM enemyFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                        enemyFSM.DamageAction(weaponPower);
                    }
                }

                // 5. �ѱ� ����Ʈ ������ ���� �ڷ�ƾ ����
                StartCoroutine(ShootEffOn(0.05f));
            }

            // 4. Ű���� ���� 1�� Ű�ٿ� : ������-��ָ��
            //    Ű���� ���� 2�� Ű�ٿ� : ������-�������۸��
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // �Էµ� �� ��� �÷��̾�� ���� ��������.
                photonView.RPC("SendMouseAction", RpcTarget.All, 2);

                weaponModeTxt.text = "Rifle Mode";
                weaponMode = WeaponMode.Normal;

                // ī�޶� FoV�� ó�� ���·� �ʱ�ȭ
                Camera.main.fieldOfView = 60;

                crossHair1.SetActive(true);
                crossHair2.SetActive(false);

                rifleImg.SetActive(true);
                sniperImg.SetActive(false);
                grenadeImge.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // �Էµ� �� ��� �÷��̾�� ���� ��������.
                photonView.RPC("SendMouseAction", RpcTarget.All, 3);

                weaponModeTxt.text = "Sniper Mode";
                weaponMode = WeaponMode.Sniper;

                crossHair1.SetActive(false);
                crossHair2.SetActive(true);

                rifleImg.SetActive(false);
                sniperImg.SetActive(true);
                grenadeImge.SetActive(false);
            }
            else
            {
                photonView.RPC("SendMouseAction", RpcTarget.All, 4);
            }
        }
        // ��� �÷��̾��� �Է��� ���,
        else
        {
            // GameManager���� 'Start' ���°� �ƴ϶�� ���� �Ұ�.
            if (GameManager.Instance.status != GameManager.GameStatus.Start)
            {
                return;
            }

            // ���콺 ������ ��ư�� Ŭ���ϸ�
            if (mouseAction == 1)  // ���콺 ���� : 0, ������ : 1, �� : 2
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
                        if (!isZoomMode)
                        {
                            // �÷��̾� ī�޶��� �þ߰��� 15�� ������.
                            Camera.main.fieldOfView = 15;
                            isZoomMode = true;

                            crossHairZoom.SetActive(true);
                            crossHair2.SetActive(false);
                        }
                        // �ܸ�� ���¶��
                        else
                        {
                            Camera.main.fieldOfView = 60;
                            isZoomMode = false;

                            crossHairZoom.SetActive(false);
                            crossHair2.SetActive(true);
                        }
                        break;
                }

            }

            // ���콺 ���� ��ư�� Ŭ���ϸ�
            else if (mouseAction == 0)
            {
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
                if (Physics.Raycast(ray, out hitInfo))
                {
                    // �ε��� ��ü�� ������ �� ��ġ�� �ǰ� ȿ���� (���� ���� ��������)�����.
                    // hitInfo.point : ���� �΋H�� ��ġ
                    hitEffect.transform.position = hitInfo.point;
                    // hitInfo.normal : �ε��� �κ��� normal ����. ƨ���� ���� ������ �������ش�.
                    hitEffect.transform.forward = hitInfo.normal;

                    // �ǰ� ����Ʈ�� ����Ѵ�.
                    particleSys.Play();

                    // �ǰ� ����� ���̶�� ������ �ֱ�
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        EnemyFSM enemyFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                        enemyFSM.DamageAction(weaponPower);
                    }
                }

                // 5. �ѱ� ����Ʈ ������ ���� �ڷ�ƾ ����
                StartCoroutine(ShootEffOn(0.05f));
            }

            // 4. Ű���� ���� 1�� Ű�ٿ� : ������-��ָ��
            //    Ű���� ���� 2�� Ű�ٿ� : ������-�������۸��
            else if (mouseAction == 2)
            {
                weaponModeTxt.text = "Rifle Mode";
                weaponMode = WeaponMode.Normal;

                // ī�޶� FoV�� ó�� ���·� �ʱ�ȭ
                Camera.main.fieldOfView = 60;

                crossHair1.SetActive(true);
                crossHair2.SetActive(false);

                rifleImg.SetActive(true);
                sniperImg.SetActive(false);
                grenadeImge.SetActive(true);
            }
            else if (mouseAction == 3)
            {
                weaponModeTxt.text = "Sniper Mode";
                weaponMode = WeaponMode.Sniper;

                crossHair1.SetActive(false);
                crossHair2.SetActive(true);

                rifleImg.SetActive(false);
                sniperImg.SetActive(true);
                grenadeImge.SetActive(false);
            }
        }
    }

    // RPC�� ���� ����
    int mouseAction = 4;
    [PunRPC]
    void SendMouseAction(int input)
    {
        mouseAction = input;
    }

    // 5. �� �߻� �� ���� �ð� �Ŀ� ������� �ѱ� ����Ʈ Ȱ��ȭ.
    IEnumerator ShootEffOn(float duration)
    {
        // ������ �ѱ� ����Ʈ Ȱ��ȭ
        int randNum = Random.Range(0, fireFlashEffs.Length-1);
        fireFlashEffs[randNum].SetActive(true);

        // ���� �ð� �Ŀ� �������.
        yield return new WaitForSeconds(duration);
        fireFlashEffs[randNum].SetActive(false);
    }

    // ���콺 �Է� ���� ����, �ޱ�.
    int receivedMouseBtn;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(receivedMouseBtn);
        }
        else
        {
            receivedMouseBtn = (int)stream.ReceiveNext();
        }
    }
}
