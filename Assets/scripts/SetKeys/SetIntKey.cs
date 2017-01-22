using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetIntKey", menuName = "Dialog/SetIntKey", order = 9)]
public class SetIntKey : SetKeyBaseClass
{
    public int valueToSet;
    public override void Set()
    {
        Debug.Log("update" + keyName + valueToSet);
        KeyStore.UpdateKey(keyName, valueToSet);
    }

    public void Set(int value)
    {
        KeyStore.UpdateKey(keyName, value);
    }
}
