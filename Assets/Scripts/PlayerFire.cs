using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : ���콺 ������ ��ư�� ���� ��ź�� Ư�� ��ġ�� �����ϰ�, �������� �߻�.
public class PlayerFire : MonoBehaviour
{
    // ��ź ���� ������Ʈ
    public GameObject bombPrf;
    // �߻� ��ġ
    public GameObject firePosition;
    public float firePower = 5f;

    void Update()
    {
        // ���콺 ������ ��ư�� Ŭ���ϸ�
        if(Input.GetMouseButton(1))  // ���콺 ���� : 0, ������ : 1, �� : 2
        {
            GameObject bombObj = Instantiate(bombPrf);
            // ��ź �߻� ��ġ�� fireposition ������ �Ͽ� Ư�� ��ġ�� �����ǰ� �Ѵ�.
            bombObj.transform.position = firePosition.transform.position;

            // ��ź ������Ʈ�� rigidBody ���� ������ ��, ��(����)�� ���Ѵ�.
            Rigidbody rigidbody = bombObj.GetComponent<Rigidbody>();
            // ī�޶��� ����(forward)�������� firePower�� ���� ����ä, ������ ������ �޴´�.(Impulse: ����)
            rigidbody.AddForce(Camera.main.transform.forward * firePower, ForceMode.Impulse);
        }
    }
}
