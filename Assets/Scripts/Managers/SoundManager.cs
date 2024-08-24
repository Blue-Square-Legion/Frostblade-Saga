using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Singleton
    public static SoundManager Instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {

        source = GetComponent<AudioSource>();
      
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
}
