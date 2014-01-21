using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestureDetector : MonoBehaviour
{
    public AudioSource audio;
    BobTimedBuffer<Vector3> mPosition;
    public Transform rightHand;
    public Transform leftHand;
    public Transform leftElbow;
    public Transform rightElbow;
    public Transform leftShoulder;
    public Transform rightShoulder;

    public float interval = 1f;
    public GameObject[] listeners;
	// Use this for initialization
    public bool IsSessionOn = false;
	void Start () {
        mPosition = new BobTimedBuffer<Vector3>(interval);
	}
	
	// Update is called once per frame
    void Update()
    {
        mPosition.AddData(this.rightHand.position);
        TestSession();
        if (IsSessionOn)
        {
            NotifyListeners("UpdateGesture", mPosition);
        }
        else
        {
            mPosition.Clear();
        }
	}

    private void NotifyListeners(string method, object data)
    {
        for (int i = 0; i < listeners.Length; i++)
        {
            listeners[i].SendMessage(method, data, SendMessageOptions.DontRequireReceiver);
        }
    }
    void OnPostRender()
    {
        GLLineRenderer.DrawLine(mPosition.getData().ToArray());
    }

    public TextMesh stateMesh;
    bool TestSession()
    {
        Vector3 leftForeArm = leftHand.position - leftElbow.position;
        Vector3 rightForArm = rightHand.position - rightElbow.position;
        Vector3 leftUpperArm = leftElbow.position - leftShoulder.position;
        Vector3 rightUppderArm = rightElbow.position - rightShoulder.position;
        if (Vector3.Angle(rightForArm, rightUppderArm) > 40)
        {
            if (!IsSessionOn)
            {
                NotifyListeners("SessionStart", null);
                stateMesh.text = "SessionStart";
            }
            IsSessionOn = true;
        }
        else if (IsSessionOn)
        {
            NotifyListeners("SessionEnd", null);
            stateMesh.text = "SessionEnd";
            IsSessionOn = false;
        }

        return IsSessionOn;
    }
}
