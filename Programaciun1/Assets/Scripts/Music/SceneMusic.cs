using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] private bool resumeWhereItLeft = false;
    [SerializeField] private float fadeSeconds = 0.5f;

    private void Start()
    {
        MusicPlayer.Play(music, volume, resumeWhereItLeft);
    }
}
