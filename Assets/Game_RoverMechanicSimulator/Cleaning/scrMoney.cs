using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using DG.Tweening;

public class scrMoney : MonoBehaviour {

    public Vector3 workScale;
    public float scaleDuration;
    public LayerMask moneyMask;

    [SerializeField] private Texture2D dirtMaskTextureBase;
    [SerializeField] private Texture2D dirtBrush;
    [SerializeField] private Material material;
    [SerializeField] private TextMeshProUGUI uiText;

    private Texture2D dirtMaskTexture;
    private bool isFlipped;
    private Animation solarAnimation;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;
    private Vector3 startingScale;
    private bool isDustCleared;
    private bool isReadyForClear;

    private void Awake() {
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("_DirtMask", dirtMaskTexture);

        solarAnimation = GetComponent<Animation>();

        dirtAmountTotal = 0f;
        for (int x = 0; x < dirtMaskTextureBase.width; x++) {
            for (int y = 0; y < dirtMaskTextureBase.height; y++) {
                dirtAmountTotal += dirtMaskTextureBase.GetPixel(x, y).g;
            }
        }
        dirtAmount = dirtAmountTotal;

        startingScale = transform.localScale;
    }

    private void Update() 
    {
        if (isDustCleared == true) return;
        if (isReadyForClear == false) return;

        if(Mathf.RoundToInt(100 - GetDirtAmount() * 100f) >= 100)
        {
            isDustCleared = true;
            scrGameManager.manager.MoneyDustCleaned();

            dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
            dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
            dirtMaskTexture.Apply();

            for (int x = 0; x < dirtMaskTexture.width; x++)
            {
                for (int y = 0; y < dirtMaskTexture.height; y++)
                {
                    dirtMaskTexture.SetPixel(x, y, Color.black);
                }
            }
            dirtMaskTexture.Apply();
            material.SetTexture("_DirtMask", dirtMaskTexture);
            return;
        }

        if (Input.GetMouseButton(0)) {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, Mathf.Infinity, moneyMask)) 
            {
                print(raycastHit.collider.name);
                scrGameManager.manager.MoveBrush(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrGameData.values.zDistance)));
                Vector2 textureCoord = raycastHit.textureCoord;

                int pixelX = (int)(textureCoord.x * dirtMaskTexture.width);
                int pixelY = (int)(textureCoord.y * dirtMaskTexture.height);

                Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
                //Debug.Log("UV: " + textureCoord + "; Pixels: " + paintPixelPosition);

                int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                int maxPaintDistance = 7;
                if (paintPixelDistance < maxPaintDistance) {
                    // Painting too close to last position
                    return;
                }
                lastPaintPixelPosition = paintPixelPosition;

                /*
                // Paint Square in Dirt Mask
                int squareSize = 32;
                int pixelXOffset = pixelX - (dirtBrush.width / 2);
                int pixelYOffset = pixelY - (dirtBrush.height / 2);

                for (int x = 0; x < squareSize; x++) {
                    for (int y = 0; y < squareSize; y++) {
                        dirtMaskTexture.SetPixel(
                            pixelXOffset + x,
                            pixelYOffset + y,
                            Color.black
                        );
                    }
                }
                //*/


                //* 
                int pixelXOffset = pixelX - (dirtBrush.width / 2);
                int pixelYOffset = pixelY - (dirtBrush.height / 2);

                for (int x = 0; x < dirtBrush.width; x++) {
                    for (int y = 0; y < dirtBrush.height; y++) {
                        Color pixelDirt = dirtBrush.GetPixel(x, y);
                        Color pixelDirtMask = dirtMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);

                        float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                        dirtAmount -= removedAmount;
                        
                        dirtMaskTexture.SetPixel(
                            pixelXOffset + x, 
                            pixelYOffset + y, 
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                        );

                       
                    }
                }
                //*/

                dirtMaskTexture.Apply();
            }
            if (Input.GetMouseButtonUp(0))
            {
                scrGameManager.manager.ReturnBrush();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            isFlipped = !isFlipped;
            if (isFlipped) {
                solarAnimation.Play("SolarPanelFlip");
            } else {
                solarAnimation.Play("SolarPanelFlipBack");
            }
        }
    }

    public void DisplayRatio()
    {
        uiText = scrGameManager.manager.uiText;

        FunctionPeriodic.Create(() => {
            uiText.text = Mathf.RoundToInt(100 - GetDirtAmount() * 100f) + "%";
        }, .03f);
    }

    [ContextMenu("Bigger")]
    public void Bigger()
    {
        transform.DOScale(workScale, scaleDuration);
    }

    [ContextMenu("Smaller")]
    public void Smaller()
    {
        transform.DOScale(startingScale, scaleDuration).OnComplete(()=> 
        {
            scrGameManager.manager.MoneyToMachine();
        });
        
    }

    public void ReadyForBrush()
    {
        isReadyForClear = true;
    }


    private float GetDirtAmount() 
    {
        return this.dirtAmount / dirtAmountTotal;
    }

}
