using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetStringKey", menuName = "Dialog/SetStringKey", order = 11)]
public class SetStringKey : SetKeyBaseClass {
    public string valueToSet;
    public override void Set()
    {
        KeyStore.UpdateKey(keyName, valueToSet);
    }
}
