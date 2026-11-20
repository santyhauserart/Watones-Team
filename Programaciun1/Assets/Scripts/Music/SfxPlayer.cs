using UnityEngine;

public static class SfxPlayer
{
    private static AudioSource source;

    public static void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (source == null)
        {
            GameObject sfxObject = new GameObject("SfxPlayer");
            UnityEngine.Object.DontDestroyOnLoad(sfxObject);
            source = sfxObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }
        source.PlayOneShot(clip, volume);
    }
}