using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class BezairCurve : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> ControllerPoints = new List<GameObject>();

    public float TotalTime = 1f;

    [SerializeField]
    private float CurTime;

    private Vector3 StartPosition;

    void Update()
    {
        if (isStart)
        {
            CurTime += Time.deltaTime;
            float t = CurTime / TotalTime;
            transform.position = Move(t);
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
        }
    }

    private Vector3 Move(float t)
    {
        var queue = new Queue<Vector3>();
        foreach (var item in ControllerPoints)
        {
            queue.Enqueue(item.transform.position);
        }

        while (queue.Count > 1)
        {
            int queueLength = queue.Count;
            Vector3 value1 = queue.Dequeue();
            Vector3 value2 = queue.Dequeue();
            for (int i = 0; i < queueLength - 1; i++)
            {
                Vector3 value3 = t * value1 + (1 - t) * value2;
                queue.Enqueue(value3);
                if (i < queueLength - 2)
                {
                    value1 = value2;
                    value2 = queue.Dequeue();
                }
            }
        }

        Vector3 rtnValue = queue.Dequeue();
        // Debug.Log(rtnValue);
        return rtnValue;
    }

    public void ResetPosition()
    {
        transform.position = StartPosition;
    }

    private void Start()
    {
        StartPosition = transform.position;
    }


}
