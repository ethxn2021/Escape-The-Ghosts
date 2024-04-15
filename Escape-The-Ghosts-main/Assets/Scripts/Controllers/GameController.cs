using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject spawnerController;
    public GameOverScreen gameOverScreen;
    public MainMenuScreen mainMenuScreen;
    public VictoryScreenScript victoryScreen;
    public LoadingScreen loadingScreen;
    public GameObject livesDisplay;
    public GameObject scoreDisplay;
    public GameObject staminaSlider;
    public GameObject menuCam;
    public GameObject powerup;
    public GameObject powerupStatus;
    public GameObject tutorialScreen;

    public GameObject gameLevel;
    public GameObject player;
    public GameObject backgroundMusicSource;


    private const string gameSceneName = "Game";

    public void StartGame()
    {
        StartCoroutine(LoadGame());
    }

    public void ShowTutorial()
    {
        mainMenuScreen.hideScreen();
        tutorialScreen.SetActive(true);
    }

    public void goBack()
    {
        tutorialScreen.SetActive(false);
        mainMenuScreen.returnMainMenu();
    }

    public void EndGame()
    {
        menuCam.SetActive(true);
        player.SetActive(false);
        livesDisplay.SetActive(false);
        scoreDisplay.SetActive(false);
        staminaSlider.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverScreen.Setup();
        PauseGame();
    }

    public void RestartGame()
    {
        ResumeGame();  // Ensure the game is not paused when reloading the scene
        SceneManager.LoadScene(gameSceneName);
    }

    public void VictoryScreen()
    {
        menuCam.SetActive(true);
        player.SetActive(false);
        livesDisplay.SetActive(false);
        scoreDisplay.SetActive(false);
        staminaSlider.SetActive(false);
        powerup.SetActive(false);
        powerupStatus.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        victoryScreen.Setup();
        PauseGame();

    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private IEnumerator LoadGame()
    {
        //Level gen
        gameLevel.SetActive(true);

        loadingScreen.Setup();
        yield return new WaitForSeconds(2);
        spawnerController.SetActive(true);
        loadingScreen.Remove();
        mainMenuScreen.StartGame();
        livesDisplay.SetActive(true);
        scoreDisplay.SetActive(true);
        staminaSlider.SetActive(true);
        powerup.SetActive(true);
        powerupStatus.SetActive(true);

        player.SetActive(true);
        menuCam.SetActive(false);
        backgroundMusicSource.SetActive(true);

    }

}
