using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "choiceData", menuName = "Dialog/Choice", order = 3)]
public class Choice : NextActionBaseClass
{

    [Tooltip("Option text")]
    public string shortText;

    [Header("Control related")]
    [Tooltip("Not working yet")]
    public bool enabled;
}
