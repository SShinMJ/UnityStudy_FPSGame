using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// W, A, S, D 입력 이동
// 캐릭터 컨트롤러 : '스페이스바'-수직점프

// 목적2 : 플레이어가 공격을 받으면 hp가 damage만큼 감소.

// 목적3 : 현재 플레이어 hp(%)를 hp 슬라이더에 적용.

// 목적4 : 적의 공격을 받을 때, Hit Image를 켰다가 끈다.
// 목적5 : hp가 0이된 경우 hit Image의 알파값을 255로 만든다.

// 목적6 : GameManager의 'Ready'상태에는 플레이어, 적 모두 움직일 수 없다.
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    // 캐릭터 컨트롤러, 중력, 수직 속력 변수
    CharacterController characterController;
    float gravity = -20f;
    float yVelocity = 0;

    // 점프 힘
    public float jumpForce = 5f;
    // 점프 상태 변수
    public bool isJumping = false;

    // 2. hp
    public int playerHp = 10;

    // 3. maxHP
    int maxHp = 10;
    // Slider
    public Slider hpSlider;

    // 4. Hit Image 게임오브젝트
    public GameObject hitImg;

    // 5. 현재 시간, hitImage 종료시간
    float currentTime;
    public float hitImageEndTime = 3f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        maxHp = playerHp;
    }

    void Update()
    {
        // 3. 현재 플레이어 hp를 hp슬라이더에 적용
        hpSlider.value = (float)playerHp / (float)maxHp;

        // GameManager에서 'Start' 상태가 아니라면 조작 불가.
        if (GameManager.Instance.status != GameManager.GameStatus.Start)
        {
            return;
        }

        // 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        // 점프가 끝났다면(캐릭터가 바닥에 닿아 있다면) (CollisionFlags.Below : 바닥)
        if (isJumping && characterController.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
        }

        // 바닥에 닿아있을 경우엔, 수직 속도를 받지 않으므로
        if (characterController.collisionFlags == CollisionFlags.Below)
        {
            // 수직 속도 초기화
            yVelocity = 0;
        }

        // 스페이스바(점프) 입력 시 점프 상태가 아니라면
        if (Input.GetButtonDown("Jump") && !isJumping) // == Input.GetKeyDown(KeyCode.Space)
        {
            yVelocity = jumpForce;
            isJumping = true;
        }

        // 이동 방향 설정
        // 절대(월드) 좌표 방식(오브젝트 축(회전방향)과 관계 없이 움직인다)
        Vector3 dir = new Vector3 (h, 0, v);
        // 상대(로컬) 좌표 방식 => 카메라의 축으로 설정
        dir = Camera.main.transform.TransformDirection(dir);

        // 캐릭터 수직 속도에 중력 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 1) 이동 속도로 플레이어 이동
        //transform.position += dir * speed * Time.deltaTime;
        // 2) 캐릭터 컨트롤러로 플레이어 이동
        characterController.Move(dir * speed * Time.deltaTime);
    }

    // 2. hp가 damage만큼 감소.
    public void DamageAction(int damage)
    {
        playerHp -= damage;

        // hitImage 껏다 키기
        if (playerHp > 0)
        {
            StartCoroutine(PlayerHitEff());
        }
        // HP가 0이 될 경우, 
        else
        {
            StartCoroutine(DeadEff());
        }
    }

    IEnumerator PlayerHitEff()
    {
        // hitImage 활성화
        hitImg.SetActive(true);

        // 0.5초 대기
        yield return new WaitForSeconds(0.2f);

        // hitImage 비활성화
        hitImg.SetActive(false);
    }

    // HitImage의 알파값을 현재 값에서 255로 만들어준다.
    IEnumerator DeadEff()
    {
        // hitImage 활성화
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
