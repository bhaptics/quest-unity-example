using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBoxToHands : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    public GameObject boxPrefab;


    void Awake()
    {
        if (boxPrefab != null)
        {
            if (leftHand != null)
            {
                Instantiate(boxPrefab, leftHand.transform);
            }

            if (rightHand != null)
            {
                Instantiate(boxPrefab, rightHand.transform);
            }
        }
    }
}
