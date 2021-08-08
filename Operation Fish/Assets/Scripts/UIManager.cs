using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Canvases")]

    public Canvas finishCanvas;
    public Canvas failCanvas;
    public Canvas mainCanvas;
    public Canvas tutorialCanvas;

    public TMP_Text finishScoreText;

    public Animator finishAnimator;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void hideMainCanvas()
    {
        mainCanvas.enabled = false;
    }

    public void showFinishCanvas()
    {
        StartCoroutine(showFinishScreen());
        hideMainCanvas();
    }

    IEnumerator showFinishScreen()
    {
        yield return new WaitForSeconds(3);
        finishCanvas.enabled = true;
        int starCount = GameManager.instance.starsGained;
        finishAnimator.SetInteger("StarCount", starCount);
        finishScoreText.text = "SCORE " + ScoreManager.instance.playerScore;
    }
    public void showFailCanvas()
    {
        failCanvas.enabled = true;
        hideMainCanvas();
    }

    public void showTutorialCanvas()
    {
        mainCanvas.enabled = false;
        tutorialCanvas.enabled = true;
    }

    public void hideTutorialCanvas()
    {
        tutorialCanvas.enabled = false;
        mainCanvas.enabled = true;
    }

    public void RestartLevel()
    {
        int curr = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadScene(curr));
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void returnMainMenu()
    {
        StartCoroutine(LoadScene(0));
    }

    public void loadNextLevel()
    {
        SaveManager saveManager = SaveManager.instance;
        MapSaver mapSaver = new MapSaver();

        if (saveManager.selectedLevelIndex < mapSaver.mapCount() - 1)
        {
            saveManager.selectedLevelIndex++;
            int curr = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadScene(curr));
        }
        else
        {
            returnMainMenu();
        }


    }
}
