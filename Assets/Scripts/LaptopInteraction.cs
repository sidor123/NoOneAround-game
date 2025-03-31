using UnityEngine;

public class LaptopInteraction : Malfunctionable
{
    private bool isLocked = false;

    private void Start()
    {
    }

    public override void Interact()
    {
        EventManager.Instance.LaptopWasInteracted = true;
        Debug.Log($"{objectName} : произошло взаимодействие.");
        if (!isLocked) {
            MalfunctionManager.Instance.UpdateLaptopInfo();
        } else {
            MalfunctionManager.Instance.OnInteract(objectName);
        }
    }

    public override void OnBreak() {
        isLocked = true;

        Light[] allLights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light light in allLights) {
            light.intensity *= 0.5f;
        }

        if (malfunctionAudioSource != null) {
            malfunctionAudioSource.Play();
        }
    }

    public override void OnFix()
    {

        if (malfunctionAudioSource != null) {
            malfunctionAudioSource.Stop();
        }

        Light[] allLights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light light in allLights) {
            light.intensity *= 2f;
        }

        isLocked = false;
    }
}
