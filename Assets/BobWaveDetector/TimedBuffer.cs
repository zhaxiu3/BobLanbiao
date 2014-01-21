using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedBuffer<T> {

    public List<T> mdataBuffer;
    public List<float> mtimeStamp;
    private float minterval = 1f;

    public int Count
    {
        get
        {
            return mdataBuffer.Count;
        }
    }
    public TimedBuffer(float interval)
    {
        mdataBuffer = new List<T>();
        mtimeStamp = new List<float>();
    }

    public T this[int i]
    {
        get
        {
            return mdataBuffer[i];
        }
    }

    public List<T> getData()
    {
        return mdataBuffer;
    }
    public void AddData(T data)
    {
        mdataBuffer.Add(data);
        mtimeStamp.Add(Time.time);
        if (Time.time - mtimeStamp[0] > minterval)
        {
            mdataBuffer.RemoveAt(0);
            mtimeStamp.RemoveAt(0);
        }
    }
    public void RemoveRange(int begin, int end)
    {
        mdataBuffer.RemoveRange(begin, end);
        mtimeStamp.RemoveRange(begin, end);
    }
    public void Clear()
    {
        mdataBuffer.Clear();
        mtimeStamp.Clear();
    }
}
