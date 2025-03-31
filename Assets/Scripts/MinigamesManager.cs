using System.Collections.Generic;
using UnityEngine;

public class MinigamesManager : MonoBehaviour
{
    public static MinigamesManager Instance;
    public List<string> minigamesList;
    public GameObject calibrationPanel;
    public GameObject mathPanel;
    public GameObject clickerPanel;
    public PlayerMovement playerMovement;
    public MalfunctionManager.Malfunction currentMalfunction;

    void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void SetMalfunction(MalfunctionManager.Malfunction malfunction) {
        currentMalfunction = malfunction;
    }

    public void StartRandomGame() {
        string minigame = minigamesList[Random.Range(0, minigamesList.Count)];
        StartGame(minigame);
    }

    public void StartGame(string game) {
        playerMovement.canMove = false;
        if (game == "Calibration") {
            calibrationPanel.GetComponent<CalibrationMinigame>().Start();
            calibrationPanel.SetActive(true);
        } else if (game == "Math") {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            mathPanel.GetComponent<MathMinigame>().Start();
            mathPanel.SetActive(true);
        } else if (game == "Clicker") {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            clickerPanel.GetComponent<ClickerMinigame>().Start();
            clickerPanel.SetActive(true);
        }
    }

    public void InterruptGame(string game) {
        currentMalfunction = null;
        EndGame(game);
    }

    public void EndGame(string game) {
        if (currentMalfunction != null) {
            MalfunctionManager.Instance.DeactivateMalfunction(currentMalfunction);
        }
        playerMovement.canMove = true;
        if (game == "Calibration") {
            calibrationPanel.SetActive(false);
        } else if (game == "Math") {
            mathPanel.SetActive(false);
        } else if (game == "Clicker") {
            clickerPanel.SetActive(false);
        }
        MalfunctionManager.Instance.alreadyFixing = false;
        Cursor.visible = false;
    }
}