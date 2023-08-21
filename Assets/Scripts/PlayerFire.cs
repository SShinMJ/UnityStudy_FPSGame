using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

// ���� : ���콺 ������ ��ư�� ���� ��ź�� Ư�� ��ġ�� �����ϰ�, �������� �߻�.
// ����2: ���콺 ���� ��ư�� ���� �ü� �������� ���� �߻�.
public class PlayerFire : MonoBehaviour
{
    // ��ź ���� ������Ʈ
    public GameObject bombPrf;
    // �߻� ��ġ
    public GameObject firePosition;
    public float firePower = 5f;

    // �� �ǰ� ȿ��
    public GameObject hitEffect;
    // ����Ʈ ��ƼŬ �ý���
    ParticleSystem particleSys;

    private void Start()
    {
        particleSys = hitEffect.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // ���콺 ������ ��ư�� Ŭ���ϸ�
        if (Input.GetMouseButtonDown(1))  // ���콺 ���� : 0, ������ : 1, �� : 2
        {
            GameObject bombObj = Instantiate(bombPrf);
            // ��ź �߻� ��ġ�� fireposition ������ �Ͽ� Ư�� ��ġ�� �����ǰ� �Ѵ�.
            bombObj.transform.position = firePosition.transform.position;

            // ��ź ������Ʈ�� rigidBody ���� ������ ��, ��(����)�� ���Ѵ�.
            Rigidbody rigidbody = bombObj.GetComponent<Rigidbody>();
            // ī�޶��� ����(forward)�������� firePower�� ���� ����ä, ������ ������ �޴´�.(Impulse: ����)
            rigidbody.AddForce(Camera.main.transform.forward * firePower, ForceMode.Impulse);
        }

        // ���콺 ���� ��ư�� Ŭ���ϸ�
        if (Input.GetMouseButtonDown(0))
        {
            // ����ĳ������ �����ϰ� �߻� ��ġ�� �߻� ������ �����Ѵ�.
            // Ray�� ����ü�� �����Ǿ� �ִ�.�������� �ڷ����� ���� �� ������ ���� �� �ִ�.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // ����ĳ������ �ε��� ����� ������ ������ �� �ִ� ������ �����.
            RaycastHit hitInfo = new RaycastHit();

            // ���̸� �߻��ϰ� (hitInfo�� �ε��� ��ü�� ������ true�� ��ȯ)
            if(Physics.Raycast(ray, out hitInfo))
            {
                // �ε��� ��ü�� ������ �� ��ġ�� �ǰ� ȿ���� (���� ���� ��������)�����.
                // hitInfo.point : ���� �΋H�� ��ġ
                hitEffect.transform.position = hitInfo.point;
                // hitInfo.normal : �ε��� �κ��� normal ����. ƨ���� ���� ������ �������ش�.
                hitEffect.transform.forward = hitInfo.normal;

                // �ǰ� ����Ʈ�� ����Ѵ�.
                if (!particleSys.isPlaying) // ����� ��
                    particleSys.Play();
            }
        }
    }
}
