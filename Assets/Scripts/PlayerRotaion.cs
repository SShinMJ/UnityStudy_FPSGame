using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ǥ : ���콺�� �Է� �޾� �÷��̾� ȸ��.(�¿츸 ���ư��� �Ѵ�.)
public class PlayerRotaion : MonoBehaviour
{
    // ���콺 �̵� �ӵ�(����)
    public float speed = 10f;
    void Update()
    {
        // 1. ���콺 �Է� �ޱ�(X ��ǥ ����, ���콺 �̵� �ӵ�)
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 dir = new Vector3(0, mouseX, 0);
        transform.eulerAngles = transform.eulerAngles + dir * speed * Time.deltaTime;
    }
}
