using Unity.AI.Navigation;
using UnityEngine;

// ���� : �÷��̾ ��ư�� ������ �ٸ��� ������, �׺���̼� �޽ø� �ٽ� �����.
public class Button : MonoBehaviour
{
    // �ٸ� ���� ������Ʈ, navMeshSurface
    public GameObject bridge;
    public NavMeshSurface navMeshSurface;

    void Start()
    {
        // ó���� �ٸ��� ������ �ʴ´�.
        bridge.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �ٸ��� ���̰� �Ѵ�.
            bridge.SetActive(true);

            // �׺���̼� �޽ø� �ٽ� �����.
            navMeshSurface.BuildNavMesh();
        }
    }
}
