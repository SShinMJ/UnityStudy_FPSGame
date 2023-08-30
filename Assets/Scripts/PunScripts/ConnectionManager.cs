using Photon.Pun;
using UnityEngine.SceneManagement;

// ���� : Photon ���� ������ �����Ѵ�.
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 1. Photon ���� ������ ����. 
        Connect();
    }

    // �����ڸ� �ڵ�� ���ٽ����� ǥ�� �����ϴ�.
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    // PUN�� ���ǵ� �Լ��� ���.(f12���� Ȯ�� ����)
    // ���� ���� ����� ȣ��ȴ�.
    public override void OnConnected()
    {
        base.OnConnected();
/*        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("Callback - �����.");*/
    }

    // 2. ������ ���� ���� �� ȣ��ȴ�.
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    // JoinLobby : Photon�� �����ϴ� �Լ�.
    // ��ư Ŭ�� �� ����ȴ�.
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        // Lobby ������ �̵�
        SceneManager.LoadScene("LobbyScene");
    }
}
