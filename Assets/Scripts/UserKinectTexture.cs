using UnityEngine;
using System.Collections;

public class UserKinectTexture : MonoBehaviour {
	public KinectImgGenerator textGen;
    public Renderer UserImageRenderer;
	// Use this for initialization
	void Start () {
		if(textGen)
		{
            Debug.Log("hello");
			if(renderer)
				renderer.material.mainTexture = textGen.getTexture();
            this.UserImageRenderer.material.SetTexture("_Mask", renderer.material.mainTexture);
				renderer.material.mainTextureScale = new Vector2(0.625f,0.46875f);
		}
	}
}
