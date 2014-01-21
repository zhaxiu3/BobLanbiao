using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class GestureDetectedArgs : EventArgs
{
    public BobTimedBuffer<Vector3> mdata;
    public GestureDetectedArgs(BobTimedBuffer<Vector3> data)
    {
        this.mdata = data;
    }
}
public class WaveGesture : MonoBehaviour {
    public AudioSource maudio;
    public float maxX = 0;
    List<Vector3> mVelocity;
    public float threathold = 20;
    public int number = 1000;
    public event EventHandler gestureUpdateEventHandler;
	// Use this for initialization
	void Start () {

        mVelocity = new List<Vector3>();
	}

    // Update is called once per frame
    void UpdateGesture(BobTimedBuffer<Vector3> mPosition)
    {

        if (mPosition.Count > 1)
        {
            mVelocity.Add((mPosition[mPosition.Count - 1] - mPosition[mPosition.Count - 2]) / (mPosition.mtimeStamp[mPosition.Count - 1] - mPosition.mtimeStamp[mPosition.Count - 2]));
        }
        else
        {
            mVelocity.Add(Vector3.zero);
        }
        if (mVelocity.Count < 2)
            return;

        for (int i = 0; i < mVelocity.Count - 1; i++)
        {
            if (Mathf.Abs(mVelocity[i].x) < 0.2f)
            {
                mVelocity.RemoveAt(i);
                break;
            }

            if (mVelocity[i].x * mVelocity[i + 1].x < 0)
            {
                float _velocity = 0;
                for (int j = 0; j < i + 1; j++)
                {
                    _velocity += mVelocity[i].x;
                }
                mVelocity.RemoveRange(0, i + 1);
                if (Mathf.Abs(_velocity) < threathold)
                    break;
                OnWaveDetected();
                break;
            }
        }

	}

    public TextMesh numberText;
    private void OnWaveDetected()
    {
        if (!maudio.isPlaying)
            maudio.Play();
        number--;
        numberText.text = number.ToString();
    }

}
