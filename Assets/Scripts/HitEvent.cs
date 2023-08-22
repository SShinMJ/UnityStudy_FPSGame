using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적 : 플레이어에게 데미지를 입히기 입힌다.
public class HitEvent : MonoBehaviour
{
    public EnemyFSM efsm;

    // 플레이어에게 데미지를 입힌다.
    public void HitPlayer()
    {
        // player hp를 감소시키는 함수를 실행한다.
        efsm.AttackAction();
    }
}
