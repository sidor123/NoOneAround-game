using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MalfunctionManager : MonoBehaviour
{
    public static MalfunctionManager Instance;

    [System.Serializable]
    public class Malfunction
    {
        public string name; // Название поломки
        public string description; // Описание поломки
        public GameObject targetObject; // Объект, который ломается
        public ParticleSystem visualEffect; // Эффект (например, искры, дым)
        public bool isActive; // Флаг активности
    }

    public List<Malfunction> malfunctions; // Список всех возможных поломок
    public TextMeshProUGUI laptopText; // Текст для отображения поломок в ноутбуке
    public TextMeshProUGUI progressText; // Текст для отображения поломок в ноутбуке

    public PlayerMovement playerController;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GenerateRandomMalfunctions());
    }

    // Генерация случайных поломок
    private IEnumerator GenerateRandomMalfunctions()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 20f)); // Интервал между поломками

            Malfunction malfunction = GetRandomInactiveMalfunction();
            if (malfunction != null)
            {
                ActivateMalfunction(malfunction);
            }
        }
    }

    // Выбор случайной неактивной поломки
    private Malfunction GetRandomInactiveMalfunction()
    {
        List<Malfunction> inactiveMalfunctions = malfunctions.FindAll(m => !m.isActive);
        if (inactiveMalfunctions.Count > 0)
        {
            return inactiveMalfunctions[Random.Range(0, inactiveMalfunctions.Count)];
        }
        return null;
    }

    // Активация поломки
    private void ActivateMalfunction(Malfunction malfunction)
    {
        malfunction.isActive = true;
        ObjectBreak(malfunction);
        if (malfunction.visualEffect != null)
        {
            malfunction.visualEffect.Play();
        }
    }

    // Деактивация поломки
    public void DeactivateMalfunction(Malfunction malfunction)
    {
        malfunction.isActive = false;
        ObjectFix(malfunction);
        if (malfunction.visualEffect != null)
        {
            malfunction.visualEffect.Stop();
        }
    }

    // Обновление текста в ноутбуке
    public void UpdateLaptopText()
    {
        laptopText.text = "Текущие поломки:\n";
        List<Malfunction> activeMalfunctions = malfunctions.FindAll(m => m.isActive);
        if (activeMalfunctions.Count == 0) {
            laptopText.text = "Нет активных поломок.\n";
        } else {
            laptopText.text = "Текущие поломки:\n";
            foreach (var malfunction in activeMalfunctions) {
                laptopText.text += $"- {malfunction.description}\n";
            }
        }
    }

    public void UpdateProgressText(string message) {
        progressText.text = message;
    }

    private IEnumerator FixingObject(Malfunction malfunction)
    {
        playerController.SetCanMove(false); // Блокируем управление

        for (int i = 0; i <= 100; i += 5)
        {
            UpdateProgressText($"{malfunction.description}: {i}%");
            yield return new WaitForSeconds(0.1f); // Ждём 0.1 секунды между шагами
        }

        UpdateProgressText(""); // Скрываем прогресс
        playerController.SetCanMove(true); // Разблокируем управление
        DeactivateMalfunction(malfunction);
    }

    private void ObjectBreak(Malfunction malfunction) {
        Malfunctionable objectMalf = malfunction.targetObject.GetComponent<Malfunctionable>();
        objectMalf.Broken();
    }

    private void ObjectFix(Malfunction malfunction) {
        Malfunctionable objectMalf = malfunction.targetObject.GetComponent<Malfunctionable>();
        objectMalf.Fixed();
    }

    public void OnInteract(string objectName) {
        List<Malfunction> activeMalfunctions = malfunctions.FindAll(m => m.isActive);
        for (int i = 0; i < activeMalfunctions.Count; ++i) {
            Malfunction currentMalfunction = activeMalfunctions[i];
            if (currentMalfunction.name == objectName + "Break") {
                StartCoroutine(FixingObject(currentMalfunction));
                return;
            }
        }
        return;
    }
}
