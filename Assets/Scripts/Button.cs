using Unity.AI.Navigation;
using UnityEngine;

// 목적 : 플레이어가 버튼을 누르면 다리가 켜지고, 네비게이션 메시를 다시 만든다.
public class Button : MonoBehaviour
{
    // 다리 게임 오브젝트, navMeshSurface
    public GameObject bridge;
    public NavMeshSurface navMeshSurface;

    void Start()
    {
        // 처음엔 다리가 보이지 않는다.
        bridge.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 다리가 보이게 한다.
            bridge.SetActive(true);

            // 네비게이션 메시를 다시 만든다.
            navMeshSurface.BuildNavMesh();
        }
    }
}
