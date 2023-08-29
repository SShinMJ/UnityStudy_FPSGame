using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 목적 : 다음 씬을 비동기 방식으로 로드한다.
// 목적2: 현재 씬에 로딩 진행률을 슬라이더로 표현.
public class LoadingNextScene : MonoBehaviour
{
    // 다음 진행할 씬 번호
    public int sceneNumber = 2;

    // 로딩 슬라이더, 로딩 텍스트
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

    // 비동기로 다음 씬 로드
    IEnumerator AsyncNextScene(int num)
    {
        // 저장된 씬을 비동기 방식으로 만든다.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(num);

        // allowSceneActivation : 씬이 화면에 보이는 여부
        asyncOperation.allowSceneActivation = false;

        // 현재 씬에 로딩 진행률 표시
        while(!asyncOperation.isDone)
        {
            // asyncOperation.progress : 해당 씬 로드 진행률을 가져 올 수 있다.
            loadingBar.value = asyncOperation.progress;

            // 특정 진행률일 때 나올 씬을 보여준다.
            if(asyncOperation.progress >= 0.9f)
            {
                // 90퍼 이상 로드되면 씬을 보이게 한다.
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
