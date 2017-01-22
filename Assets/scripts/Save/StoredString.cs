using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredString : StoredType {

    private string storedString;

    public StoredString(string stringToStore)
    {
        storedString = stringToStore;
    }

    public string String
    {
        get
        {
            return storedString;
        }

        set
        {
            storedString = value;
        }
    }
}
