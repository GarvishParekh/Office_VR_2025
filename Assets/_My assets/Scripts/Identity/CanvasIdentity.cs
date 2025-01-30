using UnityEngine;

[RequireComponent (typeof(CanvasGroup))] 
public class CanvasIdentity: MonoBehaviour, ICanvasAnimation
{
    CanvasGroup canvasGroup;
    [SerializeField] private CanvasNames myCanvasName;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public CanvasNames GetCanvasName()
    {
        return myCanvasName;
    }

    public void OpenCanvas() 
    {
        ResetCanvas();
        LeanTween.scale(gameObject, Vector3.one, 0.4f).setEaseInSine();
        LeanTween.alphaCanvas(canvasGroup, 1, 0.5f).setEaseInSine();
        canvasGroup.interactable = true;
    }
    public void CloseCanvas()
    {
        canvasGroup.interactable = false;
        LeanTween.scale(gameObject, Vector3.one * 1.3f, 0.5f).setEaseInSine();
        LeanTween.alphaCanvas(canvasGroup, 0, 0.4f).setEaseInSine();
    }
    public void ResetCanvas()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        transform.localScale = Vector3.one * 1.3f;
    }
}


public enum CanvasNames
{
    C_HOME,
    C_HRPOLICY,
    C_INSTRUCTION
}