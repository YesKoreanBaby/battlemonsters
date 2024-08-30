using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    public static SoundManager Instance { get { return instance; } }

    public AudioMixer audioMixer;
    public AudioSource bgm;
    public List<AudioClip> bgmClips;
    public List<AudioClip> effectClips;
    public int effectChannelCount = 1;

    private List<AudioSource> effects = new List<AudioSource>();
    private bool isPlaying = false;
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        for(int i = 0; i < effectChannelCount; ++i)
        {
            GameObject effect = new GameObject($"effect_{i}");
            var audioSource = effect.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("effect")[0];
            effects.Add(audioSource);

            effect.transform.SetParent(this.transform);
        }

        bgm.loop = true;
        bgm.outputAudioMixerGroup = audioMixer.FindMatchingGroups("bgm")[0];
    }
    public void PlayEffect(int code, float volum = 1f, bool isLoop = false)
    {
        if(effects.Count <= 0)
            throw new System.NotImplementedException("Not Found Chnnel");
        else
        {
            AudioClip clip = effectClips[code];
            var audioSource = effects.Find(x => x.isPlaying == false);
            if (audioSource == null)
                StartCoroutine(CreateEffectSoundRoutine(code, volum, isLoop, Time.timeScale));
            else
            {
                PlaySound(audioSource, clip, volum, false, isLoop, Time.timeScale);
            }
        }
    }
    public void StopAllEffect()
    {
        for(int i = 0; i < effects.Count; ++i)
            effects[i].Stop();
    }
    public void StopAllEffect(int code)
    {
        var effects = FindEffects(code);
        for (int i = 0; i < effects.Count; ++i)
            effects[i].Stop();
    }
    public void PlayEffectUnTimeScale(int code, float volum = 1f, bool isLoop = false)
    {
        if (effects.Count <= 0)
            throw new System.NotImplementedException("Not Found Chnnel");
        else
        {
            AudioClip clip = effectClips[code];
            var audioSource = effects.Find(x => x.isPlaying == false);
            if (audioSource == null)
                StartCoroutine(CreateEffectSoundRoutine(code, volum, isLoop, 1f));
            else
            {
                PlaySound(audioSource, clip, volum, false, isLoop);
            }
        }
    }

    public void SetBgmMixerVolume(float value)
    {
        audioMixer.SetFloat("bgm", value);
    }
    public void SetEffectMixerVolume(float value)
    {
        audioMixer.SetFloat("effect", value);
    }
    public void PlayBgm(int code, float volum, bool isLoop = true)
    {
        if (isPlaying == true)
            return;

        AudioClip clip = bgmClips[code];
        PlaySound(bgm, clip, volum, false, true);
        bgm.loop = isLoop;
    }

    public void StopBgm()
    {
        bgm.Stop();
    }

    public void StopEffect(int code)
    {
        var audioSource = FindEffect(code);
        if (audioSource != null)
            audioSource.Stop();
    }
    public AudioSource FindEffect(int code)
    {
        AudioClip clip = effectClips[code];
        var audioSource = effects.Find(x => (x != null) && (x.isPlaying == true) && (x.clip == clip));
        return audioSource;
    }
    public List<AudioSource> FindEffects(int code)
    {
        AudioClip clip = effectClips[code];
        var audioSources = effects.FindAll(x => (x.isPlaying == true) && (x.clip == clip));
        return audioSources;
    }
    public void Fade(AudioSource audioSource, float fadeTime, float startValue, float endValue)
    {
        StartCoroutine(FadeRoutine(audioSource, fadeTime, startValue, endValue));
    }
    private void PlaySound(AudioSource audioSource, AudioClip clip, float volume, bool isDelayed = false, bool isLoop = false, float pitch = 1f)
    {
        audioSource.pitch = pitch;
        audioSource.Stop();

        audioSource.volume = volume;
        audioSource.clip = clip;

        if (isDelayed == true)
            audioSource.PlayDelayed(3f);
        else
            audioSource.Play();

        audioSource.loop = isLoop;
        isPlaying = false;
    }
    private IEnumerator FadeRoutine(AudioSource audioSource, float fadeTime, float startValue, float endValue)
    {
        float lerpTime = fadeTime;
        float lerpSpeed = 1f;
        float currentTime = 0f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            audioSource.volume = Mathf.Lerp(startValue, endValue, currentSpeed);
            yield return null;
        }
    }
    private IEnumerator CreateEffectSoundRoutine(int code, float volum, bool isLoop, float pitch)
    {
        var audioSource = new GameObject("Clip").AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("effect")[0];
        audioSource.transform.SetParent(this.transform);

        AudioClip clip = effectClips[code];

        PlaySound(audioSource, clip, volum, false, isLoop, pitch);
        yield return null;
        yield return new WaitUntil(() => audioSource.isPlaying == false);

        Destroy(audioSource.gameObject);
    }
}
