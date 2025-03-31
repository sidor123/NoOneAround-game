using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MathMinigame : MonoBehaviour
{
    public TextMeshProUGUI mathText;
    public TMP_InputField answerInput;
    private int num1, num2, correctAnswer;
    private char operation;

    public bool isPlaying;

    public void Start() {
        GenerateExample();
        isPlaying = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            CheckAnswer();
        }
    }

    void GenerateExample() {
        int operationType = Random.Range(0, 4); // 0 - +, 1 - -, 2 - *, 3 - /
        num1 = Random.Range(1, 20);
        num2 = Random.Range(1, 20);
        
        switch (operationType) {
            case 0:
                operation = '+';
                correctAnswer = num1 + num2;
                break;
            case 1:
                operation = '-';
                if (num1 < num2) (num1, num2) = (num2, num1); // избегаем отрицательных ответов
                correctAnswer = num1 - num2;
                break;
            case 2:
                operation = '*';
                correctAnswer = num1 * num2;
                break;
            case 3:
                operation = '/';
                num1 = num2 * Random.Range(1, 10); // гарантируем деление нацело
                correctAnswer = num1 / num2;
                break;
        }
        
        mathText.text = $"{num1} {operation} {num2} = ?";
        answerInput.text = "";
    }

    public void CheckAnswer() {
        if (!isPlaying) return;

        if (int.TryParse(answerInput.text, out int playerAnswer) && playerAnswer == correctAnswer) {
            mathText.text = "Верно!";
            StartCoroutine(CloseGame());
        } else {
            mathText.text = "Неверно. Попробуйте снова.";
            StartCoroutine(BlockMovement());
        }
    }

    IEnumerator BlockMovement() {
        answerInput.interactable = false;
        yield return new WaitForSeconds(0.5f);
        answerInput.interactable = true;
        GenerateExample();
    }

    IEnumerator CloseGame() {
        yield return new WaitForSeconds(1f);
        MinigamesManager.Instance.EndGame("Math");
    }
}