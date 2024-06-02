using UnityEngine;

public class VoidTile : MonoBehaviour
{
    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            previousPosition = other.transform.position;
        }
    }

    public Vector3 GetPreviousLocation()
    {
        return previousPosition;
    }

}
