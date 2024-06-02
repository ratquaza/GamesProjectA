using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for IPointerClickHandler

public class Exit : MonoBehaviour, IPointerClickHandler
{
    // This method is called when the object is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
        Debug.Log("Game is exiting"); // Optional: to verify the quit action in the editor
    }
}
