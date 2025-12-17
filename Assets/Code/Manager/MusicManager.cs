using UnityEngine;
using UnityEngine.SceneManagement;

// 요청하신 3가지 BGM 타입 정의
public enum MusicType
{
    Start,
    Shop,
    BossBattle,
    None
}

public class MusicManager : Singleton<MusicManager>
{
    [Header("Audio Components")]
    public AudioSource bgmAudioSource;

    [Header("Audio Clips - BGM")]
    public AudioClip startBgmClip;      // Start 씬용
    public AudioClip shopBgmClip;       // Setting 씬용
    public AudioClip bossBattleBgmClip; // Play 씬용

    private void OnEnable()
    {
        // 씬이 로드될 때마다 감지하기 위해 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // 게임 시작 시 현재 씬에 맞는 BGM 재생 (초기화)
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    // 씬이 변경될 때 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "StartSences":
                PlayBGM(MusicType.Start);
                break;

            case "SettingSenes":
                PlayBGM(MusicType.Shop); // Setting 씬에서는 Shop BGM 사용
                break;

            case "PlaySences":
                PlayBGM(MusicType.BossBattle); // Play 씬에서는 BossBattle BGM 사용
                break;
        }
    }

    public void PlayBGM(MusicType type)
    {
        AudioClip targetClip = null;

        switch (type)
        {
            case MusicType.Start:
                targetClip = startBgmClip;
                break;
            case MusicType.Shop:
                targetClip = shopBgmClip;
                break;
            case MusicType.BossBattle:
                targetClip = bossBattleBgmClip;
                break;
            case MusicType.None:
                targetClip = null;
                break;
        }

        // 이미 같은 음악이 재생 중이라면 다시 시작하지 않음
        if (bgmAudioSource.clip == targetClip && bgmAudioSource.isPlaying) return;

        bgmAudioSource.Stop();
        bgmAudioSource.clip = targetClip;

        if (targetClip != null)
        {
            bgmAudioSource.Play();
        }
    }

    // 볼륨 조절 기능은 유지
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}