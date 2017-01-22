using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyStore {

    public enum returnValue
    {
        success,
        wrongType,
        notExisting
    }

    private static Dictionary<string, StoredType> keyStore = new Dictionary<string, StoredType>();

    public static returnValue UpdateKey(string keyName, int value, bool createIfNotExisting = true)
    {
        if (keyStore.ContainsKey(keyName))
        {
            if (keyStore[keyName] is StoredInt)
            {
                ((StoredInt)keyStore[keyName]).Int = value;
                return returnValue.success;
            }
            else
            {
                return returnValue.wrongType;
            }
        }
        else if (createIfNotExisting)
        {
            keyStore.Add(keyName, new StoredInt(value));
            return returnValue.success;
        }
        return returnValue.notExisting;
    }

    public static returnValue UpdateKey(string keyName, string value, bool createIfNotExisting = true)
    {
        if (keyStore.ContainsKey(keyName))
        {
            if (keyStore[keyName] is StoredString)
            {
                ((StoredString)keyStore[keyName]).String = value;
                return returnValue.success;
            }
            else
            {
                return returnValue.wrongType;
            }
        }
        else if (createIfNotExisting)
        {
            keyStore.Add(keyName, new StoredString(value));
            return returnValue.success;
        }
        return returnValue.notExisting;
    }

    public static returnValue UpdateKey(string keyName, bool value, bool createIfNotExisting = true)
    {
        if (keyStore.ContainsKey(keyName))
        {
            if (keyStore[keyName] is StoredBool)
            {
                ((StoredBool)keyStore[keyName]).Bool = value;
                return returnValue.success;
            }
            else
            {
                return returnValue.wrongType;
            }
        }
        else if (createIfNotExisting)
        {
            keyStore.Add(keyName, new StoredBool(value));
            return returnValue.success;
        }
        return returnValue.wrongType;
    }

    public static bool? GetBoolKeyValue(string keyName)
    {
        if (keyStore.ContainsKey(keyName))
        {
            if (keyStore[keyName] is StoredBool)
            {
                return ((StoredBool)keyStore[keyName]).Bool;
            }
        }
        return null;
    }

    public static string GetStringKeyValue(string keyName)
    {
        if (keyStore.ContainsKey(keyName))
        {
            if (keyStore[keyName] is StoredString)
            {
                return ((StoredString)keyStore[keyName]).String;
            }
        }
        return null;
    }

    public static int? GetIntKeyValue(string keyName)
    {
        if (keyStore.ContainsKey(keyName))
        {
            if (keyStore[keyName] is StoredInt)
            {
                return ((StoredInt)keyStore[keyName]).Int;
            }
        }
        return null;
    }
}
