using UnityEngine;
using System.Collections;

public class WebCamManager : MonoBehaviour {
	
	private static WebCamManager instance;
    private WebCamTexture[] textures;

    public WebCamTexture[] Textures
    {
        get { return textures; }
    }
	public static WebCamManager Instance{
		get{
		    if (null == instance)
		    {
		        GameObject obj = new GameObject();
		        obj.name = "WebCamManager";
		        instance = obj.AddComponent<WebCamManager>();
                instance.textures = new WebCamTexture[WebCamTexture.devices.Length];
                for (int i = 0; i < instance.textures.Length; i++) {
                    instance.textures[i] = new WebCamTexture(WebCamTexture.devices[i].name);
                }
		    }

		    return instance;
		}		
	}	
	
}
