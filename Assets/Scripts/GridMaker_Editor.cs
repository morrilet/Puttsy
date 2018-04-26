#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridMaker))]
public class GridMaker_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridMaker gm = (GridMaker)target;
        if(GUILayout.Button("Make Grid"))
        {
            gm.MakeGrid();
        }
    }
}
#endif
