using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

// 목적 : 마우스 오른쪽 버튼을 눌러 폭탄을 특정 위치에 생성하고, 정면으로 발사.
// 목적2: 마우스 왼쪽 버튼을 눌러 시선 방향으로 총을 발사.

// 목적3: 이동 Blend Tree의 파라미터 값이 0일 때, Attack Trigger 시전.
//                        (Blend Tree가 수행중일 때 tigger을 키면 오류날 수 있음)

// 목적4: 키보드 특정 키 입력으로 무기 모드(일반/저격) 전환

// 목적5: 총을 발사할 때, 일정 시간 후에 사라지는 총구 이팩트를 활성화한다.

public class PlayerFire : MonoBehaviour, IPunObservable
{
    // 폭탄 게임 오브젝트
    public GameObject bombPrf;
    // 발사 위치
    public GameObject firePosition;
    public float firePower = 5f;

    // 총 피격 효과
    public GameObject hitEffect;
    // 이펙트 파티클 시스템
    ParticleSystem particleSys;
    // 피격 데미지
    public int weaponPower = 1;

    // 3. 자식 오브젝트의 애니메이터
    Animator animator;

    // 4. 무기모드 열거형 변수(무기 상태)
    public enum WeaponMode
    {
        Normal,
        Sniper
    }
    WeaponMode weaponMode = WeaponMode.Normal;

    // 줌 여부 확인 변수
    bool isZoomMode = false;

    // 무기 모드 확인 텍스트 UI
    public TMP_Text weaponModeTxt;

    // 5. 총구 이팩트 배열
    public GameObject[] fireFlashEffs;

    // 초점1(라이플), 초점2(스나이퍼)
    public GameObject crossHair1;
    public GameObject crossHair2;
    public GameObject crossHairZoom;

    // 라이플, 스나이퍼, 수류탄 이미지
    public GameObject rifleImg;
    public GameObject sniperImg;
    public GameObject grenadeImge;

    PhotonView photonView;

