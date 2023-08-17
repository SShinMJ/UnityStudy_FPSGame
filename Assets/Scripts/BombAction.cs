using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ǥ : ��ź�� ��ü�� �ε����� ����Ʈ�� ����� �Բ� �ı��ȴ�.
public class BombAction : MonoBehaviour
{
    // ���� ����Ʈ�� �����´�.
    public GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject bombEffObj = Instantiate(bombEffect);
        bombEffObj.transform.position = transform.position;

        Destroy(gameObject);
    }
}
