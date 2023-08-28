using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� : ������� ���������� �Է��Ͽ� �����ϰų�(ȸ������)
// ����� �����͸� �о ���� ���� ���� ���ο� ���� �α���.

// ����2: ȸ�������ϱ� ���� ���̵�� �н����带 ����.
// ����3: �Է��� ���� ���, �Է��� ä���޶�� �޽����� ���� �ؽ�Ʈ�� ����.
public class LoginManager : MonoBehaviour
{
    // ID InputField, PWD InputField, ���� �ؽ�Ʈ
    public TMP_InputField idInput;
    public TMP_InputField pwdInput;
    public TMP_Text authTxt;

    void Start()
    {
        authTxt.text = string.Empty;
    }

    public void SignUP()
    {
        // �̹� ����Ǿ� ���� �ʴٸ�(�ش� id Ű���� �������� �ʴ� ���)
        if (!PlayerPrefs.HasKey(idInput.text))
        {
            // id�� key, pwd�� value�� ����ȴ�.
            PlayerPrefs.SetString(idInput.text, pwdInput.text);
            authTxt.text = "Signup Sucesses";
        }
        else
        {
            authTxt.text = "ID Failed";
        }
    }

    public void Login()
    {
        if (!CheckInput(idInput.text, pwdInput.text))
            return;

        if(PlayerPrefs.HasKey(idInput.text))
        {
            string password = PlayerPrefs.GetString(idInput.text);
            if (pwdInput.text == password)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                authTxt.text = "Login Failed";
            }
        }
        else
        {
            authTxt.text = "Login Failed";
        }
    }

    bool CheckInput(string id, string pwd)
    {
        if(id == "" || pwd == "")
        {
            authTxt.text = "Please Input ID or PWD";
            return false;
        }

        return true;
    }
}
