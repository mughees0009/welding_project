using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : Inventory
{
    private static PlayerInventory instance;
    public static PlayerInventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<PlayerInventory>();
            }
            return instance;
        }
    }
}
