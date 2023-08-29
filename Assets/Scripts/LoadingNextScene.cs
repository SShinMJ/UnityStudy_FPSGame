using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� : ���� ���� �񵿱� ������� �ε��Ѵ�.
// ����2: ���� ���� �ε� ������� �����̴��� ǥ��.
public class LoadingNextScene : MonoBehaviour
{
    // ���� ������ �� ��ȣ
    public int sceneNumber = 2;

    // �ε� �����̴�, �ε� �ؽ�Ʈ
    public Slider loadingBar;
    public TMP_Text loadingText;

    int txtCnt = 0;

    void Start()
    {
        loadingText.text = "Loading...";
        StartCoroutine(AsyncNextScene(sceneNumber));
    }

    void Update()
    {
        StartCoroutine(loadingTextCH());
    }

    IEnumerator loadingTextCH()
    {
        yield return new WaitForSeconds(0.5f);

        if (txtCnt > 3)
            txtCnt = 0;
        else
            txtCnt++;

        string text = "Loading";
        for(int i = 0; i < txtCnt; i++)
        {
            text += ".";
        }

        loadingText.text = text;
    }

    // �񵿱�� ���� �� �ε�
    IEnumerator AsyncNextScene(int num)
    {
        // ����� ���� �񵿱� ������� �����.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(num);

        // allowSceneActivation : ���� ȭ�鿡 ���̴� ����
        asyncOperation.allowSceneActivation = false;

        // ���� ���� �ε� ����� ǥ��
        while(!asyncOperation.isDone)
        {
            // asyncOperation.progress : �ش� �� �ε� ������� ���� �� �� �ִ�.
            loadingBar.value = asyncOperation.progress;

            // Ư�� ������� �� ���� ���� �����ش�.
            if(asyncOperation.progress >= 0.9f)
            {
                // 90�� �̻� �ε�Ǹ� ���� ���̰� �Ѵ�.
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
