using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityNotifierTarget : MonoBehaviour
{
    // Source of a Notification sent by a Visiblity Notifier
    [HideInInspector] public GameObject sourceObject = null;

    // Delegate Notification
    public delegate void NotifyDelegate();
    public NotifyDelegate delegateHandle;

    public void Notify(GameObject source)
    {
        sourceObject = source;

        delegateHandle.Invoke();
    }
}
