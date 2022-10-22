using UnityEngine;

public class ControiImage : MonoBehaviour
{
    #region Fields

    public  AudioClip[]     audioClip;
    public  GameObject      guitar;
    public  GameObject      star;

    private AudioSource     audioSource;

    #endregion

    #region Unity Methods

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        string result = RFIDThread.Instance.UID;
        switch (result)
        {
            case var a when result.Contains("1971014946"):
                guitar.SetActive(true);
                star.SetActive(false);
                audioSource.Stop();
                audioSource.clip = audioClip[0];
                audioSource.Play();
                break;

            case var b when result.Contains("24124714657"):
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
