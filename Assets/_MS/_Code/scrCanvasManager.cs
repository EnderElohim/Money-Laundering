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
    public GameObject customerButtonPanel;



    public void AccepOffer()
    {
        scrGameManager.manager.AccepOffer();
    }
}
