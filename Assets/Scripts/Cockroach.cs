using UnityEngine;

public class Cockroach : Malfunctionable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Broken() {
        transform.position = Vector3.MoveTowards(transform.position, startPosition + Vector3.up * 3f, 5f * Time.deltaTime);
    }

    public override void Fixed()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition, 5f * Time.deltaTime);   
    }


}
