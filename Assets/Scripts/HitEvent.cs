using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : �÷��̾�� �������� ������ ������.
public class HitEvent : MonoBehaviour
{
    public EnemyFSM efsm;

    // �÷��̾�� �������� ������.
    public void HitPlayer()
    {
        // player hp�� ���ҽ�Ű�� �Լ��� �����Ѵ�.
        efsm.AttackAction();
    }
}
