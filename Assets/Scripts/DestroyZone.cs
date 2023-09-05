using Photon.Realtime;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    PlayerMovement player;

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerMovement>();
        player.playerHp = 0;
    }
}
