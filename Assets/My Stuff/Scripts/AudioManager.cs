using UnityEngine.Audio;
using System;
using UnityEngine;


//to call it : FindObjectOfType<AudioManager>().Play("name");

public class AudioManager : MonoBehaviour
{
	
	public Sound[] sounds;
	
	public static AudioManager instance;
    
    void Awake()
    {
		if (instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		
        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			
		}
    }
	
	void Start()
	{
		
	}
	
    public void Play (string name)
	{
		Sound s = Array.Find(sounds,sound => sound.name == name);
		if(s == null)
		{
			return;
		}
		s.source.Play();
	}
	
	public void Pause(string name)
    {
		Sound s = Array.Find(sounds,sound => sound.name == name);
		if(s == null)
		{
			return;
		}
            s.source.Pause();
    }
 
    public void UnPause(string name)
    {
		Sound s = Array.Find(sounds,sound => sound.name == name);
		if(s == null)
		{
			return;
		}
            s.source.UnPause();
    }
	
	
	public void StopPlaying (string sound)
	{
	Sound s = Array.Find(sounds, item => item.name == sound);
	if (s == null)
	{
		return;
	}
	// s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
	// s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
	s.source.Stop ();
	}
	
}
