using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BhapticsCollisionForUI : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        var button = other.gameObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.Log("cannot find button");
            return;
        }
        
        button.OnSubmit(null);
        
    }

    void OnCollisionEnter(Collision collision)
    {
        var button = collision.gameObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.Log("cannot find button");
            return;
        }

        button.OnSubmit(null);
    }

}
