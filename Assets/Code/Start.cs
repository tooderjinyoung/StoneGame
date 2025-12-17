using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public AudioSource clickAudio; // 유니티 에디터에서 여기에 Audio Source를 드래그해서 넣으세요.
    string scene = "SettingSenes";

    // 버튼에 연결할 함수 1: 씬 전환
    public void StartSence()
    {
        // 코루틴 시작: 소리를 재생하고 기다렸다가 씬을 넘김
        StartCoroutine(PlaySoundAndLoad());
    }

    // 버튼에 연결할 함수 2: 종료
    public void exit()
    {
        // 코루틴 시작: 소리를 재생하고 기다렸다가 종료
        StartCoroutine(PlaySoundAndQuit());
    }

    IEnumerator PlaySoundAndLoad()
    {
        // 1. 소리가 연결되어 있다면 재생
        if (clickAudio != null)
        {
            clickAudio.Play();
            // 2. 소리 길이만큼 대기 (소리가 다 끝날 때까지 멈춤)
            yield return new WaitForSeconds(clickAudio.clip.length);
        }

        // 3. 대기가 끝나면 씬 전환
        SceneManager.LoadScene(scene);
    }

    IEnumerator PlaySoundAndQuit()
    {
        if (clickAudio != null)
        {
            clickAudio.Play();
            yield return new WaitForSeconds(clickAudio.clip.length);
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
