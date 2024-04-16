using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    [SerializeField] private List<Item> toLoad = new List<Item>();
    public Dictionary<string, Item> Items { get; private set; }

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            Items = toLoad.ToDictionary((item) => item.name);
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public Dictionary<string, Item> GetItems()
    {
        return Items;
    }
}
