using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목표 : 폭탄이 물체에 부딪히면 이팩트를 만들고 함께 파괴된다.
// 목표2: 폭발효과 반경 내에서 레이어가 'Enemy'인 모든 게임 오브젝트의 Collider를 저장하여
//        해당 적 게임오브젝트에게 수류탄 데미지를 준다.
public class BombAction : MonoBehaviour
{
    // 폭발 이팩트를 가져온다.
    public GameObject bombEffect;

    // 2. 폭발 효과 반경
    public float explosionRadius = 5f;
    public int damage = 3;

    private void OnCollisionEnter(Collision collision)
    {
        // 2. 폭발 반경 내 Enemy 오브젝트 가져오기
        //    OverLapSphere(위치벡터, 반경, 특정할 layer 번호) : 원 반경 내 오브젝트 탐지
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8 | 6);
        //     == Collider[] cols = Physics.OverlapSphere(위와동일, 동일, LayerMask.GetMask("Enemy") | LayerMask.GetMask("Player"));

        // 대미지 주기
        for(int i = 0; i<cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().DamageAction(damage);
        }

        GameObject bombEffObj = Instantiate(bombEffect);
        bombEffObj.transform.position = transform.position;

        Destroy(gameObject);
    }
}
