using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ǥ : ��ź�� ��ü�� �ε����� ����Ʈ�� ����� �Բ� �ı��ȴ�.
// ��ǥ2: ����ȿ�� �ݰ� ������ ���̾ 'Enemy'�� ��� ���� ������Ʈ�� Collider�� �����Ͽ�
//        �ش� �� ���ӿ�����Ʈ���� ����ź �������� �ش�.
public class BombAction : MonoBehaviour
{
    // ���� ����Ʈ�� �����´�.
    public GameObject bombEffect;

    // 2. ���� ȿ�� �ݰ�
    public float explosionRadius = 5f;
    public int damage = 3;

    private void OnCollisionEnter(Collision collision)
    {
        // 2. ���� �ݰ� �� Enemy ������Ʈ ��������
        //    OverLapSphere(��ġ����, �ݰ�, Ư���� layer ��ȣ) : �� �ݰ� �� ������Ʈ Ž��
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8 | 6);
        //     == Collider[] cols = Physics.OverlapSphere(���͵���, ����, LayerMask.GetMask("Enemy") | LayerMask.GetMask("Player"));

        // ����� �ֱ�
        for(int i = 0; i<cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().DamageAction(damage);
        }

        GameObject bombEffObj = Instantiate(bombEffect);
        bombEffObj.transform.position = transform.position;

        Destroy(gameObject);
    }
}
