using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MalfunctionManager : MonoBehaviour
{
    public static MalfunctionManager Instance;

    [System.Serializable]
    public class Malfunction
    {
        public string name; // Название поломки
        public string description; // Описание поломки
        public GameObject targetObject; // Объект, который ломается
        public ParticleSystem visualEffect; // Эффект
        public bool isActive; // Флаг активности
        public int malfStage; // На какой стадии открывается поломка
    }

    public int currentStage;

    public List<Malfunction> malfunctions; // Список всех возможных поломок
    public TextMeshProUGUI laptopText; // Текст для отображения поломок в ноутбуке
    public TextMeshProUGUI progressText; // Текст для отображения процесса починки

    public PlayerMovement playerController;
    private AudioSource audioSource;
    private bool gameEnded; // Флаг конца игры
    public bool alreadyFixing;

    // SETTINGS
    private float leftTimeInt; // Временной отрезок для возникновения поломки
    private float malfunctionAcc = 0.9f; // Множитель ускорения поломок
    private int malfsThreshold; // Крайнее число поломок
    private float textSpeed;
    private float textInterval; // Интервал между сообщениями

    // Переменная нужна для случаев, когда один вывод текста на экран прерывается другим (см. в функции SetLaptopText)
    private Coroutine typingCoroutine;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        leftTimeInt = 30f;
        malfsThreshold = 6;
        textSpeed = 0.05f;
        textInterval = 2f;
        gameEnded = false;
        alreadyFixing = false;
        audioSource = GetComponent<AudioSource>();
        SetMalfunctionRate(PlayerPrefs.GetInt("Difficulty", 0));
        StartCoroutine(GenerateRandomMalfunctions());
    }

    public float GetTimeInt() {
        return leftTimeInt;
    }

    public void SetMalfunctionRate(int difficulty) {
        if (difficulty == 0) {
            malfunctionAcc = 0.95f;
        } else if (difficulty == 1) {
            leftTimeInt = 25f;
            malfunctionAcc = 0.9f;
        } else if (difficulty == 2) {
            leftTimeInt = 20f;
            malfunctionAcc = 0.8f;
        } else {
            leftTimeInt = 15f;
            malfunctionAcc = 0.7f;
        }
    }

    public void TestMalfunction() {
        ActivateMalfunction(GetRandomInactiveMalfunction());
    }

    private bool CheckMalfunctionForBugs(Malfunction malf) {
        if (malf == null) {
            return true;
        }
        if (malf.name == "CabinDoorBreak" || malf.name == "StorageDoorBreak" || malf.name == "GeneratorDoorBreak" || malf.name == "OxygenDoorBreak") {
            if (malf.targetObject.GetComponent<Door>().isMoving) {
                return false;
            }
        }
        return true;
    }

    // Генерация случайных поломок
    public IEnumerator GenerateRandomMalfunctions() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(leftTimeInt, leftTimeInt * 2f)); // Интервал между поломками
            if (currentStage == 4) {
                leftTimeInt *= malfunctionAcc;
            }

            Malfunction malfunction = GetRandomInactiveMalfunction();
            while (!CheckMalfunctionForBugs(malfunction)) {
               malfunction = GetRandomInactiveMalfunction();
            }
            if (malfunction != null) {
                ActivateMalfunction(malfunction);
            }
            CheckForEnd();
        }
    }

    // Выбор случайной неактивной поломки
    private Malfunction GetRandomInactiveMalfunction() {
        List<Malfunction> inactiveMalfunctions = malfunctions.FindAll(m => !m.isActive).FindAll(m => m.malfStage <= currentStage);
        if (inactiveMalfunctions.Count > 0) {
            return inactiveMalfunctions[Random.Range(0, inactiveMalfunctions.Count)];
        }
        return null;
    }

    // Активация поломки
    private void ActivateMalfunction(Malfunction malfunction) {
        malfunction.isActive = true;
        InvokeBreak(malfunction);
        if (malfunction.visualEffect != null) {
            malfunction.visualEffect.Play();
        }
    }

    private void CheckForEnd() {
        if (gameEnded) {
            return;
        }
        List<Malfunction> activeMalfunctions = malfunctions.FindAll(m => m.isActive);
        if (activeMalfunctions.Count > malfsThreshold) {
            gameEnded = true;
            Timer.Instance.SaveTime();
            Timer.Instance.StopTimer();
            StartLaptopText("Корабль вышел из строя. Гибель неминуема.\n");
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame() {
        List<Malfunction> inactiveMalfunctions = malfunctions.FindAll(m => !m.isActive);
        for (int i = 0; i < inactiveMalfunctions.Count; ++i) {
            ActivateMalfunction(inactiveMalfunctions[i]);
        }
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

    // Деактивация поломки
    public void DeactivateMalfunction(Malfunction malfunction) {
        InvokeFix(malfunction);
        if (malfunction.visualEffect != null) {
            malfunction.visualEffect.Stop();
        }
        StartCoroutine(MalfunctionDelay(malfunction));
    }

    private IEnumerator MalfunctionDelay(Malfunction malfunction) {
        yield return new WaitForSeconds(5f);
        malfunction.isActive = false;
    }

    // Для множественных вызовов SetLaptopText
    public void StartLaptopText(string message) {
        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(SetLaptopText(message));
    }

    // Обновление текста в ноутбуке
    public IEnumerator SetLaptopText(string message) {
        laptopText.text = "";
        for (int i = 0; i < message.Length; ++i) {
            if (message[i] == '$') {
                yield return new WaitForSeconds(textInterval);
                laptopText.text = "";
                ++i;
            }
            laptopText.text += message[i];
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void SetProgressText(string message) {
        progressText.text = message;
    }

    public void UpdateLaptopInfo() {
        string msg;
        List<Malfunction> activeMalfunctions = malfunctions.FindAll(m => m.isActive);
        if (activeMalfunctions.Count == 0) {
            msg = "Корабль в оптимальном состоянии.\n";
        } else {
            msg = "Работа корабля нарушена. Текущие поломки:\n";
            foreach (var malfunction in activeMalfunctions) {
                msg += $"- {malfunction.description}\n";
            }
        }
        StartLaptopText(msg);
    }

    public IEnumerator Progress(string malfunction_description) {
        playerController.SetCanMove(false); // Блокируем управление

        for (int i = 0; i <= 100; i += 5) {
            SetProgressText($"{malfunction_description}: {i}%");
            yield return new WaitForSeconds(0.1f); // Ждём 0.1 секунды между шагами
        }

        SetProgressText(""); // Скрываем прогресс
        playerController.SetCanMove(true); // Разблокируем управление
    }

    private void InvokeBreak(Malfunction malfunction) {
        Malfunctionable objectMalf = malfunction.targetObject.GetComponent<Malfunctionable>();
        objectMalf.OnBreak();
    }

    private void InvokeFix(Malfunction malfunction) {
        Malfunctionable objectMalf = malfunction.targetObject.GetComponent<Malfunctionable>();
        objectMalf.OnFix();
    }

    public void OnInteract(string objectName) {
        if (gameEnded == true || alreadyFixing == true) {
            return;
        }
        List<Malfunction> activeMalfunctions = malfunctions.FindAll(m => m.isActive);
        for (int i = 0; i < activeMalfunctions.Count; ++i) {
            Malfunction currentMalfunction = activeMalfunctions[i];
            if (currentMalfunction.name == objectName + "Break") {
                alreadyFixing = true;
                MinigamesManager.Instance.SetMalfunction(currentMalfunction);
                MinigamesManager.Instance.StartRandomGame();
                return;
            }
        }
        return;
    }

    public void UpdateStage(int stage) {
        currentStage = stage;
    }    
}