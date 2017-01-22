using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetBoolKey", menuName = "Dialog/SetBoolKey", order = 10)]
public class SetBoolKey : SetKeyBaseClass
{
    public bool valueToSet;
    public override void Set()
    {
        KeyStore.UpdateKey(keyName, valueToSet);
    }
}
