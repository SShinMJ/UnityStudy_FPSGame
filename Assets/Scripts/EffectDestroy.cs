using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이팩트는 특정 시간이 지나면 삭제된다.
public class EffectDestroy : MonoBehaviour
{
    public float destroyTime = 1.5f;

    float currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
    }

}
