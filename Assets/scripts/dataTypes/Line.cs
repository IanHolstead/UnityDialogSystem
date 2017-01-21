using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineData", menuName = "Dialog/Line", order = 1)]
public class Line : ScriptableObject
{
    
    [Tooltip("Line of text that the character will say")]
    [Multiline]
    public string line;

    [Header("Character")]
    public string speakerName;
    [Tooltip("Point this to the character model")]
    public GameObject speaker;
    public string animationToPlay;

    [Header("Timing related")]
    [Tooltip("Not working yet")]
    public float delayBeforePlay;
    [Tooltip("Not working yet")]
    public bool skipable;
}
