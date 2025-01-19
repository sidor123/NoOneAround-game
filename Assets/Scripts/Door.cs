using UnityEngine;

public class Door : MonoBehaviour
{
    public float interactionRange = 3f; // Максимальное расстояние для взаимодействия
    public float openHeight = 6f; // Высота, на которую поднимается дверь
    public float moveSpeed = 2f; // Скорость движения двери
    public float stayOpenTime = 3f; // Время, в течение которого дверь остаётся открытой
    public bool isLocked = false; // Флаг для заблокированной двери

    private Vector3 initialPosition; // Начальная позиция двери
    private Vector3 targetPosition; // Целевая позиция при открытии
    private bool isMoving = false; // Флаг движения двери
    private Transform player; // Ссылка на игрока

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * openHeight;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Игрок с тегом 'Player' не найден!");
        }
    }

    void Update()
    {
        if (player == null || isLocked) return;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            isMoving = true;
            StartCoroutine(OpenAndCloseDoor());
        }
    }

    private System.Collections.IEnumerator OpenAndCloseDoor()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(stayOpenTime);

        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }
}
