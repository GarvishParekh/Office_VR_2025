using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudioSourceHigh;
    [SerializeField] private AudioSource mainAudioSourceMid;
    [SerializeField] private AudioClip uiNavigationClip;
    [SerializeField] private AudioClip buttonSelectedClip;

    private void OnEnable()
    {
        ActionManager.UiNavigated += OnUiNavigated;
    }

    private void OnDisable()
    {
        ActionManager.UiNavigated -= OnUiNavigated;
    }

    private void OnUiNavigated(E_ButtonSFX desireSFX)
    {
        switch (desireSFX)
        {
            case E_ButtonSFX.OPENING_SFX:
                mainAudioSourceHigh.PlayOneShot(uiNavigationClip);  
                break;
            case E_ButtonSFX.BUTTON_SFX:
                mainAudioSourceHigh.PlayOneShot(buttonSelectedClip);  
                break;
        }
    }
}

public enum E_ButtonSFX
{
    OPENING_SFX,
    BUTTON_SFX
}
