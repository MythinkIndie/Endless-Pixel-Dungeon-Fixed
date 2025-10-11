using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager SharedInstance;

    [SerializeField] AudioSource effectSource, musicSource, extraEffectSource;

    [SerializeField] private AudioClip SFX_Cancel, SFX_Confirm, SFX_Error, SFX_Select;

    void Awake()
    {
        if (AudioManager.SharedInstance == null)
        {

            AudioManager.SharedInstance = this;

        }
        else
        {

            Destroy(this.gameObject);

        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        if (effectSource.isPlaying)
        {
            PlayExtraSound(clip);
        }
        else
        { 
            effectSource.Stop();
            effectSource.clip = clip;
            effectSource.Play();
        }
        

    }

    private void PlayExtraSound(AudioClip clip)
    {
        
            extraEffectSource.Stop();
            extraEffectSource.clip = clip;
            extraEffectSource.Play();

    }

    public void PlayMusic(AudioClip clip, bool setInLoop)
    {

        musicSource.Stop();
        musicSource.loop = setInLoop;
        musicSource.clip = clip;
        musicSource.Play();

    }

    public void StopMusic()
    { 
        musicSource.Stop();
    }

    public void SoundCancel()
    {
        SharedInstance.PlaySound(SFX_Cancel);
    }
    public void SoundSelect()
    {
        SharedInstance.PlaySound(SFX_Select);
    }
    public void SoundConfirm()
    {
        SharedInstance.PlaySound(SFX_Confirm);
    }
    public void SoundError()
    {
        SharedInstance.PlaySound(SFX_Error);
    }

}
