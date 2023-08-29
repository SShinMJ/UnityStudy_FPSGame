using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    PlayerMovement player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player.playerHp = 0;
    }
}
