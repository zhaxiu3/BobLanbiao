using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TextureManipulate : MonoBehaviour {
    public Vector2 scalor;
    public Vector2 offset;
    public Renderer target;
    public string TextureName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null) {
            return;
        }

        if (target.sharedMaterial.GetTexture(TextureName) == null) {
            return;
        }

        target.sharedMaterial.SetTextureOffset(TextureName, offset);
        target.sharedMaterial.SetTextureScale(TextureName, scalor);
	
	}
}
