using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class ScriptableObjectDrawer : PropertyDrawer
{
    #region Fields

    // Cached scriptable object editor
    private Editor editor = null;

    #endregion Fields

    #region Public Methods

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Draw label
        EditorGUI.PropertyField(position, property, label, true);

        // Draw foldout arrow
        if (property.objectReferenceValue != null)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
        }

        // Draw foldout properties
        if (property.isExpanded)
        {
            // Make child fields be indented
            EditorGUI.indentLevel++;

            // Draw object properties
            if (property.objectReferenceValue != null)
            {
                CreateCachedEditor(property.objectReferenceValue, null);
                if (editor != null)
                {
                    editor.OnInspectorGUI();
                }
            }
            else
            {
                EditorGUI.LabelField(position, "No ScriptableObject assigned");
            }

            // Set indent back to what it was
            EditorGUI.indentLevel--;
        }
    }

    #endregion Public Methods

    #region Private Methods

    // Create cached editor
    private void CreateCachedEditor(Object obj, Editor previousEditor)
    {
        if (obj == null)
        {
            editor = null;
            return;
        }

        if (editor == null || previousEditor == null || editor.target != obj || previousEditor.target != obj)
        {
            Editor.DestroyImmediate(previousEditor);
            editor = Editor.CreateEditor(obj);
        }
    }

    #endregion Private Methods
}