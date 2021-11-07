using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public  VideoClip[] videosClip;
    private VideoPlayer videoPlayer;
    private RFIDThread readerRFID;
    
    void Start()
    {
        readerRFID = this.GetComponent<RFIDThread>();
        videoPlayer = this.GetComponent<VideoPlayer>();
    }

    void Update()
    {
        switch (readerRFID.UID)
        {
            case var a when readerRFID.UID.Contains("13812013137"):
                videoPlayer.clip = videosClip[0];
                videoPlayer.Play();
                break;
            case var b when readerRFID.UID.Contains("169181236151"):
                videoPlayer.clip = videosClip[1];
                videoPlayer.Play();
                break;
            default:
                break;
        }
    }
}
