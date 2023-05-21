using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int currentLevel = 0;

    public Level[] levelList;

    public static LevelLoader instance;

    void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Persist
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextLevel()
    {
        // Stop Current Level
        if (levelList[currentLevel].playBackgroundMusic)
        {
            FindObjectOfType<AudioManager>().Stop(levelList[currentLevel].backgroundMusicName);
        }


        // Cycle through Levels
        LoadByIndex((currentLevel + 1) % levelList.Length);
    }

    public void LoadByName(string levelName)
    {
        int level = -1;
        for (int i = 0; i < levelList.Length; ++i)
        {
            if (levelList[i].sceneName == levelName)
            {
                level = i;
                break;
            }
        }

        if (level < 0)
        {
            Debug.LogWarning("No Level Found. Name Given: " + levelName);
            return;
        }

        LoadByIndex(level);
    }

    private void LoadByIndex(int levelIndex)
    {
        currentLevel = levelIndex;

        if (levelList[currentLevel].playBackgroundMusic)
        {
            FindObjectOfType<AudioManager>().Play(levelList[currentLevel].backgroundMusicName);
        }

        SceneManager.LoadScene(levelList[currentLevel].sceneName);
    }
}
