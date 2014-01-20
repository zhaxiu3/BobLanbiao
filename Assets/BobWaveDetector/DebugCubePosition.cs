using UnityEngine;
using System.Collections;

public class DebugCubePosition : MonoBehaviour {
    TimedBuffer<Vector3> mPosition;
    TimedBuffer<Vector3> mVelocity;

    public float interval = 1f;
	// Use this for initialization
	void Start () {
        mPosition = new TimedBuffer<Vector3>(interval);
        mVelocity = new TimedBuffer<Vector3>(interval);
	}
	
	// Update is called once per frame
    void Update()
    {
        mPosition.AddData(this.transform.position);
        if (mPosition.Count > 1)
        {
            mVelocity.AddData(mPosition[mPosition.Count-1] - mPosition[mPosition.Count - 2]);
        }
        else
        {
            mVelocity.AddData(Vector3.zero);
        }
        bool changed = false;
        for (int i = 1; i < mVelocity.Count; i++)
        {
            if (mVelocity[i].x * mVelocity[i - 1].x < 0)
            {
                for (int j = 0; j < i; j++)
                {

                }
            }
        }
	}
}
