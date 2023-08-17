using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ǥ : ���콺�� �Է� �޾� ī�޶� ȸ��
public class CamRotation : MonoBehaviour
{
    // ���콺 �̵� �ӵ�(����)
    public float speed = 10f;
    float mx = 0;
    float my = 0;

    private void Start()
    {
        transform.eulerAngles = Vector3.zero;
    }

    void Update()
    {
        // 1. ���콺 �Է� �ޱ�(X, Y ��ǥ ��, ���콺 �̵� �ӵ�)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. ���� ���� -90~90���� ����
        mx += mouseX * speed * Time.deltaTime;
        my += mouseY * speed * Time.deltaTime;
        my = Mathf.Clamp(my, -90f, 90f);

        // 3. �Է¿� ���� ī�޶� ȸ�� ���� ����
        Vector3 dir = new Vector3(-my, mx, 0);

        // 4. ������ �������� ī�޶� ȸ��
        // r = r0 + vt
        transform.eulerAngles = dir;
        // ���� ���� = ���� ���� + �̵��� ����*�ӵ�*����ȭ
    }
}
