using UnityEngine;
using System.Collections;

public class VariableDelayLine
{

    int N;
    float[] A;
    int rptr = 0;
    int wptr = 0;
    
    public VariableDelayLine (int N)
    {
        this.N = N;
        A = new float[N];
    }

    public void SetDelay (float P)
    {
        int M = Mathf.Clamp (Mathf.FloorToInt (P * N), 0, N-1);
        rptr = wptr - M;
        while (rptr < 0) {
            rptr += N;
        }
    }
    
    public float Tick (float x)
    {
        float y;
        A [wptr++] = x; 
        rptr++;
        while (rptr >= N) {
            rptr -= N;
        }
        try {
            y = A [rptr];
        } catch(System.IndexOutOfRangeException) {
            Debug.Log(N);
            Debug.Log(rptr);
            throw;
        }
        while ((wptr) >= N) {
            wptr -= N;
        }
        return y;
    }
}
