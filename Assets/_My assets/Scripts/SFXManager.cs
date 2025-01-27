using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioClip uiNavigationClip;

    private void OnEnable()
    {
        ActionManager.UiNavigated += OnUiNavigated;
    }

    private void OnDisable()
    {
        ActionManager.UiNavigated -= OnUiNavigated;
    }

    private void OnUiNavigated() => mainAudioSource.PlayOneShot(uiNavigationClip);  
}
