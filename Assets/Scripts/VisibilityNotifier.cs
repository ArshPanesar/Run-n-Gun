using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityNotifier : MonoBehaviour
{
    // Who is Looking to Notify
    public GameObject sourceObject;

    // Who is the Target
    public string targetLayerName = "Default";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int targetLayer = LayerMask.NameToLayer(targetLayerName);

        if (collision.gameObject.layer == targetLayer)
        {
            // Target Found
            VisibilityNotifierTarget target = null;
            if (collision.gameObject.TryGetComponent(out target))
            {
                // Notify it
                target.Notify(sourceObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        int targetLayer = LayerMask.NameToLayer(targetLayerName);

        if (collision.gameObject.layer == targetLayer)
        {
            // Target Found
            VisibilityNotifierTarget target = null;
            if (collision.gameObject.TryGetComponent(out target))
            {
                // Notify it
                target.NotifyExit(sourceObject);
            }
        }
    }
}
