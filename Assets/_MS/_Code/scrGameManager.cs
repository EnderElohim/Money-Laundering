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
    public Transform moneySpawnPosition;
    public Transform customerEnterPosition;
    public Transform customerEndPosition;
    public scrCustomer[] customers;
    public TextMeshProUGUI uiText;
    

    [Header("WorkSpace")]
    public scrClickOnObjectInteraction magnifyingGlass;
    public scrClickOnObjectInteraction brush;
    public scrClickOnObjectInteraction machine;


    //Private
    private TriggerCondition currentCondition = TriggerCondition.onEnd;
    private Transform cameraCustomerPosition;
    private scrCustomer currentCustomer;
    private int currentMoney;
    private int customerMoneyDemand;
    private Vector3 magnifyingGlassStartingScale;
    private GameStateEnum currentGameState = GameStateEnum.WaitingForCustomer;
    private scrMoney currentCoin;
    private Vector3 brushStartingPosition;
    private Vector3 brushStartingScale;
    private Quaternion brushStartingRotation;
    private Vector3 machineStartingScale;
    
    private void Start()
    {
        cameraCustomerPosition = new GameObject().transform;
        cameraCustomerPosition.position = Camera.main.transform.position;
        cameraCustomerPosition.rotation = Camera.main.transform.rotation;
        magnifyingGlassStartingScale = magnifyingGlass.transform.localScale;
        machineStartingScale = machine.transform.localScale;
        brushStartingScale = brush.transform.localScale;
        brushStartingPosition = brush.transform.position;
        brushStartingRotation = brush.transform.rotation;
        Invoke("CreateCustomer", 0.5f);
        
    }

    private void Update()
    {
        ClickControlOnObjects();

    }

    public void MoveBrush(Vector3 _pos)
    {
        brush.transform.rotation = Quaternion.Lerp(scrGameData.values.brushAnimationRots[0], scrGameData.values.brushAnimationRots[1], Mathf.PingPong(Time.time * scrGameData.values.brushAnimationSpeed, 1) );
        brush.transform.position = _pos;
    }

    public void ReturnBrush()
    {
        print("ReturnBrush");
        brush.transform.rotation = brushStartingRotation;
        brush.transform.position = brushStartingPosition;
    }
    
    public void AccepOffer()
    {
        scrCanvasManager.manager.customerTextDisplayer.transform.parent.gameObject.SetActive(false);
        scrCanvasManager.manager.customerButtonPanel.SetActive(false);
        MoveCameraToWork();
        CreateNewMoney();
        StopGlassAnimation();
        magnifyingGlass.transform.DOScale(magnifyingGlassStartingScale * scrGameData.values.mgScaleMultiplier, scrGameData.values.mgScaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

    }

    private void StopMachineAnimation()
    {
        machine.transform.DOKill();
        machine.transform.localScale = machineStartingScale;
    }

    private void StopGlassAnimation()
    {
        magnifyingGlass.transform.DOKill();
        magnifyingGlass.transform.localScale = magnifyingGlassStartingScale;
    }

    private void StopBrushAnimation()
    {
        brush.transform.DOKill();
        brush.transform.localScale = brushStartingScale;
    }

    [ContextMenu("CreateCustomer")]
    public void CreateCustomer()
    {
        if(currentCustomer != null)
        {
            currentCustomer.gameObject.SetActive(false);
        }

        currentGameState = GameStateEnum.WaitingForCustomer;

        scrCanvasManager.manager.customerTextDisplayer.transform.parent.gameObject.SetActive(false);
        scrCanvasManager.manager.customerButtonPanel.SetActive(false);

        currentCustomer = customers[Random.Range(0, customers.Length)];
        currentCustomer.gameObject.SetActive(true);
        currentCustomer.transform.position = customerEnterPosition.position;

        currentCustomer.anim.SetBool("Is Moving", true);
        currentCustomer.transform.DOMove(customerEndPosition.position, scrGameData.values.customerMoveDuration).SetEase(Ease.Linear).OnComplete(() => 
        {
            currentCustomer.anim.SetBool("Is Moving", false);
            customerMoneyDemand = Random.Range((int)scrGameData.values.customerMoneyDemandRange.x, (int)scrGameData.values.customerMoneyDemandRange.y);
            string _randomText = scrGameData.values.customerRequestLines[Random.Range(0, scrGameData.values.customerRequestLines.Length)];
            string _moneyDemandString = string.Format(_randomText, "<color=green>" + customerMoneyDemand + "</color>");
            scrCanvasManager.manager.customerTextDisplayer.text = _moneyDemandString;
            scrCanvasManager.manager.customerTextDisplayer.transform.parent.gameObject.SetActive(true);
            scrCanvasManager.manager.customerButtonPanel.SetActive(true);
            currentGameState = GameStateEnum.WaitingForGlass;
        });


    }

    

    [ContextMenu("CreateNewMoney")]
    public void CreateNewMoney()
    {
        currentCoin = Instantiate(scrGameData.values.moneyList[Random.Range(0, scrGameData.values.moneyList.Count)], moneySpawnPosition.position, moneySpawnPosition.rotation);
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
        Camera.main.transform.DOMove(cameraWorkPosition.position, scrGameData.values.cameraToWorkDuration);
        Camera.main.transform.DORotate(cameraWorkPosition.rotation.eulerAngles, scrGameData.values.cameraToWorkDuration);
        Camera.main.DOFieldOfView(scrGameData.values.cameraWorkFov, scrGameData.values.cameraToWorkDuration);
       
    }

    private void ClickControlOnObjects()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.collider.CompareTag("Interactive Object"))
                {
                    Debug.Log(hit.transform.name);
                    hit.collider.gameObject.GetComponent<scrClickOnObjectInteraction>().Interact();
                }
            }
        }
    }

    public void GlassInteraction()
    {
        print("GlassInteraction: " + currentGameState);
        switch (currentGameState)
        {
            case GameStateEnum.WaitingForCustomer:
                break;
            case GameStateEnum.WaitingForGlass:
                currentCoin.Bigger();
                StopGlassAnimation();
                brush.transform.DOScale(brushStartingScale * scrGameData.values.brushScaleMultiplier, scrGameData.values.brushScaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                currentGameState = GameStateEnum.WaitingForBrush;
                break;
            case GameStateEnum.WaitingForBrush:
                break;
            case GameStateEnum.WaitingForBrushFinish:
                break;
            default:
                break;
        }
    }

    public void BrushInteraction()
    {
        print("BrushInteraction: " + currentGameState);

        switch (currentGameState)
        {
            case GameStateEnum.WaitingForCustomer:
                break;
            case GameStateEnum.WaitingForGlass:
                break;
            case GameStateEnum.WaitingForBrush:
                currentCoin.ReadyForBrush();
                brush.gameObject.GetComponent<Collider>().enabled = false;
                StopBrushAnimation();

                currentGameState = GameStateEnum.WaitingForBrushFinish;
                break;
            case GameStateEnum.WaitingForBrushFinish:
                break;
            default:
                break;
        }
    }

    public void MachineInteraction()
    {
        print("MachineInteraction: " + currentGameState);

        switch (currentGameState)
        {
            case GameStateEnum.WaitingForCustomer:
                break;
            case GameStateEnum.WaitingForGlass:
                break;
            case GameStateEnum.WaitingForBrush:
                break;
            case GameStateEnum.WaitingForBrushFinish:
                break;
            case GameStateEnum.WaitingForMachine:
                currentGameState = GameStateEnum.WaitingForMachine;
                StopMachineAnimation();
                currentCoin.Smaller();
                break;
            case GameStateEnum.WaitingForMachineFinish:
                break;
            default:
                break;
        }
    }

    public void MoneyDustCleaned()
    {
        print("Money Cleared");
        machine.transform.DOScale(machineStartingScale * scrGameData.values.machineScaleMultiplier, scrGameData.values.machineScaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        currentGameState = GameStateEnum.WaitingForMachine;
        ReturnBrush();
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

public enum GameStateEnum
{
    WaitingForCustomer,
    WaitingForGlass,
    WaitingForBrush,
    WaitingForBrushFinish,
    WaitingForMachine,
    WaitingForMachineFinish

}