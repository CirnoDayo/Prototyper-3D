using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lan_LivesUI : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    private static int _lives;
    public int startLives = 20;
    [SerializeField] private int _round;
    [SerializeField] private int damageTaken = 1;
    [SerializeField] private List<Button> allButtons;
    
    // Start is called before the first frame update
    void Start()
    {
        _lives = startLives;
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            allButtons.Add(button);
        }
    }
    
   
    public void EnemyReachedEnd()
    {
        _lives -= damageTaken;
        livesText.text = _lives.ToString() + " LIVES";
        
        if(_lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        livesText.text = "GAME OVER!";
        Debug.Log("Game Over!");
        foreach (var button in allButtons)
        {
            button.interactable = false;
        }
    }
}
