using System.Collections;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    public void StartGame()
    {
        gameObject.SetActive(false);
    }

    public void hideScreen()
    {
        gameObject.SetActive(false);
    }

    public void returnMainMenu()
    {
        gameObject.SetActive(true);
    }

}
