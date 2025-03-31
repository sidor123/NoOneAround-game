using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public bool LaptopWasInteracted = false;
    public bool DoorWasInteracted = false;
    public bool DoorWasFixed = false;
    public bool BoxesWasInteracted = false;
    public bool FuelWasInteracted = false;
    public bool OxygenWasInteracted = false;
    public Door storageDoor;
    public Door generatorDoor;
    public Door oxygenDoor;

    private int taskDelay = 10;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator Start() {
        if (PlayerPrefs.GetInt("Guide", 1) == 0) {
            yield return StartCoroutine(Beginning());
            yield return new WaitForSeconds(taskDelay);

            yield return StartCoroutine(FirstTask());
            yield return new WaitForSeconds(taskDelay);

            yield return StartCoroutine(SecondTask());
            yield return new WaitForSeconds(taskDelay);

            yield return StartCoroutine(ThirdTask());
            yield return new WaitForSeconds(taskDelay);
        } else {
            storageDoor.isLocked = false;
            generatorDoor.isLocked = false;
            oxygenDoor.isLocked = false;
            LaptopWasInteracted = true;
            DoorWasInteracted = true;
            DoorWasFixed = true;
            BoxesWasInteracted = true;
            FuelWasInteracted = true;
            OxygenWasInteracted = true;
        }
        Ending();
    }

    // Начало (обучение)
    IEnumerator Beginning() {
        MalfunctionManager.Instance.UpdateStage(-1);
        MalfunctionManager.Instance.StartLaptopText("Добро пожаловать на корабль!$Подойдите к бортовому компьютеру и взаимодействуйте с ним клавишей E.");

        yield return new WaitUntil(() => LaptopWasInteracted);
        yield return new WaitForSeconds(3f);
        MalfunctionManager.Instance.StartLaptopText("Бортовой компьютер выводит актуальную информацию о состоянии корабля.$Иногда на корабле случаются поломки, здесь вы сможете получить их список и описание.$Сейчас, попробуйте взаимодействовать клавишей E с дверью слева и выйти из главной комнаты.");

        yield return new WaitUntil(() => DoorWasInteracted);
        MalfunctionManager.Instance.StartLaptopText("Это коридор, он соединяет различные комнаты корабля. Осмотритесь вокруг.");
        yield return new WaitForSeconds(10f);

        MalfunctionManager.Instance.UpdateStage(0);
        MalfunctionManager.Instance.TestMalfunction();
        MalfunctionManager.Instance.UpdateStage(-1);
        yield return new WaitForSeconds(2f);

        MalfunctionManager.Instance.StartLaptopText("Обнаружена поломка главной двери.$Взаимодействуйте с дверью клавишей Е для исправления поломки.");
        yield return new WaitUntil(() => DoorWasFixed);
        yield return new WaitForSeconds(2f);
        MalfunctionManager.Instance.StartLaptopText("Ожидайте дальнейших указаний.");

    }

    // Первая задача (склад)
    IEnumerator FirstTask() {
        MalfunctionManager.Instance.StartLaptopText("Новая задача: отправьтесь на склад (дверь напротив главной) и разложите продукты (взаимодействуйте с коробками).");
        storageDoor.isLocked = false;

        yield return new WaitUntil(() => BoxesWasInteracted);
        yield return new WaitForSeconds(2f);
        MalfunctionManager.Instance.StartLaptopText("Ожидайте дальнейших указаний.");
        MalfunctionManager.Instance.UpdateStage(1);
    }

    IEnumerator SecondTask() {
        MalfunctionManager.Instance.StartLaptopText("Новая задача: сходите к генератору (поворот слева от склада) и заправьте топливо (взаимодействуйте с топливным баком).");
        generatorDoor.isLocked = false;

        yield return new WaitUntil(() => FuelWasInteracted);
        yield return new WaitForSeconds(2f);
        MalfunctionManager.Instance.StartLaptopText("Ожидайте дальнейших указаний.");
        MalfunctionManager.Instance.UpdateStage(2);
    }

    IEnumerator ThirdTask() {
        MalfunctionManager.Instance.StartLaptopText("Новая задача: посетите комнату с кислородом (справа от главной дверы) и проверьте трубы (взаимодействуйте с машиной).");
        oxygenDoor.isLocked = false;

        yield return new WaitUntil(() => OxygenWasInteracted);
        yield return new WaitForSeconds(2f);
        MalfunctionManager.Instance.StartLaptopText("Ожидайте дальнейших указаний.");
        MalfunctionManager.Instance.UpdateStage(3);
    }

    void Ending() {
        MalfunctionManager.Instance.StartLaptopText("На данный момент выполнены все задачи.$Ваша текущая миссия - следить за состоянием корабля и не позволить ему выйти из строя.");
        MalfunctionManager.Instance.UpdateStage(4);
        Timer.Instance.StartTimer(); // Запускаем таймер
    }
}
