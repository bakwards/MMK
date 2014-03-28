using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    #region Singleton Pattern 
    private static AudioController _instance = null;
    public static AudioController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load("Sound/AudioController", typeof(AudioController))) as AudioController;
                _instance.Init();
                return _instance;
            }
            else if (_instance != null)
            {
                return _instance;
            }
            return null;
        }
    }

    
    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    public AudioClip pickUpCoin;
    public AudioSource mainAudioSource;
	private bool important;

    void Init()
    {
    }

    public void PlaySound()
    {
		mainAudioSource.loop = false;
        mainAudioSource.clip = pickUpCoin;
        mainAudioSource.Play();        
    }
	
	public void PlayClip(AudioClip clip, bool impo = false){
		if(!important){
			mainAudioSource.loop = false;
			mainAudioSource.clip = clip;
			mainAudioSource.Play();   
		}
		if(impo){
			important = true;
		}
	}

	public void LoopClip (AudioClip clip)
	{
		mainAudioSource.loop = true;
		mainAudioSource.clip = clip;
		mainAudioSource.Play();   
	}

	void Update(){
		if(important && !mainAudioSource.isPlaying){
			important = false;
		}
	}
}
