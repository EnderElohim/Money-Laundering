using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class scrGameManager : MonoBehaviour
{
    #region Singleton
    public static scrGameManager manager;
    private void Awake()
    {
        manager = this;
    }
    #endregion

    [Header("Assignment")]
    public EventStruct[] EventList;
    public Transform cameraWorkPosition;


    //Private
    private TriggerCondition currentCondition = TriggerCondition.onEnd;
    private float currentGameTime = 0;
    private Transform cameraCustomerPosition;


    private void Start()
    {
        cameraCustomerPosition = new GameObject().transform;
        cameraCustomerPosition.position = Camera.main.transform.position;
        cameraCustomerPosition.rotation = Camera.main.transform.rotation;

    }

    private void Update()
    {
        currentGameTime += Time.deltaTime;
    }

    [ContextMenu("MoveCameraToCustomer")]
    public void MoveCameraToCustomer()
    {
        Camera.main.transform.DOMove(cameraCustomerPosition.position, scrGameData.values.cameraToCustomerDuration);
        Camera.main.DOFieldOfView(scrGameData.values.cameraCustomerFov, scrGameData.values.cameraToCustomerDuration);
        Camera.main.transform.DORotate(cameraCustomerPosition.rotation.eulerAngles, scrGameData.values.cameraToCustomerDuration);
    }
    [ContextMenu("MoveCameraToWork")]
    public void MoveCameraToWork()
    {
        Camera.main.transform.DOMove(cameraWorkPosition.position, scrGameData.values.cameraToCustomerDuration);
        Camera.main.transform.DORotate(cameraWorkPosition.rotation.eulerAngles, scrGameData.values.cameraToCustomerDuration);
        Camera.main.DOFieldOfView(scrGameData.values.cameraWorkFov, scrGameData.values.cameraToCustomerDuration);
       

    }

    private string GetColouredString(string _val, Color _color)
    {
        return string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(_color.r * 255f), (byte)(_color.g * 255f), (byte)(_color.b * 255f), _val);
    }


    public void Win()
    {
        currentCondition = TriggerCondition.onWin;
        TriggerEvents();

    }

    public void Lose()
    {
        currentCondition = TriggerCondition.onLose;
        TriggerEvents();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void TriggerEvents()
    {
        foreach (EventStruct item in EventList)
        {
            if (item.triggerCondition == TriggerCondition.onEnd || item.triggerCondition == currentCondition)
            {
                switch (item.triggerEvent)
                {
                    case TriggerEvent.Enable:
                        foreach (GameObject currentItem in item.subjects)
                        {
                            currentItem.SetActive(true);
                        }
                        break;
                    case TriggerEvent.Disable:
                        foreach (GameObject currentItem in item.subjects)
                        {
                            currentItem.SetActive(false);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void EnableDisableObject(GameObject _subject, bool _val)
    {
        _subject.SetActive(_val);
    }

    private void AnimationTrigger(Animator _anim, string _valName)
    {
        _anim.SetTrigger(_valName);
    }

    private void AnimationSetInt(Animator _anim, string _valName, int _val)
    {
        _anim.SetInteger(_valName, _val);
    }

    private void AnimationSetFloat(Animator _anim, string _valName, float _val)
    {
        _anim.SetFloat(_valName, _val);
    }

    private void AnimationSetBool(Animator _anim, string _valName, bool _val)
    {
        _anim.SetBool(_valName, _val);
    }

    private void ChangeString(TextMeshProUGUI _text, string _val, float _actionSpeed)
    {

        if (_text == null)
        {
            Debug.LogError("ChangeString not have subject. MSG: " + _val);
            return;
        }
        //_text.text = _val;
        StartCoroutine(ChangeStringEnumarator(_text, _val, _actionSpeed));
    }
    private IEnumerator ChangeStringEnumarator(TextMeshProUGUI _text, string _val, float _actionSpeed)
    {
        float calculatedTime = _actionSpeed / (float)_val.Length;
        for (int i = 0; i <= _val.Length; i++)
        {
            _text.text = _val.Substring(0, i);

            yield return new WaitForSeconds(calculatedTime);
        }
    }


    private void MoveToPosition(GameObject _subject, Vector3 _destination)
    {
        _subject.transform.position = _destination;
    }

    private void ChangeRotation(GameObject _subject, Quaternion _rotation)
    {
        _subject.transform.rotation = _rotation;
    }
}

[System.Serializable]
public enum TriggerCondition
{
    onWin = 0,
    onLose = 1,
    onEnd = 2
}

public enum TriggerEvent
{
    Enable = 0,
    Disable = 1
}

[System.Serializable]
public struct EventStruct
{
    public string description;
    public GameObject[] subjects;
    public TriggerCondition triggerCondition;
    public TriggerEvent triggerEvent;
}