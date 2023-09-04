using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : �÷��̾ Ű�Է¿� ���� �̵�.
public class PunPlayerMovement : MonoBehaviour
{
    // �̵� �ӵ�
    public float speed = 3;

    PhotonView pw;

    private void Start()
    {
        pw = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pw.IsMine)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, 0, -speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
        }
    }
}
