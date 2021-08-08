using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{

    public GameObject buttonPrefab;
    public GameObject lockedPrefab;
    public GameObject parent;
    public RectTransform arrow;


    Vector2 startPoint;
    float buttonWidth;
    float buttonHeight;

    RectTransform[] buttons;


    int levelCount;
    int currentLevel;
    int selectedLevel = 1;

    private void Start()
    {
        RectTransform buttonRectTransform = buttonPrefab.GetComponent<RectTransform>();
        buttonWidth = buttonRectTransform.sizeDelta.x;
        startPoint = Vector2.zero;

        levelCount = getLevelCount();
        currentLevel = SaveManager.instance.GetCurrentLevel();
        buttons = new RectTransform[levelCount];

        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int i = 1; i <= levelCount; i++)
        {
            GameObject button;
            if (i > currentLevel)
            {
                button = Instantiate(lockedPrefab, Vector3.zero, Quaternion.identity, parent.transform);
                button.name = "-1"; // -1 means this level is locked. We won't start the game if the selected button's name is -1.
            }
            else
            {
                button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, parent.transform);
                button.name = i.ToString();
            }
            if (button.name == "-1" && i == 16)
            {
                button.name = "SOON...";
                button.SetActive(false);
                //there may be new levels.

            }
            buttons[i - 1] = button.GetComponent<RectTransform>();
            buttons[i - 1].GetComponentInChildren<TextMeshProUGUI>().text = i + "," + getLevelStarCount(i).ToString();
        }
    }

    public void chooseLevel(string levelId)
    {
        selectedLevel = int.Parse(levelId);
    }

    public void startButton()
    {
        if (selectedLevel != -1) // if level isn't locked.
        {
            SaveManager.instance.selectedLevelIndex = selectedLevel;
            StartCoroutine(LoadNextScene());
        }
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

    int getLevelCount()
    {
        MapSaver mapSaver = new MapSaver();

        int levelCount = mapSaver.mapCount();
        return levelCount;
    }

    int getLevelStarCount(int levelIndex)
    {
        int starCount = SaveManager.instance.GetMapStarCount(levelIndex);
        return starCount;
    }
}
