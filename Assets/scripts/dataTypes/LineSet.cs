﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineSetData", menuName = "Dialog/LineSet", order = 2)]
public class LineSet : NextActionBaseClass {
    [HideInInspector]
    public List<Line> lines;
}
