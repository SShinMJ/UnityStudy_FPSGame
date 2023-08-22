using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적 : HP바의 앞방향을 타겟의 앞 방향으로 향한다. (플레이어 정면에 바가 보인다)
public class HpBarTarget : MonoBehaviour
{
    // 타겟(플레이어 정면 방향)
    public Transform target;

    void Update()
    {
        transform.forward = target.forward;
    }
}
