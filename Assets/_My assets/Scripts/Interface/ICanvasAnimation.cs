using System;

public interface ICanvasAnimation
{
    public void OpenCanvas();
    public void CloseCanvas(Action<bool> success);
    public void ResetCanvas();
}