using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugCubePosition : MonoBehaviour {
    public AudioSource audio;
    TimedBuffer<Vector3> mPosition;
    List<Vector3> mVelocity;
    public GameObject Cube;
    public float interval = 1f;
    public int number = 100;
    public float threathold = 20;
	// Use this for initialization
	void Start () {
        mPosition = new TimedBuffer<Vector3>(interval);
        mVelocity = new List<Vector3>();
	}
	
	// Update is called once per frame
    void Update()
    {
        mPosition.AddData(this.Cube.transform.position);
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

        for (int i = 0; i < mVelocity.Count-1; i++)
        {
            if (mVelocity[i].x == 0)
            {
                mVelocity.RemoveAt(i);
                break;
            }

            if (mVelocity[i].x * mVelocity[i + 1].x < 0)
            {
                float _velocity = 0;
                for (int j = 0; j < i+1; j++)
                {
                    _velocity += mVelocity[i].x;
                }
                mVelocity.RemoveRange(0, i+1);
                Debug.Log(_velocity);
                if (Mathf.Abs(_velocity) < threathold)
                    break;
                if(!audio.isPlaying)
                audio.Play();
                number--;
                GameObject.FindObjectOfType<TextMesh>().text = number.ToString();
                break;
            }
        }
	}

    void OnPostRender()
    {
        GLLineRenderer.DrawLine(mPosition.getData().ToArray());
    }
}
