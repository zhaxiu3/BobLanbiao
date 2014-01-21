using UnityEngine;
using System.Collections;

public class WebCam : MonoBehaviour {

    //Camera ca;
    //Transform trans;
	public int index = 0;
    public int width = 1024;
    public int height = 576;
	// Use this for initialization
    WebCamTexture mwebCamTexture;
	void Start () {	
		this.mwebCamTexture = WebCamManager.Instance.Textures[index];
        this.mwebCamTexture.requestedHeight = this.height;
        this.mwebCamTexture.requestedWidth = this.width;
		renderer.material.mainTexture = this.mwebCamTexture;
		this.mwebCamTexture.Play();
	}

    public void PlayWebCam()
    {
        this.mwebCamTexture.Play();
    }

    public void StopWebCam() {
        this.mwebCamTexture.Stop();
    }
}
