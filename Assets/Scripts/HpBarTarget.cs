using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : HP���� �չ����� Ÿ���� �� �������� ���Ѵ�. (�÷��̾� ���鿡 �ٰ� ���δ�)
public class HpBarTarget : MonoBehaviour
{
    // Ÿ��(�÷��̾� ���� ����)
    public Transform target;

    void Update()
    {
        transform.forward = target.forward;
    }
}
