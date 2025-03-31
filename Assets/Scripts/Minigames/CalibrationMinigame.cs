using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CalibrationMinigame : MonoBehaviour
{
    public Slider calibrationSlider;
    public TextMeshProUGUI calibrationText;
    public float targetZoneStart;
    public float targetZoneEnd;
    private float sliderSpeed;

    public bool isPlaying;

    public void Start() {
        sliderSpeed = 1f;
        calibrationText.text = "Откалибруйте значение.";
        isPlaying = true;
        calibrationSlider.value = 0.5f;
        targetZoneStart = Random.Range(0.05f, 0.95f);
        targetZoneEnd = targetZoneStart + 0.1f;
    }

    void Update() {
        if (!isPlaying) return;

        float move = Input.GetAxis("Horizontal") * sliderSpeed * Time.deltaTime;
        calibrationSlider.value = Mathf.Clamp(calibrationSlider.value + move, 0, 1);

        if (Input.GetKeyDown(KeyCode.E) && Input.GetKeyDown(KeyCode.Return)) {
            CheckCalibration();
        }
    }

    void CheckCalibration() {
        if (calibrationSlider.value >= targetZoneStart && calibrationSlider.value <= targetZoneEnd) {
            calibrationText.text = "Успешно!";
            StartCoroutine(CloseGame());
        } else {
            if (calibrationSlider.value > targetZoneEnd) {
                calibrationText.text = "Левее.";
            } else if (calibrationSlider.value < targetZoneStart) {
                calibrationText.text = "Правее.";
            }
            StartCoroutine(BlockMovement());
        }
    }

    IEnumerator BlockMovement() {
        float tmpSpeed = sliderSpeed;
        sliderSpeed = 0f;
        yield return new WaitForSeconds(0.5f);
        sliderSpeed = tmpSpeed;
        calibrationText.text = "Попробуйте еще раз.";
    }

    IEnumerator CloseGame() {
        yield return new WaitForSeconds(1f);
        MinigamesManager.Instance.EndGame("Calibration");
    }
}