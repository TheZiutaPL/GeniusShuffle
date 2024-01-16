using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuList : MonoBehaviour
{
    public int currentOption;
    [SerializeField] List<string> allOptions;
    [SerializeField] TMP_Text displayText;

    public void NextOption() => SetOption(currentOption < allOptions.Count - 1 ? ++currentOption : 0);
    public void PreviousOption() => SetOption(currentOption > 0 ? --currentOption : allOptions.Count - 1);
    public void SetOption(int option)
    {
        currentOption = Mathf.Clamp(option, 0, allOptions.Count - 1);

        if (displayText != null) 
            displayText.text = allOptions[currentOption]; // TODO: Some animations
    }
}
