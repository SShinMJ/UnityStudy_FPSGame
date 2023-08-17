using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ʈ�� Ư�� �ð��� ������ �����ȴ�.
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
