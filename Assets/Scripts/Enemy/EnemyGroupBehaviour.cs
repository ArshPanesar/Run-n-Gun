using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupBehaviour : MonoBehaviour
{
    private VisibilityNotifierTarget target;

    public void SubscribeToNotification(VisibilityNotifierTarget.NotifyDelegate enemyDelegate)
    {
        if (target == null)
        {
            target = GetComponent<VisibilityNotifierTarget>();
        }

        target.delegateHandle += enemyDelegate;
    }
}
