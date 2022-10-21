using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControiImage : MonoBehaviour
{
    #region Fields

    public  AudioClip[]     audioClip;
    public  GameObject      guitar;
    public  GameObject      star;

    private AudioSource     audioSource;
    private RFIDThread      readerRFID;

    #endregion

    #region Unity Methods

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        readerRFID  = this.GetComponent<RFIDThread>();
    }

    void Update()
    {
        switch (readerRFID.UID)
        {
            case var a when readerRFID.UID.Contains("13812013137"):
                guitar.SetActive(true);
                star.SetActive(false);
                audioSource.Stop();
                audioSource.clip = audioClip[0];
                audioSource.Play();
                break;

            case var b when readerRFID.UID.Contains("169181236151"):
                guitar.SetActive(false);
                star.SetActive(true);
                audioSource.Stop();
                audioSource.clip = audioClip[1];
                audioSource.Play();
                break;
            default:
                break;
        }
    }

    #endregion
}
