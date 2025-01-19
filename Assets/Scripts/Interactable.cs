using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string objectName;

    public void Interact()
    {
        Debug.Log($"{objectName} interacted with.");
        TaskManager.Instance.OnInteract(objectName);
    }
}
