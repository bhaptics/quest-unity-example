using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BhapticsOculusPointing : MonoBehaviour {

    public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;

    public Transform shootPoint;
    public Material laserMaterial;

    private LineRenderer laser;
    private bool hitSomething;
    public LayerMask rayLayerMask;

    private bool isTriggerDown;

    void Start()
    {
        laser = gameObject.AddComponent<LineRenderer>();
        laser.startWidth = laser.endWidth = 0.015f;
        laser.SetPositions(new Vector3[] { transform.position, transform.position });
        laser.material = laserMaterial;
        laser.enabled = true;
    }

    

    void Update()
    {
        CheckForTrigger();
        DrawLine();
    }

    private void CheckForTrigger()
    {
        if (OVRInput.GetDown(button))
        {
            isTriggerDown = true;
        } else if (OVRInput.GetUp(button))
        {
            isTriggerDown = false;
        }

        if (isTriggerDown)
        {
            Debug.Log("Down " + isTriggerDown);
        }

        RaycastHit raycastHit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out raycastHit, 30f, rayLayerMask))
        {
            var destination = raycastHit.point;
            
            var uiButton = raycastHit.collider.GetComponent<Button>();
            
            if (uiButton != null)
            {
                Debug.Log("CheckForTrigger hit: " + isTriggerDown);
//                if (isTriggerDown)
//                {
                    uiButton.OnSubmit(null);
//                }
            }
        }

    }

    private void DrawLine()
    {
        if (shootPoint == null)
        {
            laser.enabled = false;
            return;
        }
//
        RaycastHit raycastHit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out raycastHit, 10f, rayLayerMask))
        {
            var destination = raycastHit.point;
//            Debug.Log("DrawLine hit" + destination);
            laser.material.color = Color.green;
        }
        else
        {
            laser.material.color = Color.red;
        }



        laser.enabled = true;
        
        laser.SetPosition(0, shootPoint.position);
        laser.SetPosition(1, shootPoint.position + shootPoint.forward * 10f);
    }
}
