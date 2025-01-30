using UnityEngine;
using System.Collections.Generic;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] private List<CanvasIdentity> canvasList = new List<CanvasIdentity>();

    private void Awake()
    {
        instance = this;
    }

    public void OpenCanvas(CanvasNames desireCanvas)
    {
        foreach (CanvasIdentity canvas in canvasList)
        {
            if (canvas.GetCanvasName() == desireCanvas)
            {
                canvas.OpenCanvas();
            }
            else
            {
                canvas.CloseCanvas();
            }
        }
    }

    public void CloseCanvas(CanvasNames desireCanvas)
    {
        foreach (CanvasIdentity canvas in canvasList)
        {
            if (canvas.GetCanvasName() == desireCanvas)
            {
                canvas.CloseCanvas();
            }
        }
    }

    public void CloseAllCanvas()
    {
        foreach (CanvasIdentity canvas in canvasList)
        {
            canvas.CloseCanvas();
        }
    }
}