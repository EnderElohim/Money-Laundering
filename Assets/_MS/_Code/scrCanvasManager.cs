using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrCanvasManager : MonoBehaviour
{
    #region Singleton
    public static scrCanvasManager manager;
    private void Awake()
    {
        manager = this;
    }
    #endregion
    public TextMeshProUGUI customerTextDisplayer;
    public TextMeshProUGUI workshopTextDisplayer;
    public GameObject customerButtonPanel;
    public GameObject finishButton;
    public TextMeshProUGUI moneyDisplayer;
    public GameObject debugMenu;
    

    public void AccepOffer()
    {
        scrGameManager.manager.AccepOffer();
    }

    public void ConfirmButton()
    {
        scrGameManager.manager.ConfirmButton();
    }

    public void DisplayMoney(int _val)
    {
        moneyDisplayer.text = _val.ToString() + "$";
    }

    public void ToggleDebugMenu()
    {
        debugMenu.SetActive(!debugMenu.activeSelf);
    }

    public void DisplayWorkshopText(string _val)
    {
        workshopTextDisplayer.text = _val;
    }
}
