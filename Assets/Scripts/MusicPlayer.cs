using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float initVolume = 0.2f;
    public AudioClip[] clips;

    private AudioSource audioSource;
    private int playingIndex;
    private bool canPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlay && !audioSource.isPlaying)
        {
            PlayNewRandomMusic();
        }
    }
    public void StartPlayingMusic()
    {
        canPlay = true;
        StartCoroutine(MusicFadeTo(initVolume));
    }
    public void StopPlayingMusic()
    {
        canPlay = false;
        audioSource.Stop();
    }
    public void UpdateVolume(float newVolume)
    {
        newVolume = Mathf.Clamp(newVolume, 0, 1);
        StartCoroutine(MusicFadeTo(newVolume));
    }
    // 想做“被挡住”的效果，但不知道咋整
    public void SetSpatialBlend(float target)
    {
        float newBlend = Mathf.Clamp(target, 0, 1);
        audioSource.spatialBlend = newBlend;
    }

    private void PlayNewRandomMusic()
    {
        audioSource.Stop();
        if (clips.Length != 0)
        {
            playingIndex = Random.Range(0, clips.Length);
            audioSource.PlayOneShot(clips[playingIndex]);
        }
    }
    private IEnumerator MusicFadeTo(float targetVolume)
    {
        float fadeRate = 0.5f;

        while (Mathf.Abs(audioSource.volume - targetVolume) > 0.0001f)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, fadeRate * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
