using UnityEditor;
using UnityEngine;

public class Door : Malfunctionable
{
    public float interactionRange = 3f; // Максимальное расстояние для взаимодействия
    public float openHeight = 6f; // Высота, на которую поднимается дверь
    public float moveSpeed = 2f; // Скорость движения двери
    public float stayOpenTime = 3f; // Время, в течение которого дверь остаётся открытой
    public bool isLocked = false; // Флаг для заблокированной двери

    private Vector3 initialPosition; // Начальная позиция двери
    private bool isMoving = false; // Флаг движения двери
    private Transform player; // Ссылка на игрока


    void Start()
    {
        initialPosition = transform.position;

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
            StartCoroutine(OpenAndCloseDoor(initialPosition + Vector3.up * openHeight, 3));
            isMoving = false;
        }
    }

    private System.Collections.IEnumerator OpenAndCloseDoor(Vector3 targetPosition, int delay) {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

    }

    private System.Collections.IEnumerator OpenBrokenDoor(Vector3 targetPosition) {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private System.Collections.IEnumerator CloseBrokenDoor() {
        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public override void Broken() {
        isLocked = true;
        if (malfunctionAudioSource != null) {
            malfunctionAudioSource.Play();
        }
        StartCoroutine(OpenBrokenDoor(initialPosition + Vector3.up * 2f));
    }

    public override void Fixed() {
        if (malfunctionAudioSource != null) {
            malfunctionAudioSource.Stop();
        }
        StartCoroutine(CloseBrokenDoor());
        isLocked = false;
    }
}
