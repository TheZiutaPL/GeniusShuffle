using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] InteractableAnimationHandler tutorialWindow;
    const string TUTORIAL_FINISHED_KEY = "TUTORIAL_FINISHED";
    int tutorialFinished = 0;

    private void Start()
    {
        tutorialFinished = PlayerPrefs.GetInt(TUTORIAL_FINISHED_KEY, 0);
    }

    public void ShowTutorial()
    {
        if (tutorialFinished == 0) 
        {
            InteractionManager.instance.EnableInteractions(false);

            tutorialWindow.HandleAnimation(true);

            tutorialFinished = 1;
            PlayerPrefs.SetInt(TUTORIAL_FINISHED_KEY, tutorialFinished);
        }
    }
}
