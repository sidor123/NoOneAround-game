using Unity.VisualScripting;
using UnityEngine;

public class Malfunctionable : MonoBehaviour
{

    public string objectName;
    public AudioSource malfunctionAudioSource; // Звук поломки

    void Start() {
    }

    public virtual void Interact()
    {
        Debug.Log($"{objectName} : произошло взаимодействие.");
        MalfunctionManager.Instance.OnInteract(objectName);
    }

    public virtual void Broken() {
        if (malfunctionAudioSource != null) {
            malfunctionAudioSource.Play();
        }
        Debug.Log($"{objectName} : сломался!");
    }

    public virtual void Fixed() {
        if (malfunctionAudioSource != null) {
            malfunctionAudioSource.Stop();
        }
        Debug.Log($"{objectName} : починился!");
    }
}
