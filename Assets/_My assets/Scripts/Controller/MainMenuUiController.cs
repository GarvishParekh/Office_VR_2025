using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuUiController : MonoBehaviour
{
    UiManager uiManager;

    [Header("<size=13><b>Vr component")]
    [SerializeField] private List<GameObject> uiRayPointList = new List<GameObject>();

    [Header("<size=13><b>Home ui animation")]
    [SerializeField] private CanvasGroup homeCanvas;

    [Header ("<size=13><b>Hr policies")]
    [SerializeField] private List<Toggle> contentToggleList = new List<Toggle>();
    [SerializeField] private List<GameObject> contentHolderList = new List<GameObject>();


    [Header ("<size=13><b>Startup animation")]
    [SerializeField] private CanvasGroup pressAImage;
    [SerializeField] private GameObject whiteBGImage;
    [SerializeField] private CanvasGroup officeImageWhite;
    [SerializeField] private CanvasGroup officeImageBlack;

    [Header("<size=13><b>Office tour components")]
    [SerializeField] private GameObject hotspotPoints;


    bool expStarted = false;

    private void Awake()
    {
        Application.targetFrameRate = 90;
        hotspotPoints.SetActive(false);
    }

    private void Start()
    {
        uiManager = UiManager.instance;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (expStarted) return;

            ActionManager.UiNavigated?.Invoke(E_ButtonSFX.OPENING_SFX);
            StartCoroutine(nameof(StartTheExp));
        }

        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            ResetScene();
        }
    }

    IEnumerator StartTheExp()
    {
        expStarted = true;

        LeanTween.alphaCanvas(pressAImage, 0, 0.2f).setEaseInSine();
        LeanTween.scale(pressAImage.gameObject, Vector3.one * 5, 0.5f).setEaseInSine().setOnComplete(()=>
        {
            uiManager.CloseAllCanvas();
            LeanTween.alphaCanvas(officeImageBlack, 1, 0.2f).setEaseInSine();
        });
        officeImageWhite.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);

        whiteBGImage.gameObject.SetActive(false);
        officeImageWhite.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        LeanTween.alphaCanvas(officeImageWhite, 0, 0.5f).setEaseInSine().setOnComplete(()=>
        {
            homeCanvas.transform.localScale = Vector3.zero;
            uiManager.OpenCanvas(CanvasNames.C_HOME);
        });
        
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartOfficeTour()
    {
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
        uiManager.OpenCanvas(CanvasNames.C_INSTRUCTION);
    }

    public void B_OnToggleChange()
    {
        foreach (Toggle contentToggle in contentToggleList) 
        {
            //ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
            int index = contentToggle.transform.GetSiblingIndex();

            if (contentToggle.isOn) contentHolderList[index].SetActive(true);
            else contentHolderList[index].SetActive(false); ;
        }
    }

    public void B_OpenHRPolicies()
    {
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
        uiManager.OpenCanvas(CanvasNames.C_HRPOLICY);
    }


    public void B_OpenHomeButton()
    {
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
        uiManager.OpenCanvas(CanvasNames.C_HOME);
    }

    public void B_CloseInstruction()
    {
        foreach (GameObject rayPointer in uiRayPointList)
        {
            rayPointer.SetActive(false);
        }
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
        uiManager.CloseAllCanvas();
        hotspotPoints.SetActive(true);
    }
}
