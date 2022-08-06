using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip [] sounds;
    private AudioSource source;
    void Awake()
    {
        this.source = GetComponent<AudioSource>();
    }

   public void Play(int index, bool loop = false){
        if(source.isPlaying){
            Stop();
        }
        source.clip = sounds[index];
        source.loop = loop;
        source.Play();
   }
   public void Pause(){
    source.Pause();
   }
   public void Resume(){
    source.UnPause();
   }
    public void Stop(){
        source.Stop();
    }
    public void ChangeVolume(float vol = .75f){
        source.volume = vol;
    }
    public void Mute(bool mute){
        source.mute = mute;
    }
}
