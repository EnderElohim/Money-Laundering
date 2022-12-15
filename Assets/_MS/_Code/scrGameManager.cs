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
    public Transform door;
    public Transform cameraWorkPosition;
    public Transform moneySpawnPosition;
    public Transform customerEnterPosition;
    public Transform customerEndPosition;
    public Transform[] customerLeftPositions;
    public Transform customerCreationPosition;
    public Transform glassActivePosition;
    public scrCustomer[] customers;
    public TextMeshProUGUI uiText;
    

    [Header("WorkSpace")]
    public scrClickOnObjectInteraction magnifyingGlass;
    public scrClickOnObjectInteraction brush;
    public scrClickOnObjectInteraction machine;


    //Private
    private Transform cameraCustomerPosition;
    private scrCustomer currentCustomer;
    private int currentMoney;
    private int customerMoneyDemand;
    private Vector3 magnifyingGlassStartingScale;
    private Vector3 magnifyingGlassStartingPosition;
    private Quaternion magnifyingGlassStartingRotation;
    private GameStateEnum currentGameState = GameStateEnum.WaitingForCustomer;
    private scrMoney currentCoin;
    private Vector3 brushStartingPosition;
    private Vector3 brushStartingScale;
    private Quaternion brushStartingRotation;
    private Vector3 machineStartingScale;
    private int currentCustomerId;
     
    
    private void Start()
    {
        cameraCustomerPosition = new GameObject().transform;
        cameraCustomerPosition.position = Camera.main.transform.position;
        cameraCustomerPosition.rotation = Camera.main.transform.rotation;
        magnifyingGlassStartingScale = magnifyingGlass.transform.localScale;
        magnifyingGlassStartingPosition = magnifyingGlass.transform.position;
        magnifyingGlassStartingRotation = magnifyingGlass.transform.rotation;
        machineStartingScale = machine.transform.localScale;
        brushStartingScale = brush.transform.localScale;
        brushStartingPosition = brush.transform.position;
        brushStartingRotation = brush.transform.rotation;
        Invoke("CreateCustomer", 0.5f);
        
    }

    private void Update()
    {
        ClickControlOnObjects();
        if(currentGameState == GameStateEnum.WaitingForConfirm && currentCoin != null) 
        {
            currentCoin.transform.RotateAround(Vector3.up, Time.deltaTime);
        }
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
        scrCanvasManager.manager.DisplayWorkshopText("Click on Magnifying Glass");
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
        currentGameState = GameStateEnum.WaitingForCustomer;

        scrCanvasManager.manager.customerTextDisplayer.transform.parent.gameObject.SetActive(false);
        scrCanvasManager.manager.customerButtonPanel.SetActive(false);
       

        currentCustomer = customers[currentCustomerId];
        currentCustomerId++;

        if (currentCustomerId >= customers.Length) { currentCustomerId = 0; }
        currentCustomer.gameObject.SetActive(true);
        currentCustomer.transform.position = customerCreationPosition.position;
        currentCustomer.transform.LookAt(customerEnterPosition);
        currentCustomer.anim.SetBool("Is Moving", true);
        currentCustomer.transform.DOMove(customerEnterPosition.position, scrGameData.values.customerMoveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            door.transform.DORotate(new Vector3(0, 270, 0), 0.5f);
            currentCustomer.transform.DOLookAt(customerEndPosition.position, 0.1f).SetEase(Ease.Linear).OnComplete(()=> 
            {
                currentCustomer.transform.DOMove(customerEndPosition.position, scrGameData.values.customerMoveDuration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    door.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
                    currentCustomer.anim.SetBool("Is Moving", false);
                    customerMoneyDemand = Random.Range((int)scrGameData.values.customerMoneyDemandRange.x, (int)scrGameData.values.customerMoneyDemandRange.y);
                    string _randomText = scrGameData.values.customerRequestLines[Random.Range(0, scrGameData.values.customerRequestLines.Length)];
                    string _moneyDemandString = string.Format(_randomText, "<color=green>" + customerMoneyDemand + "</color>");
                    ChangeString(scrCanvasManager.manager.customerTextDisplayer, _moneyDemandString, scrGameData.values.customerTextDuration);
                    //scrCanvasManager.manager.customerTextDisplayer.text = _moneyDemandString;
                    scrCanvasManager.manager.customerTextDisplayer.transform.parent.gameObject.SetActive(true);
                   

                });
            });
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

    public void MoneyToMachine()
    {
        currentCoin.transform.DOMove(machine.transform.GetChild(0).position, scrGameData.values.moneyToMachineMoveDuration).OnComplete(()=> 
        {
            currentCoin.transform.DOMove(machine.transform.position, 0.5f).OnComplete(() =>
            {
                currentCoin.transform.position = new Vector3(1000, 1000, 1000);
                scrCanvasManager.manager.DisplayWorkshopText("Wait for Machine");
                machine.transform.DOShakeRotation(scrGameData.values.machineShakeDuration, scrGameData.values.machineStrength, scrGameData.values.machineVibrato, scrGameData.values.machineRandomness).SetEase(Ease.Linear).SetLoops(scrGameData.values.machineLoopCount, LoopType.Yoyo).OnComplete(() => 
                {
                    scrCanvasManager.manager.DisplayWorkshopText("");
                    currentCoin.transform.position = machine.transform.GetChild(1).position;
                    currentCoin.transform.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
                    currentCoin.transform.DOMove(moneySpawnPosition.position + (Vector3.up), 1);
                    currentGameState = GameStateEnum.WaitingForConfirm;
                    scrCanvasManager.manager.finishButton.SetActive(true);

                    currentCoin.Bigger();

                });
            });
        });
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
                magnifyingGlass.transform.DOMove(glassActivePosition.position, 0.1f);
                magnifyingGlass.transform.DORotate(glassActivePosition.rotation.eulerAngles, 0.1f);
                brush.transform.DOScale(brushStartingScale * scrGameData.values.brushScaleMultiplier, scrGameData.values.brushScaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                currentGameState = GameStateEnum.WaitingForBrush;
                scrCanvasManager.manager.DisplayWorkshopText("Click on Brush");
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
                scrCanvasManager.manager.DisplayWorkshopText("Brush the coin");
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
                scrCanvasManager.manager.DisplayWorkshopText("");
                currentGameState = GameStateEnum.WaitingForMachine;
                StopMachineAnimation();
                currentCoin.Smaller();
                magnifyingGlass.transform.DOMove(magnifyingGlassStartingPosition, 0.1f);
                magnifyingGlass.transform.DORotate(magnifyingGlassStartingRotation.eulerAngles, 0.1f);
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
        scrCanvasManager.manager.DisplayWorkshopText("Click on Machine");
        ReturnBrush();
    }

    private string GetColouredString(string _val, Color _color)
    {
        return string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(_color.r * 255f), (byte)(_color.g * 255f), (byte)(_color.b * 255f), _val);
    }

    public void ConfirmButton()
    {
        scrCanvasManager.manager.finishButton.SetActive(false);
        brush.gameObject.GetComponent<Collider>().enabled = true;
        Destroy(currentCoin.gameObject);
        MoveCameraToCustomer();
        
        currentMoney += customerMoneyDemand;
        scrCanvasManager.manager.CreateMoney(currentMoney);
        door.transform.DORotate(new Vector3(0, 270, 0), 1);
        scrCustomer _oldCustomer = currentCustomer;
        currentCustomer.anim.SetBool("Is Moving", true);
        Transform _existPosition = customerLeftPositions[(Random.Range(0, 4) == 0 ? 0 : 1)];
        _oldCustomer.transform.DOLookAt(customerEnterPosition.position, 0.1f).SetEase(Ease.Linear).OnComplete(()=> 
        {
            _oldCustomer.transform.DOMove(customerEnterPosition.position, scrGameData.values.customerMoveDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                _oldCustomer.transform.DOLookAt(_existPosition.position, 0.1f).SetEase(Ease.Linear).OnComplete(()=>
                {
                    CreateCustomer();
                    door.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
                    _oldCustomer.transform.DOMove(_existPosition.position, 2).SetEase(Ease.Linear).OnComplete(() => 
                    {
                        _oldCustomer.gameObject.SetActive(false);
                     
                    });
                });
            });
        });
       
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        bool startedSkipping = false;
        bool isColourFinded = false;
        for (int i = 0; i <= _val.Length; i++)
        {
            if(isColourFinded == false)
            {
                if (_val.Substring(i, 1) == "<")
                {
                    isColourFinded = true;
                    startedSkipping = true;
                    continue;
                }
            }

            if (startedSkipping == true)
            {
                if (_val.Substring(i, 1) == "$")
                {
                    startedSkipping = false;
                }
                else
                {
                    continue;
                }
            }


            _text.text = _val.Substring(0, i);

            yield return new WaitForSeconds(calculatedTime);
        }

        scrCanvasManager.manager.customerButtonPanel.SetActive(true);
        currentGameState = GameStateEnum.WaitingForGlass;
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



public enum GameStateEnum
{
    WaitingForCustomer,
    WaitingForGlass,
    WaitingForBrush,
    WaitingForBrushFinish,
    WaitingForMachine,
    WaitingForMachineFinish,
    WaitingForConfirm

}