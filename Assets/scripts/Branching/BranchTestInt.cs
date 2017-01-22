using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntBranchTest", menuName = "Dialog/IntBranchTest", order = 6)]
public class BranchTestInt : BranchTestBaseClass {
    public CompareOperation compareOperation;
    public int valueToCompare;

    public enum CompareOperation
    {
        greaterThan,
        greaterThanOrEqual,
        equal,
        LessThanOrEqual,
        LessThan
    }

	public override bool DoTest()
    {
        int? keyReturn = KeyStore.GetIntKeyValue(keyName);
        int keyValue;
        if (keyReturn == null)
        {
            return false;
        }

        keyValue = keyReturn.Value;
        switch (compareOperation)
        {
            case CompareOperation.greaterThan:
                return keyValue > valueToCompare;
            case CompareOperation.greaterThanOrEqual:
                return keyValue >= valueToCompare;
            case CompareOperation.equal:
                return keyValue == valueToCompare;
            case CompareOperation.LessThanOrEqual:
                return keyValue <= valueToCompare;
            case CompareOperation.LessThan:
                return keyValue < valueToCompare;
        }
        return false;
    }
}
