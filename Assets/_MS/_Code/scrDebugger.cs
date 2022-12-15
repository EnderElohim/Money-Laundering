using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDebugger : MonoBehaviour
{
    public void ChanceMachineStrength(float _val)
    {
        scrGameData.values.machineStrength = _val;
    }

    public void ChanceMachineDuration(float _val)
    {
        scrGameData.values.machineShakeDuration = _val;
    }

    public void ChanceBrushAnimationSpeed(float _val)
    {
        scrGameData.values.brushAnimationSpeed = _val;
    }

    public void ChanceTextSpeed(float _val)
    {
        scrGameData.values.customerTextDuration = _val;
    }


}
