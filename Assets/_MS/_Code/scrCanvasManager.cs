using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    public Transform moneyStartPosition;


 

    public void CreateMoney(int _val)
    {
        GameObject go = new GameObject();
        go.transform.parent = this.transform;
        go.transform.position = moneyStartPosition.position;
        Image _img = go.AddComponent<Image>();
        _img.sprite = scrGameData.values.moneySprite;
        go.transform.DOMove(moneyDisplayer.transform.parent.GetChild(0).position, scrGameData.values.moneySpriteMoveDuration).SetEase(Ease.Linear).OnComplete(() => 
        {
            DisplayMoney(_val);
            Destroy(go);
        });
    }

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

    public void ToggleDebugMenu(GameObject _debugMenu)
    {
        _debugMenu.SetActive(!_debugMenu.activeSelf);
    }

    public void DisplayWorkshopText(string _val)
    {
        workshopTextDisplayer.text = _val;
    }
}
