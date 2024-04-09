using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public KeyCode restartButton = KeyCode.R;

    private void Start()
    {
        UnityEditor.Selection.activeGameObject = this.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(restartButton))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
