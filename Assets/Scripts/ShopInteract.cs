using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopInteract : MonoBehaviour
{
    private PlayerActions playerActions;
    private InputAction interactInput;


    [SerializeField] private GameObject ShopUIButtons;
    private bool inTrigger;

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }

    private void Awake()
    {
        playerActions = new PlayerActions();
        interactInput = playerActions.Inventory.Interact;
        interactInput.performed += ctx => ToggleShopUI();   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            inTrigger = true;    
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            ShopUIButtons.SetActive(false);
            inTrigger = false;
        }

    }

    private void ToggleShopUI()
    {
        if (inTrigger && ShopUIButtons.activeSelf == true)
        {
            Debug.Log("test");
            ShopUIButtons.SetActive(false);
        }
        else if (inTrigger)
        {
            Debug.Log("test2");
            ShopUIButtons.SetActive(true);
        }
    }
}
