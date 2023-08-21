using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

// 목적 : 마우스 오른쪽 버튼을 눌러 폭탄을 특정 위치에 생성하고, 정면으로 발사.
// 목적2: 마우스 왼쪽 버튼을 눌러 시선 방향으로 총을 발사.
public class PlayerFire : MonoBehaviour
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

    private void Start()
    {
        particleSys = hitEffect.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // 마우스 오른쪽 버튼을 클릭하면
        if (Input.GetMouseButtonDown(1))  // 마우스 왼쪽 : 0, 오른쪽 : 1, 휠 : 2
        {
            GameObject bombObj = Instantiate(bombPrf);
            // 폭탄 발사 위치를 fireposition 값으로 하여 특정 위치에 생성되게 한다.
            bombObj.transform.position = firePosition.transform.position;

            // 폭탄 오브젝트의 rigidBody 값을 가져온 후, 힘(폭팔)을 더한다.
            Rigidbody rigidbody = bombObj.GetComponent<Rigidbody>();
            // 카메라의 정면(forward)방향으로 firePower을 힘을 가진채, 질량의 영향을 받는다.(Impulse: 질량)
            rigidbody.AddForce(Camera.main.transform.forward * firePower, ForceMode.Impulse);
        }

        // 마우스 왼쪽 버튼을 클릭하면
        if (Input.GetMouseButtonDown(0))
        {
            // 레이캐스팅을 생성하고 발사 위치와 발사 방향을 설정한다.
            // Ray는 구조체로 구성되어 있다.여러가지 자료형을 넣을 수 있으며 묶을 수 있다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 레이캐스팅이 부딪힌 대상의 정보를 저장할 수 있는 변수를 만든다.
            RaycastHit hitInfo = new RaycastHit();

            // 레이를 발사하고 (hitInfo는 부딪힌 물체가 있으면 true를 반환)
            if(Physics.Raycast(ray, out hitInfo))
            {
                // 부딪힌 물체가 있으면 그 위치에 피격 효과를 (법선 벡터 방향으로)만든다.
                // hitInfo.point : 현재 부딫힌 위치
                hitEffect.transform.position = hitInfo.point;
                // hitInfo.normal : 부딪힌 부분의 normal 벡터. 튕겨져 나갈 방향을 지정해준다.
                hitEffect.transform.forward = hitInfo.normal;

                // 피격 이팩트를 재생한다.
                if (!particleSys.isPlaying) // 재생할 때
                    particleSys.Play();
            }
        }
    }
}
