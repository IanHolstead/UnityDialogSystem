﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BranchTestBaseClass : ScriptableObject {
    public string keyName;
    public abstract bool DoTest();
}
