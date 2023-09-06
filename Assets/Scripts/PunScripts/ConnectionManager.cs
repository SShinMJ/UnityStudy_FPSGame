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
    public void Connect()
    {
        // AutomaticallySyncScene: ������ Ŭ���̾�Ʈ�� �Ϲ� Ŭ���̾�Ʈ���� ������ ����ȭ���� ����
        // true�� �����ϸ� ������ Ŭ�󿡼� LoadLevel()�� ������ �����ϸ� ��� Ŭ���̾�Ʈ���� �ڵ����� ������ ������ �ε�.
        PhotonNetwork.AutomaticallySyncScene = true;

        // 1�ʿ� PhotonNetwork ��������� �� �� ���� ������ ����. default�� 30
        PhotonNetwork.SendRate = 30;
        // 1�ʿ� PhotonNetwork���� �����͸� �� �� ���� ������ ����. default 10.
        // �ٸ� �÷��̾��� �̵��� ���� ���� ���̱� ���� ���� �ø���.
        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.ConnectUsingSettings();
    }

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

    public void EnterSoloPlayeMode()
    {
        // Loading ������ �̵�
        SceneManager.LoadScene("LoadingScene");
    }
}
