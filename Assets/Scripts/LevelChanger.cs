﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");
    private AsyncOperation operation;


    public Animator Animator;

    private int levelToLoad;
    public static LevelChanger Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void FadeToLevel(int levelIndex)
    {
        PlayerPrefs.DeleteAll();
        operation = SceneManager.LoadSceneAsync(levelIndex);
        operation.allowSceneActivation = false;
        Animator.SetTrigger(FadeOut);
        levelToLoad = levelIndex;
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete()
    {
        operation.allowSceneActivation = true;
    }
}