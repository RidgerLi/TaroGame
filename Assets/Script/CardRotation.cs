using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotation : MonoBehaviour
{
    public float TotalTime = 1f;
    public float CurTime = 0f;
    public Vector3 TargetRotation = Vector3.zero;
    private Quaternion StartRotationQ;
    private Quaternion TargetRotationQ;

    void Update()
    {
        if (isStart)
        {
            CurTime += Time.deltaTime;
            float t = CurTime / TotalTime;
            // transform.Rotate(new Vector3(1, 0, 0), 90 / TotalTime * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.Lerp(StartRotationQ, TargetRotationQ, t);

            if (CurTime > TotalTime)
            {
                isStart = false;
            }
        }
    }

    public bool isStart = false;

    public void Begin()
    {
        if (!isStart)
        {
            isStart = true;
            CurTime = 0;
            StartRotationQ = transform.rotation;
            TargetRotationQ = Quaternion.Euler(TargetRotation);
        }
    }

    public void ResetRotation()
    {
        transform.rotation = StartRotationQ;
    }
}