    private void Start()
    {
        particleSys = hitEffect.GetComponent<ParticleSystem>();

        // 3. 자식 오브젝트의 애니메이터
        animator = GetComponentInChildren<Animator>();

        // 무기 모드 초기화
        weaponModeTxt.text = "Rifle Mode";

        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        // 나의 입력일 경우
        if (photonView.IsMine)
        {
            // GameManager에서 'Start' 상태가 아니라면 조작 불가.
            if (GameManager.Instance.status != GameManager.GameStatus.Start)
            {
                return;
            }

            // 마우스 오른쪽 버튼을 클릭하면
            if (Input.GetMouseButtonDown(1))  // 마우스 왼쪽 : 0, 오른쪽 : 1, 휠 : 2
            {
                // 입력될 시 상대 플레이어에게 값이 보내진다.
                // RPC의 해당 함수의 모든 타겟에게 해당 값이 전해진다.
                photonView.RPC("SendMouseAction", RpcTarget.All, 1);

                // 4. 키보드 특정 키 입력으로 무기 모드(일반/저격) 전환
                // 무기 모드(상태)에 따라 
                switch (weaponMode)
                {
                    // 노멀 모드 : 마우스 오른쪽 클릭 시 폭탄을 던진다.
                    case WeaponMode.Normal:
                        GameObject bombObj = Instantiate(bombPrf);
                        // 폭탄 발사 위치를 fireposition 값으로 하여 특정 위치에 생성되게 한다.
                        bombObj.transform.position = firePosition.transform.position;

                        // 폭탄 오브젝트의 rigidBody 값을 가져온 후, 힘(폭팔)을 더한다.
                        Rigidbody rigidbody = bombObj.GetComponent<Rigidbody>();
                        // 카메라의 정면(forward)방향으로 firePower을 힘을 가진채, 질량의 영향을 받는다.(Impulse: 질량)
                        rigidbody.AddForce(Camera.main.transform.forward * firePower, ForceMode.Impulse);
                        break;

                    // 스나이퍼 모드 : 마우스 오른쪽 클릭 시 화면이 줌(확대) 된다.
                    case WeaponMode.Sniper:
                        // 줌모드 상태가 아니라면
                        if (!isZoomMode)
                        {
                            // 플레이어 카메라의 시야각을 15도 좁힌다.
                            Camera.main.fieldOfView = 15;
                            isZoomMode = true;

                            crossHairZoom.SetActive(true);
                            crossHair2.SetActive(false);
                        }
                        // 줌모드 상태라면
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

            // 마우스 왼쪽 버튼을 클릭하면
            else if (Input.GetMouseButtonDown(0))
            {
                // 입력될 시 상대 플레이어에게 값이 보내진다.
                photonView.RPC("SendMouseAction", RpcTarget.All, 0);

                // Blend Tree의 파라미터 값이 0이면,
                if (animator.GetFloat("MoveMotion") <= 0.1)
                {
                    // 피격 애니메이션 실행
                    animator.SetTrigger("Attack");
                }

                // 레이캐스팅을 생성하고 발사 위치와 발사 방향을 설정한다.
                // Ray는 구조체로 구성되어 있다.여러가지 자료형을 넣을 수 있으며 묶을 수 있다.
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                // 레이캐스팅이 부딪힌 대상의 정보를 저장할 수 있는 변수를 만든다.
                RaycastHit hitInfo = new RaycastHit();

                // 레이를 발사하고 (hitInfo는 부딪힌 물체가 있으면 true를 반환)
                if (Physics.Raycast(ray, out hitInfo))
                {
                    // 부딪힌 물체가 있으면 그 위치에 피격 효과를 (법선 벡터 방향으로)만든다.
                    // hitInfo.point : 현재 부딫힌 위치
                    hitEffect.transform.position = hitInfo.point;
                    // hitInfo.normal : 부딪힌 부분의 normal 벡터. 튕겨져 나갈 방향을 지정해준다.
                    hitEffect.transform.forward = hitInfo.normal;

                    // 피격 이팩트를 재생한다.
                    particleSys.Play();

                    // 피격 대상이 적이라면 데미지 주기
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        EnemyFSM enemyFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                        enemyFSM.DamageAction(weaponPower);
                    }
                }

                // 5. 총구 이팩트 실행을 위한 코루틴 실행
                StartCoroutine(ShootEffOn(0.05f));
            }

            // 4. 키보드 숫자 1번 키다운 : 무기모드-노멀모드
            //    키보드 숫자 2번 키다운 : 무기모드-스나이퍼모드
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 입력될 시 상대 플레이어에게 값이 보내진다.
                photonView.RPC("SendMouseAction", RpcTarget.All, 2);

                weaponModeTxt.text = "Rifle Mode";
                weaponMode = WeaponMode.Normal;

                // 카메라 FoV를 처음 상태로 초기화
                Camera.main.fieldOfView = 60;

                crossHair1.SetActive(true);
                crossHair2.SetActive(false);

                rifleImg.SetActive(true);
                sniperImg.SetActive(false);
                grenadeImge.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 입력될 시 상대 플레이어에게 값이 보내진다.
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
        // 상대 플레이어의 입력일 경우,
        else
        {
            // GameManager에서 'Start' 상태가 아니라면 조작 불가.
            if (GameManager.Instance.status != GameManager.GameStatus.Start)
            {
                return;
            }

            // 마우스 오른쪽 버튼을 클릭하면
            if (mouseAction == 1)  // 마우스 왼쪽 : 0, 오른쪽 : 1, 휠 : 2
            {
                // 4. 키보드 특정 키 입력으로 무기 모드(일반/저격) 전환
                // 무기 모드(상태)에 따라 
                switch (weaponMode)
                {
                    // 노멀 모드 : 마우스 오른쪽 클릭 시 폭탄을 던진다.
                    case WeaponMode.Normal:
                        GameObject bombObj = Instantiate(bombPrf);
                        // 폭탄 발사 위치를 fireposition 값으로 하여 특정 위치에 생성되게 한다.
                        bombObj.transform.position = firePosition.transform.position;

                        // 폭탄 오브젝트의 rigidBody 값을 가져온 후, 힘(폭팔)을 더한다.
                        Rigidbody rigidbody = bombObj.GetComponent<Rigidbody>();
                        // 카메라의 정면(forward)방향으로 firePower을 힘을 가진채, 질량의 영향을 받는다.(Impulse: 질량)
                        rigidbody.AddForce(Camera.main.transform.forward * firePower, ForceMode.Impulse);
                        break;

                    // 스나이퍼 모드 : 마우스 오른쪽 클릭 시 화면이 줌(확대) 된다.
                    case WeaponMode.Sniper:
                        // 줌모드 상태가 아니라면
                        if (!isZoomMode)
                        {
                            // 플레이어 카메라의 시야각을 15도 좁힌다.
                            Camera.main.fieldOfView = 15;
                            isZoomMode = true;

                            crossHairZoom.SetActive(true);
                            crossHair2.SetActive(false);
                        }
                        // 줌모드 상태라면
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

            // 마우스 왼쪽 버튼을 클릭하면
            else if (mouseAction == 0)
            {
                // Blend Tree의 파라미터 값이 0이면,
                if (animator.GetFloat("MoveMotion") <= 0.1)
                {
                    // 피격 애니메이션 실행
                    animator.SetTrigger("Attack");
                }

                // 레이캐스팅을 생성하고 발사 위치와 발사 방향을 설정한다.
                // Ray는 구조체로 구성되어 있다.여러가지 자료형을 넣을 수 있으며 묶을 수 있다.
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                // 레이캐스팅이 부딪힌 대상의 정보를 저장할 수 있는 변수를 만든다.
                RaycastHit hitInfo = new RaycastHit();

                // 레이를 발사하고 (hitInfo는 부딪힌 물체가 있으면 true를 반환)
                if (Physics.Raycast(ray, out hitInfo))
                {
                    // 부딪힌 물체가 있으면 그 위치에 피격 효과를 (법선 벡터 방향으로)만든다.
                    // hitInfo.point : 현재 부딫힌 위치
                    hitEffect.transform.position = hitInfo.point;
                    // hitInfo.normal : 부딪힌 부분의 normal 벡터. 튕겨져 나갈 방향을 지정해준다.
                    hitEffect.transform.forward = hitInfo.normal;

                    // 피격 이팩트를 재생한다.
                    particleSys.Play();

                    // 피격 대상이 적이라면 데미지 주기
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        EnemyFSM enemyFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                        enemyFSM.DamageAction(weaponPower);
                    }
                }

                // 5. 총구 이팩트 실행을 위한 코루틴 실행
                StartCoroutine(ShootEffOn(0.05f));
            }

            // 4. 키보드 숫자 1번 키다운 : 무기모드-노멀모드
            //    키보드 숫자 2번 키다운 : 무기모드-스나이퍼모드
            else if (mouseAction == 2)
            {
                weaponModeTxt.text = "Rifle Mode";
                weaponMode = WeaponMode.Normal;

                // 카메라 FoV를 처음 상태로 초기화
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

    // RPC를 위한 변수
    int mouseAction = 4;
    [PunRPC]
    void SendMouseAction(int input)
    {
        mouseAction = input;
    }

    // 5. 총 발사 시 일정 시간 후에 사라지는 총구 이팩트 활성화.
    IEnumerator ShootEffOn(float duration)
    {
        // 랜덤한 총구 이팩트 활성화
        int randNum = Random.Range(0, fireFlashEffs.Length-1);
        fireFlashEffs[randNum].SetActive(true);

        // 일정 시간 후에 사라진다.
        yield return new WaitForSeconds(duration);
        fireFlashEffs[randNum].SetActive(false);
    }

    // 마우스 입력 값을 전달, 받기.
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
