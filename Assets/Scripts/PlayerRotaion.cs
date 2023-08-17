using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목표 : 마우스의 입력 받아 플레이어 회전.(좌우만 돌아가게 한다.)
public class PlayerRotaion : MonoBehaviour
{
    // 마우스 이동 속도(감도)
    public float speed = 10f;
    void Update()
    {
        // 1. 마우스 입력 받기(X 좌표 값만, 마우스 이동 속도)
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 dir = new Vector3(0, mouseX, 0);
        transform.eulerAngles = transform.eulerAngles + dir * speed * Time.deltaTime;
    }
}
