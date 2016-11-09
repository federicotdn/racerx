﻿using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on: 
 *      https://docs.unity3d.com/540/Documentation/Manual/WheelColliderTutorial.html 
 */
public class ScoreController : MonoBehaviour {

    int scoresAmount = 0;
    private Dictionary<String, Score> scores;

    private static ScoreController instance;

    public static ScoreController Instance {
        get {
            return instance;
        }
    }

    void Awake() {
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        scores = new Dictionary<String, Score>();
        scoresAmount = PlayerPrefs.GetInt("scoresAmount");
        for (int i = 0; i < scoresAmount; i++) {
            Score score = new Score();
            score.LevelSeed = PlayerPrefs.GetInt("seed" + i);
            score.LevelDifficulty = PlayerPrefs.GetFloat("difficulty" + i);
            score.Checkpoints = PlayerPrefs.GetInt("checkpoints" + i);
            score.Time = PlayerPrefs.GetFloat("time" + i);
            scores[score.LevelSeed + "-" + score.LevelDifficulty] = score;
        }
    }

    public Score GetScore(int seed, float difficulty) {
        if (scores.ContainsKey(seed + "-" + difficulty)) {
            return scores[seed + "-" + difficulty];
        }
        return null;
    }

    public void SetScore(int seed, float difficulty, int checkpoints, float time) {
        Score score = null;
        bool scoreExists = false;
        if (scores.ContainsKey(seed + "-" + difficulty)) {
            score = scores[seed + "-" + difficulty];
            scoreExists = true;
        }
        if (!scoreExists || checkpoints > score.Checkpoints || (checkpoints == score.Checkpoints && time < score.Time)) {
            Score newScore = new Score();
            newScore.LevelSeed = seed;
            newScore.LevelDifficulty = difficulty;
            newScore.Checkpoints = checkpoints;
            newScore.Time = time;
            scores[newScore.LevelSeed + "-" + newScore.LevelDifficulty] = score;
            if (!scoreExists) {
                newScore.Index = scoresAmount;
                scoresAmount++;
            } else {
                newScore.Index = score.Index;
            }
            PlayerPrefs.SetInt("seed" + newScore.Index, newScore.LevelSeed);
            PlayerPrefs.SetFloat("difficulty" + newScore.Index, newScore.LevelDifficulty);
            PlayerPrefs.SetInt("checkpoints" + newScore.Index, newScore.Checkpoints);
            PlayerPrefs.SetFloat("time" + newScore.Index, newScore.Time);
        }
    }
}