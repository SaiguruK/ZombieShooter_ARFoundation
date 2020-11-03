using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float waitTime = 1f;

    public UnityEvent setupLevelEvent; //when game is launched
    public UnityEvent startLevelEvent;
    public UnityEvent gameOverEvent;
    public UnityEvent restartLevelEvent;

    bool isSetupReady;
    bool hasLevelStarted;
    bool isGameOver;
    bool isLevelRestarted;

    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartLevel());
        yield return StartCoroutine(GameLevel());
        yield return StartCoroutine(RestartLevel());
    }

    IEnumerator StartLevel()
    {
        if (setupLevelEvent != null)
            setupLevelEvent.Invoke();

        while (!isSetupReady)
            yield return null; // wait for 1 frame until set up is ready

        if (startLevelEvent != null)
            startLevelEvent.Invoke();
    }

    IEnumerator GameLevel()
    {
        yield return new WaitForSeconds(waitTime);

        while (!isGameOver)
        {
            yield return null;
        }

        if (gameOverEvent != null)
            gameOverEvent.Invoke();
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(waitTime);

        while (!isLevelRestarted)
        {
            yield return null;
        }

        if (restartLevelEvent != null)
            restartLevelEvent.Invoke();

        ReloadScene();
    }

    public void StartGame()
    {
        isSetupReady = true;
    }

    public void RestartGame()
    {
        isLevelRestarted = true;
    }

    void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
