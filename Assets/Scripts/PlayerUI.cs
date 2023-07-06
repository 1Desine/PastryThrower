using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Player player;



    private void Start() {
        player.OnScoreChanged += Player_OnScoreChanged;
    }

    private void Player_OnScoreChanged(object sender, Player.OnScoreChangedEventArgs e) {
        UpdateScore(e.score);
    }

    private void Update() {
        UpdateTimer();
    }


    private void UpdateTimer() {
        timeText.text = string.Format("{00:00}:{1:00}", Time.time / 60, Time.time % 60);
    }
    private void UpdateScore(int score) {
        scoreText.text = score.ToString();
    }

}
