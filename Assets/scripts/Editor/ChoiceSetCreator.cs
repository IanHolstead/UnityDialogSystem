using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ChoiceSet))]
public class ChoiceSetCreator : Editor
{
    private ReorderableList reorderableList;

    private ChoiceSet ChoiceSet
    {
        get
        {
            return target as ChoiceSet;
        }
    }

    private void OnEnable()
    {
        reorderableList = new ReorderableList(ChoiceSet.choices, typeof(Choice), true, true, true, true);

        // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
        // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
        // which is a UnityEngine.Object
        // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

        // Add listeners to draw events
        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.drawElementCallback += DrawElement;

        reorderableList.onAddCallback += AddItem;
        reorderableList.onRemoveCallback += RemoveItem;
    }

    private void OnDisable()
    {
        // Make sure we don't get memory leaks etc.
        reorderableList.drawHeaderCallback -= DrawHeader;
        reorderableList.drawElementCallback -= DrawElement;

        reorderableList.onAddCallback -= AddItem;
        reorderableList.onRemoveCallback -= RemoveItem;
    }

    /// <summary>
    /// Draws the header of the list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Choice Set");
    }

    /// <summary>
    /// Draws one element of the list (ListItemExample)
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="active"></param>
    /// <param name="focused"></param>
    private void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        Choice item = ChoiceSet.choices[index];
        Choice newItem;
        EditorGUI.BeginChangeCheck();
        //item.boolValue = EditorGUI.Toggle(new Rect(rect.x, rect.y, 18, rect.height), item.boolValue);
        newItem = (Choice)EditorGUI.ObjectField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), item, typeof(Choice), false);
        //EditorGUI.PropertyField(rect, serializedObject.FindProperty("lineSet").GetArrayElementAtIndex(index));
        if (EditorGUI.EndChangeCheck())
        {
            ChoiceSet.choices[index] = newItem;
            Debug.Log("endCheck: " + newItem);
            EditorUtility.SetDirty(target);
        }

        // If you are using a custom PropertyDrawer, this is probably better
        // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
        // Although it is probably smart to cach the list as a private variable ;)
    }

    private void AddItem(ReorderableList list)
    {

        //Line asset = ScriptableObject.CreateInstance<Line>();

        //AssetDatabase.CreateAsset(asset, "assets/data/lines/line.asset");
        //AssetDatabase.SaveAssets();

        //LineSet.lines.Add(asset);
        ChoiceSet.choices.Add(null);

        EditorUtility.SetDirty(target);
    }

    private void RemoveItem(ReorderableList list)
    {
        ChoiceSet.choices.RemoveAt(list.index);

        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Actually draw the list in the inspector
        reorderableList.DoLayoutList();
    }
}
