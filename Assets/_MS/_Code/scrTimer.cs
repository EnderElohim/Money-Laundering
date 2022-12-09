using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrTimer : MonoBehaviour
{
    private TextMeshProUGUI myText;
    private bool isIncrease;
    private float limit;
    private float startingValue;
    private float currentValue;
    private bool isStarted;
    private bool isFinished;
    private bool destroyOnFinish;
    

    private void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!isStarted || isFinished) return;

        if (isIncrease)
        {
            if(currentValue < limit)
            {
                currentValue += Time.deltaTime;
            }
            else
            {
                isFinished = true;
               
            }
        }
        else
        {
            if (currentValue > limit)
            {
                currentValue -= Time.deltaTime;
            }
            else
            {
                isFinished = true;
            }
        }
        myText.text = ((int)currentValue).ToString();

        if(isFinished && destroyOnFinish)
        {
            Destroy(gameObject);
        }
    }
    public void Setup(bool _isIncrease, float _limit, float _startingValue, bool _destroy)
    {
        isIncrease = _isIncrease;
        limit = _limit;
        startingValue = _startingValue;
        currentValue = startingValue;
        isStarted = true;
        destroyOnFinish = _destroy;
    }


}
