using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public TextMeshProUGUI taskText;
    public Door generatorDoor; // Ссылка на дверь в генераторную

    private bool boxTaskCompleted;
    private bool generatorTaskCompleted;

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
        StartBoxTask();
    }

    private void StartBoxTask()
    {
        boxTaskCompleted = false;
        UpdateTaskText("New task: Go to the Box and return to the Laptop.");
    }

    private void StartGeneratorTask()
    {
        generatorTaskCompleted = false;
        UpdateTaskText("New task: Go to the Generator and fix it.");
        generatorDoor.UnlockDoor(); // Разблокируем дверь в генераторную
    }

    public void OnInteract(string objectName)
    {
        if (objectName == "Box" && !boxTaskCompleted)
        {
            boxTaskCompleted = true;
            UpdateTaskText("Box interacted. Now return to the Laptop.");
        }
        else if (objectName == "Laptop" && boxTaskCompleted && !generatorTaskCompleted)
        {
            StartGeneratorTask();
        }
        else if (objectName == "Generator" && !generatorTaskCompleted)
        {
            generatorTaskCompleted = true;
            UpdateTaskText("Generator fixed. All tasks completed!");
        }
    }

    private void UpdateTaskText(string message)
    {
        taskText.text = message;
    }
}
