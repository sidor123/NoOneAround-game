using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public TextMeshProUGUI timerText;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        timerText.text = $"Personal Record: {string.Format("{0:N2}", PlayerPrefs.GetFloat("Time", 0))} seconds";
    }

    public void StartGame() {
        SceneManager.LoadScene(1); // Загружает сцену с индексом 1
    }

    public void ExitGame() {
        Application.Quit(); // Закрывает игру
    }
}