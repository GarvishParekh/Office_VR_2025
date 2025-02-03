using UnityEngine;
using System.Collections.Generic;
using System;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] private List<CanvasIdentity> canvasList = new List<CanvasIdentity>();
    [SerializeField] private List<CanvasIdentity> popupList = new List<CanvasIdentity>();

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
                canvas.CloseCanvas(success: (bool isSuccess)=>
                {
                    // null
                });
            }
        }
    }

    public void CloseCanvas(CanvasNames desireCanvas)
    {
        foreach (CanvasIdentity canvas in canvasList)
        {
            if (canvas.GetCanvasName() == desireCanvas)
            {
                canvas.CloseCanvas(success: (bool isSuccess) =>
                {
                    // null
                });
            }
        }
    }

    public void CloseAllCanvas()
    {
        foreach (CanvasIdentity canvas in canvasList)
        {
            canvas.CloseCanvas(success: (bool isSuccess) =>
            {
                // null
            });
        }
    }

    public void OpenPopUp(CanvasNames desireCanvas)
    {
        foreach (CanvasIdentity poppup in popupList)
        {
            if (poppup.GetCanvasName() == desireCanvas)
            {
                poppup.OpenCanvas();
            }
        }
    }

    public void ClosePopUp(Action<bool> success, CanvasNames desireCanvas)
    {
        foreach (CanvasIdentity poppup in popupList)
        {
            if (poppup.GetCanvasName() == desireCanvas)
            {
                poppup.CloseCanvas(success: (bool isSuccess) =>
                {
                    success?.Invoke(true);
                });
            }
        }
    }

}