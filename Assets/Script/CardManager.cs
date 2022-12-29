using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    private const int Size = 3;
    private int MaxCardNumber = 78;

    [SerializeField]
    public List<GameObject> Cards = new List<GameObject>();

    [SerializeField]
    public List<GameObject> Trail0 = new List<GameObject>();

    [SerializeField]
    public List<GameObject> Trail1 = new List<GameObject>();

    [SerializeField]
    public List<GameObject> Trail2 = new List<GameObject>();

    private List<GameObject>[] Trails = new List<GameObject>[Size];
    private List<GameObject> CardOut = new List<GameObject>();

    public void JitterCard()
    {
        Quaternion baseRotation = Cards[0].transform.rotation;
        Vector3 basePosition = Cards[0].transform.position;

        for (int i = 1; i < MaxCardNumber; i++)
        {
            Vector3 newPosition = basePosition;
            newPosition.x = newPosition.x + Random.Range(-0.01f, +0.01f);
            newPosition.y = newPosition.y + Random.Range(-0.01f, +0.01f);
            newPosition.z = newPosition.z + Random.Range(-0.01f, +0f);
            Cards[i].transform.position = newPosition;
            basePosition.z = newPosition.z;

            Cards[i].transform.rotation = baseRotation;
            Cards[i].transform.Rotate(new Vector3(0, 0, 1), Random.Range(-7, 7));
        }
    }

    public void ResetCard()
    {
        for (int i = 0; i < Size; i++)
        {
            var curve = CardOut[i].GetComponent<BezairCurve>();
            curve.ResetPosition();
            var rotation = CardOut[i].GetComponent<CardRotation>();
            rotation.ResetRotation();
        }
    }

    public void SendCard()
    {
        var cardsNumberDic = new Dictionary<int, int>();
        while (cardsNumberDic.Count < Size)
        {
            int newCardNumber = Random.Range(0, MaxCardNumber);
            if (!cardsNumberDic.ContainsKey(newCardNumber))
            {
                cardsNumberDic.Add(newCardNumber, 0);
            }
        }

        var cardsNumber = cardsNumberDic.Keys.ToList();
        CardOut.Clear();
        for (int i = 0; i < Size; i++)
        {
            GameObject card = Cards[cardsNumber[i]];
            BezairCurve curve = card.GetComponent<BezairCurve>();
            curve.ControllerPoints.Clear();

            for (int j = 0; j < Trails[i].Count; j++)
            {
                curve.ControllerPoints.Add(Trails[i][j]);
            }
            CardOut.Add(card);
        }

        BeginSending();
    }

    private void Start()
    {
        Trails[0] = Trail0;
        Trails[1] = Trail1;
        Trails[2] = Trail2;
        MaxCardNumber = Cards.Count;
        for (int i = 0; i < Size; i++)
        {
            CardOut.Add(Cards[i]);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !IsStart)
        {
            ResetCard();
            SendCard();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCard();
        }

        if (IsStart)
        {
            CurTime += Time.deltaTime;

            if (CurTime > TotalTime)
            {
                CurTime = 0;

                var curve = CardOut[CurSendingCard].GetComponent<BezairCurve>();
                curve.Begin();
                var rotation = CardOut[CurSendingCard].GetComponent<CardRotation>();
                rotation.Begin();

                CurSendingCard++;
                if (CurSendingCard >= Size)
                {
                    IsStart = false;
                }
            }
        }
    }

    public float TotalTime = 1f;
    public float CurTime = 0f;
    public bool IsStart = false;
    private int CurSendingCard = 0;

    public void BeginSending()
    {
        if (!IsStart)
        {
            IsStart = true;
            CurTime = 0;
            CurSendingCard = 0;
        }
    }
}
