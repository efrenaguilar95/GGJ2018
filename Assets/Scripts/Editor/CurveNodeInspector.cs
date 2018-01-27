using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

//Made w/ help of tutorial http://catlikecoding.com/unity/tutorials/curves-and-splines/
[CustomEditor(typeof(CurveNode))]
public class CurveNodeInspector : Editor {
    private void OnSceneGUI()
    {
        //Data
        CurveNode node = target as CurveNode;
        Transform handleTransform = node.transform;
        Quaternion handleRotation = (Tools.pivotRotation == PivotRotation.Local ? 
            node.transform.rotation : Quaternion.identity);
        Vector2 p0 = handleTransform.TransformPoint(node.inTangent); //inTangent
        Vector2 p1 = handleTransform.TransformPoint(node.outTangent); //outTangent
        //Rendering
        Handles.color = Color.white;
        Handles.DrawLine(p0, node.transform.position);
        Handles.DrawLine(node.transform.position, p1);

        //Interactable GUI
        //Update Event Handling?
        //AG: This not being a hookable event feels wrong
        EditorGUI.BeginChangeCheck();
        Vector2 hp0 = Handles.DoPositionHandle(p0, handleRotation);
        Vector2 hp1 = Handles.DoPositionHandle(p1, handleRotation);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(node, "Moved Tangent");
            EditorUtility.SetDirty(node);
            node.inTangent = handleTransform.InverseTransformPoint(hp0);
            node.outTangent = handleTransform.InverseTransformPoint(hp1);
        }
    }
}

#endif