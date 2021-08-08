using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenu : MonoBehaviour
{

    public GameObject textObject;
    public TMP_Text scoreText;

    private void Start()
    {
        int score = PlayerPrefs.GetInt("playerScore", 0);
        scoreText.text = "Score:" + score;
    }

    public void NextScene()
    {
        textObject.SetActive(true);
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        int curr = SceneManager.GetActiveScene().buildIndex;
        curr++;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(curr);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
