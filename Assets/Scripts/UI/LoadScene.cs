using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected string sceneName;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.Defaults();
        GameManager.Instance.ToDungeon(sceneName);
    }
}
