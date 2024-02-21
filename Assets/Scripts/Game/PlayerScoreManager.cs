using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerStats
{
    public float playedTime;
    public int GetMedalIndex() => PlayerScoreManager.GetMedalByTime(playedTime);
}

public class PlayerScoreManager : MonoBehaviour
{
    private static PlayerScoreManager instance;
    [SerializeField] private GameManager gameManager;

    [System.Serializable]
    private struct PlayerMedal
    {
        public string medalName;
        public Sprite medalSprite;

        public float medalEarningTime;
    }

    [SerializeField] private Hourglass hourglass;
    [SerializeField] private PlayerMedal[] playerMedals = new PlayerMedal[0];
    private int currentMedal;

    private float playTime;
    private bool updatePlayTime;

    [SerializeField] private GameObject resumeGameButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (updatePlayTime) playTime += Time.deltaTime;
    }

    public static void StartGame()
    {
        instance.currentMedal = 0;

        instance.playTime = 0;
        instance.updatePlayTime = true;

        instance.UpdateMedal(0);
    }

    public void SetResumeButton() => resumeGameButton.SetActive(gameManager.IsGameStarted());

    public static void PauseGame()
    {
        instance.updatePlayTime = false;
        instance.hourglass.StopTimer();
    }

    public static void ResumeGame()
    {
        instance.updatePlayTime = true;
        instance.hourglass.ResumeTimer();
    }

    public static PlayerStats EndGame()
    {
        instance.updatePlayTime = false;

        PlayerStats playerStats = new PlayerStats()
        {
            playedTime = instance.playTime,
        };

        instance.hourglass.StopTimer();

        return playerStats;
    }

    private void UpdateMedal(int medalIndex = -1)
    {
        if (medalIndex >= 0)
            currentMedal = medalIndex;
        else
            currentMedal++;

        if (currentMedal >= playerMedals.Length)
        {
            currentMedal = playerMedals.Length - 1;
            return;
        }

        hourglass.StartTimer(playerMedals[currentMedal].medalEarningTime, () => UpdateMedal());
    }

    public static int GetMedalByTime(float time)
    {
        int medal;
        for (medal = 0; medal < instance.playerMedals.Length; medal++)
        {
            if (time < instance.playerMedals[medal].medalEarningTime)
                break;

            time -= instance.playerMedals[medal].medalEarningTime;
        }

        return medal;
    }
}