using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목표 : 폭탄이 물체에 부딪히면 이팩트를 만들고 함께 파괴된다.
public class BombAction : MonoBehaviour
{
    // 폭발 이팩트를 가져온다.
    public GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject bombEffObj = Instantiate(bombEffect);
        bombEffObj.transform.position = transform.position;

        Destroy(gameObject);
    }
}
