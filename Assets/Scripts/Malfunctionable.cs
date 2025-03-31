using Unity.VisualScripting;
using UnityEngine;

public class Malfunctionable : MonoBehaviour
{

    public string objectName;
    public AudioSource malfunctionAudioSource; // Звук поломки

    void Start() {
    }

    public virtual void Interact() {
        Debug.Log($"{objectName} : произошло взаимодействие.");
        if (objectName == "Crate" & !EventManager.Instance.BoxesWasInteracted) {
            EventManager.Instance.BoxesWasInteracted = true;
            StartCoroutine(MalfunctionManager.Instance.Progress("Разбираем продукты"));
        } else if (objectName == "Fuel" & !EventManager.Instance.FuelWasInteracted) {
            EventManager.Instance.FuelWasInteracted = true;
            StartCoroutine(MalfunctionManager.Instance.Progress("Заливаем топливо"));
        } else if (objectName == "Oxygen" & !EventManager.Instance.OxygenWasInteracted) {
            EventManager.Instance.OxygenWasInteracted = true;
            StartCoroutine(MalfunctionManager.Instance.Progress("Проверяем трубы"));
        }
        MalfunctionManager.Instance.OnInteract(objectName);
    }

    public virtual void OnBreak() {
        if (malfunctionAudioSource != null) {
            malfunctionAudioSource.Play();
        }
        Debug.Log($"{objectName} : сломался!");
    }

    public virtual void OnFix() {
        if (malfunctionAudioSource != null) {
            malfunctionAudioSource.Stop();
        }
        Debug.Log($"{objectName} : починился!");
    }
}
