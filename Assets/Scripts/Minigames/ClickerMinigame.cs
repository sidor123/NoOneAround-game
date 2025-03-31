using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class ClickerMinigame : MonoBehaviour
{
    public TextMeshProUGUI progressText;
    public Button clickBtn;
    private int value;

    public bool isPlaying;
    public bool isDecreasing;

    public void Start() {
        isPlaying = true;
        progressText.text = "0%";
        value = 0;
        isDecreasing = false;
    }

    void Update() {
        if (!isDecreasing && isPlaying) {
            StartCoroutine(DecreaseValue());
        }
        progressText.text = $"{value}%";
        CheckFinish();
    }

    IEnumerator DecreaseValue() {
        isDecreasing = true;
        yield return new WaitForSeconds(0.1f);
        value = Math.Max(0, value - 5);
        isDecreasing = false;
    }

    public void OnClick() {
        value += 10;
    }

    void CheckFinish() {
        if (value >= 100) {
            isPlaying = false;
            progressText.text = "Успешно!";
            StartCoroutine(CloseGame());
        }
    }

    public void Interrupt() {
        MinigamesManager.Instance.InterruptGame("Clicker");
    }

    IEnumerator CloseGame() {
        yield return new WaitForSeconds(1f);
        MinigamesManager.Instance.EndGame("Clicker");
    }
}