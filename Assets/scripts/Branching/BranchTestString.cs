using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "StringBranchTest", menuName = "Dialog/StringBranchTest", order = 8)]
public class BranchTestString : BranchTestBaseClass
{
    public string equalTo;

    public override bool DoTest()
    {
        string keyReturn = KeyStore.GetStringKeyValue(keyName);
        
        if (keyReturn == null)
        {
            return false;
        }

        return keyReturn == equalTo;
    }
}
