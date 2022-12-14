using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectClickOnUIObject : MonoBehaviour, IPointerClickHandler
{
    public GameObject toClose;
    public bool isImmidiateClose;
    public bool disableVFXSound;
    public bool tutorialSequenceInturrupt;
    public bool disableUsingUI;
    public bool isClosingScreen = true;

    private void OnEnable()
    {
        if (toClose && UIManager.Instance)
        {
            StartCoroutine(UIManager.Instance.SetIsUsingUI(true));
        }
    }

    private void OnDisable()
    {
        if (isImmidiateClose && disableUsingUI)
        {
           UIManager.Instance.CallSetIsUsingUI(false);
        }

        if (disableVFXSound)
        {
            SoundManager.Instance.audioSourceSFX.Stop();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked HERE");

        if (isClosingScreen)
        {
            if (tutorialSequenceInturrupt)
            {
                if (!TutorialSequence.Instacne.duringSequence)
                {
                    CloseWindow();
                }
            }
            else
            {
                CloseWindow();
            }
        }
    }

    void CloseWindow()
    {
        if (disableUsingUI)
        {
            StartCoroutine(UIManager.Instance.SetIsUsingUI(false));
        }

        if (isImmidiateClose)
        {
            UIManager.Instance.CloseWindowNoAdditionalAction(toClose);
        }
        else
        {
            UIManager.Instance.closeWindow(toClose);
        }
    }
}
