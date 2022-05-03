using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerDispatcher : MonoBehaviour
{
    public MonoBehaviour attackFrameReceiver;
    private IAttackFrameReceiver iAttackFrameReceiver;
    // Start is called before the first frame update
    void Start()
    {
        iAttackFrameReceiver = attackFrameReceiver as IAttackFrameReceiver;
    }

    public void AttackEvent()
    {
        iAttackFrameReceiver.AttackFrame();
    }
}
