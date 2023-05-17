using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityNotifierTarget : MonoBehaviour
{
    // Delegate Notification
    public delegate void NotifyDelegate(GameObject source);
    public NotifyDelegate delegateHandle;
    public NotifyDelegate exitDelegateHandle;

    public void Notify(GameObject source)
    {
        if (delegateHandle != null)
        {
            delegateHandle.Invoke(source);
        }
    }

    public void NotifyExit(GameObject source)
    {
        if (exitDelegateHandle != null)
        {
            exitDelegateHandle.Invoke(source);
        }
    }
}
