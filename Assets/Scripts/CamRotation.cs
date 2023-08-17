using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목표 : 마우스의 입력 받아 카메라 회전
public class CamRotation : MonoBehaviour
{
    // 마우스 이동 속도(감도)
    public float speed = 10f;
    float mx = 0;
    float my = 0;

    private void Start()
    {
        transform.eulerAngles = Vector3.zero;
    }

    void Update()
    {
        // 1. 마우스 입력 받기(X, Y 좌표 값, 마우스 이동 속도)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 상하 값을 -90~90도로 고정
        mx += mouseX * speed * Time.deltaTime;
        my += mouseY * speed * Time.deltaTime;
        my = Mathf.Clamp(my, -90f, 90f);

        // 3. 입력에 따라 카메라 회전 방향 설정
        Vector3 dir = new Vector3(-my, mx, 0);

        // 4. 정해진 방향으로 카메라 회전
        // r = r0 + vt
        transform.eulerAngles = dir;
        // 현재 방향 = 현재 방향 + 이동할 방향*속도*평준화
    }
}
