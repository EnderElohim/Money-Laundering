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
    public float customerTextDuration;

    [Header("Others")]
    public List<scrMoney> moneyList;
    public float zDistance;
    public float moneyToMachineMoveDuration;
    public Sprite moneySprite;
    public float moneySpriteMoveDuration;

    [Header("M. Glass")]
    public float mgScaleMultiplier;
    public float mgScaleDuration;

    [Header("Brush")]
    public float brushScaleMultiplier;
    public float brushScaleDuration;
    public Quaternion[] brushAnimationRots;
    public float brushAnimationSpeed;

    [Header("Machine")]
    public float machineScaleMultiplier;
    public float machineScaleDuration;


    [Header("Machine Shake")]
    public float machineShakeDuration;
    public float machineStrength;
    public int machineVibrato;
    public float machineRandomness;
    public int machineLoopCount;
    //float duration, float strength = 90, int vibrato = 10, float randomness = 90
}
