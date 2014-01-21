using UnityEngine;
using System.Collections;

public class KinectImgGenerator : MonoBehaviour {
	public		Transform UserTrans=null;
    public bool EnableAlphaBlur;
    public int HorizonalOffset = 20;
    public int erosion = 4;
    public int blurLen = 10;
	Texture2D 	mTexture;
	int			mImgWidth;
	int			mImgHeight;
	float		mLabelRatioW;
	float		mLabelRatioH;
	bool		canWork;
	Color32[] 	buf;
	public Texture2D getTexture()
	{
		return mTexture;
	}
	// Use this for initialization
	void Start () {
		mTexture = new Texture2D(1024,1024, TextureFormat.RGBA32, false);
		buf = new Color32[1024*1024];
		if(ZigInput.Settings.UpdateImage&&ZigInput.Settings.UpdateLabelMap)
		{
			for(int i=0; i<buf.Length; i++)
			{
				buf[i].r=0;
				buf[i].g=0;
				buf[i].b=0;
				buf[i].a=0;
			}
			canWork=true;
		}
        ZigInput.Instance.AddListener(gameObject);
	} 
	// Update is called once per frame
	void Update () {
		if(UserTrans)
		{
			float depth=0f;
			foreach(ZigTrackedUser currentUser in ZigInput.Instance.TrackedUsers.Values)
			{
				depth -= currentUser.Position.z/1000.0f;
			}
			if(0!=ZigInput.Instance.TrackedUsers.Count)
				depth /= ZigInput.Instance.TrackedUsers.Count;
			UserTrans.localPosition = new Vector3(UserTrans.localPosition.x,UserTrans.localPosition.y,depth);
		}
	}
	void AlphaBlur(int cols, int rows)
	{	
		for(int row=0 ; row < rows && row<1024; row++)
		{
			int rowOffset = row*1024;
			for(int col=0; col < cols && col<1024; col++)
			{
				if(row>=1 && col>1)
				{
					uint agg = buf[1024*(row-1)+col].a;
					agg += buf[1024*(row+1)+col].a;
					agg += buf[1024*row+col+1].a;
					agg += buf[1024*row+col-1].a;
					buf[rowOffset+col].a =(byte)(agg/4);
				}
			}
		}
	}
	void Zig_Update(ZigInput input)
    {
		if(canWork)
		{
			mImgWidth = ZigInput.Image.xres;
			mImgHeight = ZigInput.Image.yres;
			int lWidth=ZigInput.LabelMap.xres;
            int lHeight = ZigInput.LabelMap.yres;
			mLabelRatioW = (float) lWidth/ (float )mImgWidth;
			mLabelRatioH = (float) lHeight/ (float )mImgHeight;
			
			Color32[] 	rawImageMap = ZigInput.Image.data;
			short[]	 	rawLabelMap = ZigInput.LabelMap.data;
			for(int row=0 ; row < mImgHeight && row<1024; row++)
			{
				int rowOffset = row*1024;
				int imgROffset = row*mImgWidth;
				for(int col=0; col < mImgWidth && col<1024; col++)
				{
					buf[rowOffset+col] = rawImageMap[imgROffset+col];
                    int labelIdx = (int)(row * mLabelRatioH) * lWidth + (int)(col * mLabelRatioW);
                    buf[rowOffset + col].a = (byte)(0 == rawLabelMap[labelIdx] ? 0 : 255);
                }
            }
            bool indetail = false;
            byte[] temp1 = new byte[1024];
            byte[] temp2 = new byte[1024];
            for (int row = 0; row < mImgHeight && row < 1024; ++row )
            {
                
                int rowOffset = row * 1024;
                int imgROffset = row * mImgWidth;

               //¿ØÖÆÆ«ÒÆ
                for (int col = 0; col < mImgWidth && col < 1024; col++)
                {
                    if (col - HorizonalOffset > 0)
                        temp1[col] = buf[rowOffset + col - HorizonalOffset].a;
                    else
                        temp1[col] = buf[rowOffset + col - HorizonalOffset + 1024].a;
                }

                //¸¯Ê´±ß½ç
                for (int col = 0; col < mImgWidth && col < 1024; col++)
                {
                    if (temp1[col] == 255 && indetail == false) {
                        indetail = true;
                        for (int i = 0; i < erosion; i++ )
                        {
                            if(col+1<1023)
                                temp1[col++] = 0;
                        }
                    }
                    else if (temp1[col] == 0 && indetail ==true)
                    {
                        indetail = false;
                        for (int i = 0; i < erosion; i++)
                        {
                            if(col-i >0)
                                temp1[col - i] = 0;
                        }
                        
                    }
                }
                indetail = false;

                for (int col = 0; col < mImgWidth && col < 1024; col++)
                {
                    if (temp1[col] == 255 && indetail == false)
                    {
                        indetail = true;
                        for (int i = 0; i < blurLen; i++)
                        {
                            if (col -i>0)
                                temp1[col-i] = (byte)(255*(1.0f-(float)i/(float)blurLen));
                        }
                    }
                    else if (temp1[col] == 0 && indetail == true)
                    {
                        indetail = false;
                        for (int i = 0; i < blurLen; i++)
                        {
                            if (col +1  < 1024)
                                temp1[col++] = (byte)(255 * (1-(float)i / (float)blurLen));
                        }

                    }
                }

                for (int col = 0; col < mImgWidth && col < 1024; col++)
                {
                    buf[rowOffset + col].a = temp1[col];// (temp1[col] == (byte)255 && temp2[col] == (byte)255) ? (byte)255 : (byte)0;
                }
            }
            //Ä£ºý±ßÔµ


            //if (EnableAlphaBlur)
            //{
            //    AlphaBlur(mImgWidth, mImgHeight);
            //    AlphaBlur(mImgWidth, mImgHeight);
            //}
			mTexture.SetPixels32(buf,0);
			mTexture.Apply(false);
		}
    }
}
