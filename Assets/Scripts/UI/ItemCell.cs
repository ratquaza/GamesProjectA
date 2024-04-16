using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCell : MonoBehaviour
{
    private int itemIndex;

    public int GetItemIndex()
    {
        return itemIndex;
    }

    public void SetItemIndex(int itemIndex)
    {
        this.itemIndex = itemIndex;
    }
}
