using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public static bool freezeTime;
    public static bool speedBoost;
    public static bool healthPack;
    public static bool nuke;

    private void Start()
    {
        freezeTime = false; 
        speedBoost = false;
        healthPack = false;
        nuke = false;
    }

    private void Update()
    {
        if (freezeTime) {
            statusText.text = "Time Frozen....";
        } else if (speedBoost) {
            statusText.text = "Base Speed Increased";
        } else if (healthPack) {
            statusText.text = "Added an extra life...";
        } else if (nuke) {
            statusText.text = "Incoming Nuke...";
        } else
        {
            statusText.text = "";
        }
    }

}
