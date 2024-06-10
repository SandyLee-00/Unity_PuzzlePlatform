using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사운드 매니저
/// Bgm, Effect 사운드를 재생한다.
/// </summary>  
public class SoundManager : Singleton<SoundManager>
{
    private AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.Max];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (_componentInstance == null)
        {
            _componentInstance = SoundManager.Instance;
        }

        // BGM, Effect 등등 사운드 타입을 만들어준다.
        string[] soundTypeNames = System.Enum.GetNames(typeof(Define.Sound));
        for (int count = 0; count < soundTypeNames.Length - 1; count++)
        {
            GameObject go = new GameObject { name = soundTypeNames[count] };
            _audioSources[count] = go.AddComponent<AudioSource>();
            go.transform.parent = _componentInstance.transform;
        }
        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    /// <summary>
    /// 기존 재생하던 음악 모두 멈추고 캐싱해둔 오디오 클립을 모두 삭제
    /// </summary>
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    /// <summary>
    /// 사운드 채널에 맞게 볼륨 설정
    /// </summary>
    /// <param name="type"></param>
    /// <param name="volume"></param>
    public void SetVolume(Define.Sound type, float volume)
    {
        _audioSources[(int)type].volume = volume;
    }

    public float GetVolume(Define.Sound type)
    {
        return _audioSources[(int)type].volume;
    }

    /// <summary>
    /// 사운드 채널에 맞게 사운드 재생 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="path"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public bool Play(Define.Sound type, string path, float pitch = 1.0f)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("SoundManager::Play() path is null or empty.");
            return false;
        }

        if (path.Contains("Sound/") == false)
        {
            path = string.Format("Sound/{0}", path);
        }

        AudioSource audioSource = _audioSources[(int)type];
        audioSource.pitch = pitch;

        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.LogError($"SoundManager::Play() AudioClip is null. path: {path}");
                return false;
            }

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = audioClip;
            audioSource.Play();
            return true;
        }
        else if (type == Define.Sound.Effect)
        {
            AudioClip audioClip = GetAudioClip(path);
            if (audioClip == null)
            {
                Debug.LogError($"SoundManager::Play() AudioClip is null. path: {path}");
                return false;
            }

            audioSource.PlayOneShot(audioClip);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 오디오 클립을 Dictionary에 캐싱하고 반환
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (_audioClips.TryGetValue(path, out audioClip))
        {
            return audioClip;
        }

        audioClip = Resources.Load<AudioClip>(path);
        _audioClips.Add(path, audioClip);
        return audioClip;
    }
}