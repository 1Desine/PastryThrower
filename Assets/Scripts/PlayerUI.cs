using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private Player player;


    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image throwPowerImage;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI ammoMaxText;

    private const string TARGET_HIT_TRIGGER = "TargetHit";
    [SerializeField] private GameObject targetHit;
    [SerializeField] private TextMeshProUGUI targetHitText;
    private Animator targetHitAnimator;


    private void Awake() {
        targetHitAnimator = targetHit.GetComponent<Animator>();
    }


    private void Start() {
        player.OnScoreChanged += Player_OnScoreChanged;
        player.OnThrowPowerChanged += Player_OnThrowPowerChanged;
        player.OnAmmoChanged += Player_OnAmmoChanged;
        player.OnHitTarget += Player_OnHitTarget;
        SetAmmoMax();

        targetHit.SetActive(false);
    }

    private void Player_OnHitTarget(object sender, Pastry.HitTargetCallBackArgs e) {
        targetHit.SetActive(true);
        targetHitText.text =
            "hit: " + e.targetType.ToString() +
            "\ndistance: " + e.distance.ToString("F2");
        targetHitAnimator.SetTrigger(TARGET_HIT_TRIGGER);
    }

    private void Player_OnAmmoChanged(object sender, Player.OnAmmoChangedEventArgs e) {
        ammoText.text = e.ammo.ToString();
    }
    private void SetAmmoMax() {
        ammoMaxText.text = player.GetAmmoMax().ToString();
    }

    private void Player_OnThrowPowerChanged(object sender, Player.OnThrowPowerChangedEventArgs e) {
        UpdateThrowPower(e.throwPowerNormalized);
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
    private void UpdateThrowPower(float throwPower) {
        throwPowerImage.fillAmount = 1 - throwPower;
    }

}
