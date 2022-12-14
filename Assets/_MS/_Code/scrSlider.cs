using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrSlider : MonoBehaviour
{
    private Slider mySlider;
    private bool isStarted;
    private bool isFinished;
    private bool destroyOnFinish;
    private float targetValue;
    private float currentValue;
    private float speed;
    private bool isIncrease;


    private void Start()
    {
        mySlider = GetComponent<Slider>();
        currentValue = mySlider.value;

        if (currentValue > targetValue)
        {
            isIncrease = false;
        }
        else
        {
            isIncrease = true;
        }

    }

    private void Update()
    {
        if (!isStarted || isFinished) return;

        if (isIncrease)
        {
            if (currentValue < targetValue)
            {
                currentValue += Time.deltaTime * (1 + speed);
            }
            else
            {
                isFinished = true;
            }
        }
        else
        {
            if (currentValue > targetValue)
            {
                currentValue -= Time.deltaTime * (1 + speed);
            }
            else
            {
                isFinished = true;
            }
        }

        mySlider.value = currentValue;

        if(destroyOnFinish && isFinished)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(float _targetValue, float _speed, bool _destroyOnFinish)
    {
        speed = _speed;
        targetValue = _targetValue;
        destroyOnFinish = _destroyOnFinish;
        isStarted = true;
    }

    public void Refresh(float _targetValue, float _speed, bool _destroyOnFinish)
    {
        speed = _speed;
        targetValue = _targetValue;
        destroyOnFinish = _destroyOnFinish;
        isStarted = true;

        currentValue = mySlider.value;

        if (currentValue > targetValue)
        {
            isIncrease = false;
        }
        else
        {
            isIncrease = true;
        }
    }

}
