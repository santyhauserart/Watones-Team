using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;
    private AudioSource source;
    private Dictionary<AudioClip, float> savedTimes = new Dictionary<AudioClip, float>();
    private Coroutine fadeRoutine;
    private float targetVolume = 0.5f;

    public static void Play(AudioClip clip, float volume = 0.5f, bool resume = false, float fadeSeconds = 0.5f)
    {
        if (clip == null) return;
        CreateIfMissing();
        instance.Change(clip, volume, resume, fadeSeconds);
    }

    public static void FadeOut(float seconds)
    {
        if (instance == null) return;
        if (instance.fadeRoutine != null) instance.StopCoroutine(instance.fadeRoutine);
        instance.fadeRoutine = instance.StartCoroutine(instance.FadeOutRoutine(seconds));
    }

    private static void CreateIfMissing()
    {
        if (instance != null) return;
        GameObject musicObject = new GameObject("MusicPlayer");
        instance = musicObject.AddComponent<MusicPlayer>();
        DontDestroyOnLoad(musicObject);
        instance.source = musicObject.AddComponent<AudioSource>();
        instance.source.loop = true;
        instance.source.playOnAwake = false;
    }

    private void Change(AudioClip clip, float volume, bool resume, float fadeSeconds)
    {
        targetVolume = volume;
        if (source.clip == clip && source.isPlaying && fadeRoutine == null)
        {
            source.volume = volume;
            return;
        }
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(ChangeRoutine(clip, resume, fadeSeconds));
    }

    private IEnumerator ChangeRoutine(AudioClip clip, bool resume, float fadeSeconds)
    {
        if (source.clip != clip || !source.isPlaying)
        {
            if (source.isPlaying)
            {
                yield return Fade(source.volume, 0f, fadeSeconds);
                savedTimes[source.clip] = source.time;
            }
            source.clip = clip;
            source.volume = 0f;
            if (resume && savedTimes.ContainsKey(clip))
            {
                source.time = savedTimes[clip];
            }
            source.Play();
        }
        yield return Fade(source.volume, targetVolume, fadeSeconds);
        fadeRoutine = null;
    }

    private IEnumerator FadeOutRoutine(float seconds)
    {
        if (source.isPlaying)
        {
            yield return Fade(source.volume, 0f, seconds);
            savedTimes[source.clip] = source.time;
            source.Stop();
        }
        fadeRoutine = null;
    }

    private IEnumerator Fade(float from, float to, float seconds)
    {
        if (seconds <= 0f)
        {
            source.volume = to;
            yield break;
        }
        float t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(from, to, t / seconds);
            yield return null;
        }
        source.volume = to;
    }
}