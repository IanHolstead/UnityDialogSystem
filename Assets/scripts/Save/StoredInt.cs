using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredInt : StoredType
{
    private int storedInt;

    public StoredInt(int intToStore)
    {
        storedInt = intToStore;
    }

    public int Int
    {
        get
        {
            return storedInt;
        }

        set
        {
            storedInt = value;
        }
    }
}
