using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [Header("UI References")]
    public Slider progressBar;

    [Header("Scene Settings")]
    public string nextSceneName = "PlaySences";

    [Header("Speed Settings")]
    [Range(0.4f, 1f)] public float slowSpeed = 0.4f; // 0% ~ 90% 구간 속도 (낮을수록 느림)
    [Range(1f, 5f)] public float fastSpeed = 2.0f;   // 90% ~ 100% 구간 속도 (높을수록 빠름)

    private void Start()
    {
        // 1. 슬라이더 터치/클릭 방지
        if (progressBar != null)
        {
            progressBar.interactable = false;
            progressBar.value = 0f; // 0부터 시작 확실하게
        }

        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false; // 자동 이동 방지

        float currentProgress = 0f; // 실제 슬라이더에 반영할 값

        while (!op.isDone)
        {
            yield return null; // 프레임 대기

            // 로딩 진행률 (0.0 ~ 0.9)
            // op.progress가 0.9면 로딩 완료임.
            float targetProgress = op.progress;

            // 2. 속도 조절 로직

            // [구간 1] 로딩 중이거나, 로딩은 끝났지만 슬라이더가 아직 90%에 도달 못했을 때 (천천히)
            if (op.progress < 0.9f || progressBar.value < 0.9f)
            {
                // MoveTowards를 써서 일정한 속도로 증가 (Lerp보다 속도 제어가 명확함)
                progressBar.value = Mathf.MoveTowards(progressBar.value, 0.9f, slowSpeed * Time.deltaTime);
            }
            // [구간 2] 로딩도 끝났고(0.9 이상), 슬라이더도 90%를 넘겼을 때 (빠르게 피니시)
            else
            {
                // 남은 10%를 빠르게 채움
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, fastSpeed * Time.deltaTime);
            }

            // 슬라이더가 거의 꽉 찼을 때 씬 전환
            if (progressBar.value >= 0.99f)
            {
                op.allowSceneActivation = true;
            }
        }
    }
}