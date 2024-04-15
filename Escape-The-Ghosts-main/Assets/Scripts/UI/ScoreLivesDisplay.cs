using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreLivesDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI livesDisplay;

    public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        livesDisplay.text = "Lives: " + player.currentHealth;
        scoreDisplay.text = "Score: " + player.playerScore + "/30";
    }
}
