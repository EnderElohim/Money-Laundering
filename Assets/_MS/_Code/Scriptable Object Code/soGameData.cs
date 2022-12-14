using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Game Data", menuName = "Moonlight Studio/Game Data")]
public class soGameData : ScriptableObject
{
    [Header("Camera")]
    public float cameraToCustomerDuration;
    public float cameraToWorkDuration;
    public float cameraWorkFov;
    public float cameraCustomerFov;
    [Header("Customer")]
    public float customerMoveDuration;
    public string[] customerRequestLines;
    public Vector2 customerMoneyDemandRange;
    [Header("Others")]
    public List<scrMoney> moneyList;
    public float zDistance;

    [Header("M. Glass")]
    public float mgScaleMultiplier;
    public float mgScaleDuration;
    [Space()]
    public float brushScaleMultiplier;
    public float brushScaleDuration;
    public Quaternion[] brushAnimationRots;
    public float brushAnimationSpeed;
    [Space()]
    public float machineScaleMultiplier;
    public float machineScaleDuration;
}
