using System.Collections.Generic;
using UnityEngine;

public class SecondaryStorage : Inventory
{
    private static SecondaryStorage instance;
    public static SecondaryStorage Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SecondaryStorage>();
            }
            return instance;
        }
    }
}

