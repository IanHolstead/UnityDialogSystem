using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredBool : StoredType
{

    private bool storedBool;

    public StoredBool(bool stringToStore)
    {
        storedBool = stringToStore;
    }

    public bool Bool
    {
        get
        {
            return storedBool;
        }

        set
        {
            storedBool = value;
        }
    }
}
