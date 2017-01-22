using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleBranch", menuName = "Dialog/SimpleBranch", order = 5)]
public class SimpleBranch : ActionBaseClass {

    public ActionBaseClass doIfTrue;
    public ActionBaseClass doIfFalse;

    public BranchTestBaseClass testToPerform;
}
