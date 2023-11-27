using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public static SFXManager GetInstance()
    {
        return instance;
    }

    public void PlayAudioClip(Transform root, AudioClip clip, bool loop = false)
    {
        if (audioSource != null)
        {
            this.transform.position = root.position;
            if (loop)
            {
                audioSource.loop = true;
                audioSource.clip= clip;
                audioSource.Play();
            }
            else
            audioSource.PlayOneShot(clip);
        }
        else
            Debug.Log("AudioSource = null");
    }

   public void Mute() {
        audioSource.mute = !audioSource.mute;
    }
}
