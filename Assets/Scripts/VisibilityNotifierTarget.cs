using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityNotifierTarget : MonoBehaviour
{
    // Delegate Notification
    public delegate void NotifyDelegate(GameObject source);
    public NotifyDelegate delegateHandle;

    public void Notify(GameObject source)
    {
        delegateHandle.Invoke(source);
    }
}
