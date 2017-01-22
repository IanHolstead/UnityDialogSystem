using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolBranchTest", menuName = "Dialog/BoolBranchTest", order = 7)]
public class BranchTestBool : BranchTestBaseClass
{
    public bool equalTo;

    public override bool DoTest()
    {
        bool? keyReturn = KeyStore.GetBoolKeyValue(keyName);

        if (keyReturn == null)
        {
            return false;
        }

        return keyReturn.Value == equalTo;
    }
}

