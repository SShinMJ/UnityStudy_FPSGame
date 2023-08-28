using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// 목적 : 사용자의 개인정보를 입력하여 저장하거나(회원가입)
// 저장된 데이터를 읽어서 개인 정보 저장 여부에 따라 로그인.

// 목적2: 회원가입하기 위해 아이디와 패스워드를 저장.
// 목적3: 입력이 없을 경우, 입력을 채워달라는 메시지를 인증 텍스트에 띄운다.
public class LoginManager : MonoBehaviour
{
    // ID InputField, PWD InputField, 인증 텍스트
    public TMP_InputField idInput;
    public TMP_InputField pwdInput;
    public TMP_Text authTxt;

    void Start()
    {
        authTxt.text = string.Empty;
    }

    public void SignUP()
    {
        // 이미 저장되어 있지 않다면(해당 id 키값이 존재하지 않는 경우)
        if (!PlayerPrefs.HasKey(idInput.text))
        {
            // id가 key, pwd가 value로 저장된다.
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
