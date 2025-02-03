using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class MainMenuUiController : MonoBehaviour
{
    UiManager uiManager;

    [Header("<size=13><b>Arrow animation component")]
    [SerializeField] private CanvasGroup meetOurTeamCanvas;
    [SerializeField] private GameObject arrowHolder;
    [SerializeField] private Transform endpoint;

    [Header("<size=13><b>Meeting canvas component")]
    [SerializeField] private TMP_Text employeeNameText;
    [SerializeField] private CanvasGroup meetingConfirmedToast;
    [SerializeField] private GameObject meetingCanvasButtonsHolder;
    [SerializeField] private Transform meetingCanvasTransform;
    [SerializeField] private Vector3 meetingCanvasDefaultPos;
    [SerializeField] private List<CanvasGroup> employeeCardsList = new List<CanvasGroup>();

    [Header("<size=13><b>Vr component")]
    [SerializeField] private GameObject startupCanvas;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerHeadTransform;
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
        Application.targetFrameRate = 120;
        startupCanvas.SetActive(true);

        meetingCanvasDefaultPos = meetingCanvasTransform.position;
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

        yield return new WaitForSeconds(2f);
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

        StartArrowAnimation();
    }

    public void B_OpenSetMeeting(string employeeName)
    {
        employeeNameText.text = employeeName;
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);

        meetingCanvasButtonsHolder.SetActive(true);
        meetingConfirmedToast.gameObject.SetActive(false);

        Vector3 headRotation = playerTransform.eulerAngles;
        headRotation.x = 0; headRotation.z = 0;

        Vector3 newPosition = playerTransform.position;
        newPosition.y = meetingCanvasDefaultPos.y;   

        meetingCanvasTransform.position = newPosition;
        meetingCanvasTransform.rotation = Quaternion.Euler(headRotation);

        EmployeeCard(false);
        hotspotPoints.SetActive(false);

        uiManager.OpenPopUp(CanvasNames.P_MEETING);
    }

    public void B_CancelMeeting()
    {
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);

        uiManager.ClosePopUp(success: (bool isSuccess) =>
        {
            if (isSuccess)
            {
                Vector3 defaultPosition = meetingCanvasDefaultPos;
                meetingCanvasTransform.position = defaultPosition;

                EmployeeCard(true);
                hotspotPoints.SetActive(true);
            }
        },
        CanvasNames.P_MEETING
        );
    }

    public void B_ConfirmMeeting()
    {
        ActionManager.UiNavigated?.Invoke(E_ButtonSFX.BUTTON_SFX);

        meetingCanvasButtonsHolder.SetActive(false);
        meetingConfirmedToast.gameObject.SetActive(true);
        meetingConfirmedToast.alpha = 1;

        LeanTween.alphaCanvas(meetingConfirmedToast, 0, 0.5f).setDelay(1f).setOnComplete(() =>
        {
            uiManager.ClosePopUp(success: (bool isSuccess) =>
            {
                if (isSuccess)
                {
                    Vector3 defaultPosition = meetingCanvasDefaultPos;
                    meetingCanvasTransform.position = defaultPosition;

                    EmployeeCard(true);
                    hotspotPoints.SetActive(true);
                }
            },
            CanvasNames.P_MEETING
            );
        });
    }

    private void EmployeeCard(bool enable)
    {
        if (enable == true)
        {
            foreach (CanvasGroup card in employeeCardsList)
            {
                LeanTween.alphaCanvas(card, 1, 0.6f).setEaseInOutSine();
                card.interactable = true;
            }
        }
        else if (enable == false)
        {
            foreach (CanvasGroup card in employeeCardsList)
            {
                LeanTween.alphaCanvas(card, 0, 0.6f).setEaseInOutSine();
                card.interactable = false;
            }
        }
        else return;
    }

    private void StartArrowAnimation()
    {
        meetOurTeamCanvas.gameObject.SetActive(true);
        LeanTween.move(arrowHolder, endpoint.position, 2f).setLoopCount(10).setOnComplete(()=>
        {
            LeanTween.alphaCanvas(meetOurTeamCanvas, 0, 1f).setOnComplete(()=>
            {
                meetOurTeamCanvas.gameObject.SetActive(false);
            });
        });
    }
}
