using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChoiceSetData", menuName = "Dialog/ChoiceSet", order = 4)]
public class ChoiceSet : ActionBaseClass {

    public Line lineToPlay;
    [HideInInspector]
    public List<Choice> choices;
}
