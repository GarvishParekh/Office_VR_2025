using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MainMenuUiController : MonoBehaviour
{
    UiManager uiManager;

    [Header("<size=13><b>Vr component")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerHeadTransform;
    [SerializeField] private List<GameObject> uiRayPointList = new List<GameObject>();
    [SerializeField] private Transform meetingCanvasTransform;

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

    //--------------------------- TOGGLE FUNCTIONS ---------------------------
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
    
    //--------------------------- BUTTON FUNCTIONS ---------------------------

    public void StartOfficeTour()
    {
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
        uiManager.OpenCanvas(CanvasNames.C_INSTRUCTION);
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

    public void B_OrgOverview()
    {
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
        uiManager.OpenCanvas(CanvasNames.C_ORG_OVERVIEW);
    }

    public void B_RoomForLearning()
    {
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
        uiManager.OpenCanvas(CanvasNames.C_OPPORTUNITIES_AND_LEARNING);
    }

    public void B_CloseInstruction()
    {
        foreach (GameObject rayPointer in uiRayPointList)
        {
            //rayPointer.SetActive(false);
        }
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);
        uiManager.CloseAllCanvas();
        hotspotPoints.SetActive(true);
    }

    public void B_OpenSetMeeting()
    {
        Vector3 headRotation = playerTransform.eulerAngles;
        headRotation.x = 0; headRotation.z = 0;

        Vector3 newPosition = playerTransform.position;

        meetingCanvasTransform.position = newPosition;
        meetingCanvasTransform.rotation = Quaternion.Euler(headRotation);

        uiManager.OpenPopUp(CanvasNames.P_MEETING);
    }

    public void B_CloseSetMeeting()
    {
        meetingCanvasTransform.position = playerTransform.position;
        uiManager.ClosePopUp(CanvasNames.P_MEETING);
    }

}
