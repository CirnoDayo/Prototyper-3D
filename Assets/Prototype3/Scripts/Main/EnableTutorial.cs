using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnableTutorial : MonoBehaviour
{
    public GameObject tutorialPanel;
    
    public void TogglePanel()
    {
        tutorialPanel.SetActive(!tutorialPanel.activeInHierarchy);
    }
}
