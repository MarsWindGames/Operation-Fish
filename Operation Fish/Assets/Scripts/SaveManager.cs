using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;
    public int selectedLevelIndex = 1;

    private void Awake()
    {
        
        DontDestroyOnLoad(this);

        if (instance == null)
            instance = this;
    }

    public void SaveMapInfo(int mapIndex, int StarCount)
    {
        PlayerPrefs.SetInt(mapIndex.ToString(), StarCount);
    }

    public void SavePlayerInfo(bool levelUp)
    {
        if (levelUp)
        {
            int currentLevel = GetCurrentLevel();
            currentLevel++;
            PlayerPrefs.SetInt("currentLevel", currentLevel);
        }
    }

    public void SetLevelCount(int count)
    {
        PlayerPrefs.SetInt("levelCount", count);
    }

    public int GetCurrentLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel", 1);

        return currentLevel;
    }

    public int GetLevelCount()
    {
        int levelCount = PlayerPrefs.GetInt("levelCount");

        return levelCount;
    }


    public int GetMapStarCount(int mapIndex)
    {
        int starCount = PlayerPrefs.GetInt(mapIndex.ToString(), 0);

        return starCount;
    }

}
