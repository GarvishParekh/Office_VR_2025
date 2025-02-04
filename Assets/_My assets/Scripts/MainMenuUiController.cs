using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuUiController : MonoBehaviour
{
    [SerializeField] private CanvasGroup pressAImage;
    [SerializeField] private GameObject whiteBGImage;
    [SerializeField] private CanvasGroup officeImageWhite;
    [SerializeField] private CanvasGroup officeImageBlack;

    bool expStarted = false;

    private void Awake()
    {
        Application.targetFrameRate = 90;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (expStarted) return;

            ActionManager.UiNavigated?.Invoke();
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
            LeanTween.alphaCanvas(officeImageBlack, 1, 0.2f).setEaseInSine();
        });
        officeImageWhite.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);

        whiteBGImage.gameObject.SetActive(false);
        officeImageWhite.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        LeanTween.alphaCanvas(officeImageWhite, 0, 0.5f).setEaseInSine();
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
