#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIData), true)]
public class UIInspector : Editor
{

    #region VARIABLE

    private int _eventCount;

    #endregion


    
    #region LIFECYCLE

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        
        serializedObject.Update();
        
        EditorGUILayout.BeginVertical();
        
        _eventCount = EditorGUILayout.IntField(_eventCount);
        if (GUILayout.Button("Create"))
        {
            // Index crate
            Create();
        }
        
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(20f);
        base.OnInspectorGUI();
        
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);    
        }
    }

    #endregion



    #region FUNCTION

    private void Create()
    {
        var data = (UIData)target;
        if (data is null)
            return;

        data.table ??= new List<UITable>();
        data.table.Clear();

        for (var i = 0; i < _eventCount; i++)
        {
            data.table.Add(new UITable{ text = $"{i}" });
        }
        
        EditorUtility.SetDirty(data);
    }

    #endregion
    
}

# endif