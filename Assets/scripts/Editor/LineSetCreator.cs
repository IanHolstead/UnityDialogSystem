using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(LineSet))]
public class LineSetCreator : Editor
{
    private ReorderableList reorderableList;

    private LineSet LineSet
    {
        get
        {
            return target as LineSet;
        }
    }

    private void OnEnable()
    {
        //LineSet lineset = (LineSet)target;
        //Debug.Log("OnEnable: " + target);
        if (LineSet == null)
        {
            //Debug.Log("is Null!");
            return;
        }
        if (reorderableList != null)
        {
            Debug.Log("Already Made!");
            return;
        }

        reorderableList = new ReorderableList(LineSet.lines, typeof(Line), true, true, true, true);

        Debug.Log("Reorderable List Created: " + reorderableList + ", lineset: "+ target);
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
        if (LineSet == null)
        {
            return;
        }
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
        GUI.Label(rect, "Line Set");
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
        Line item = LineSet.lines[index];
        Line newItem;
        EditorGUI.BeginChangeCheck();
        //item.boolValue = EditorGUI.Toggle(new Rect(rect.x, rect.y, 18, rect.height), item.boolValue);
        newItem = (Line)EditorGUI.ObjectField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), item, typeof(Line), false);
        //EditorGUI.PropertyField(rect, serializedObject.FindProperty("lineSet").GetArrayElementAtIndex(index));
        if (EditorGUI.EndChangeCheck())
        {
            LineSet.lines[index] = newItem;
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
        LineSet.lines.Add(null);

        EditorUtility.SetDirty(target);
    }

    private void RemoveItem(ReorderableList list)
    {
        LineSet.lines.RemoveAt(list.index);

        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Actually draw the list in the inspector
        if (reorderableList != null)
        {
            try
            {
                Debug.Log(reorderableList.count);
            }
            catch( System.Exception e)
            {
                //Gotta catch 'em all
                Debug.Log(e);
            }
            reorderableList.DoLayoutList();
        }
    }
}
