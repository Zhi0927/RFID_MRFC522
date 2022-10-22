using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    #region Fields 

    public  VideoClip[] videosClip;
    private VideoPlayer videoPlayer;

    #endregion

    #region Unity Methods

    void Start()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
    }

    void Update()
    {
        string result = RFIDThread.Instance.UID;
        switch (result)
        {
            case var a when result.Contains("1971014946"):
                videoPlayer.clip = videosClip[0];
                videoPlayer.Play();
                Debug.Log("T");
                break;
            case var b when result.Contains("24124714657"):
                videoPlayer.clip = videosClip[1];
                videoPlayer.Play();
                break;
            default:
                break;
        }
    }

    #endregion
}
