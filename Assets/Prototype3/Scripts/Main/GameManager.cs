using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public KeyCode restartButton = KeyCode.R;

    private void Start()
    { 
#if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = this.gameObject;
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(restartButton))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
