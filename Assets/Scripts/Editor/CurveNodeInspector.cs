using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
//Made w/ help of tutorial http://catlikecoding.com/unity/tutorials/curves-and-splines/
[CustomEditor(typeof(CurveNode))]
public class CurveNodeInspector : Editor
{
    public enum SelectedCurveUI
    {
        outTangent,
        inTangent,
        unknown
    }

    private CurveNode node;
    private BezierSpline parentSpline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private SelectedCurveUI selectedUI;

    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;
    private const int lineRenderSteps = 20;


    private void OnSceneGUI()
    {
        DrawGUI(true);
    }

    public void DrawGUI(bool renderCurves)
    {
        //Data
        node = target as CurveNode;
        parentSpline = node.GetComponentInParent<BezierSpline>();
        handleTransform = node.transform;
        handleRotation = (Tools.pivotRotation == PivotRotation.Local ?
            node.transform.rotation : Quaternion.identity);

        //Rendering
        SetInTangentGizmo();
        SetOutTangentGizmo();

        if(renderCurves && parentSpline != null)
        {
            if(node.index == null || node.index == -1)
            {
                node.index = parentSpline.GetIndexOfNode(node);
            }
            if(node.index != -1)
            {
                RenderAdjacentCurves();
            }
        }
    }

    private Vector2 SetInTangentGizmo()
    {
        return SetTangentGizmo(ref node.inTangent, SelectedCurveUI.inTangent);
    }

    private Vector2 SetOutTangentGizmo()
    {
       return SetTangentGizmo(ref node.outTangent, SelectedCurveUI.outTangent);
    }

    private Vector2 SetTangentGizmo(ref Vector2 tangent, SelectedCurveUI requestedUI)
    {
        Handles.color = Color.white;
        Vector2 point = handleTransform.TransformPoint(tangent);
        if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap))
        {
            selectedUI = requestedUI;
        }
        if(selectedUI == requestedUI)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(node);
                tangent = handleTransform.InverseTransformPoint(point);
            }
        }

        Handles.DrawLine(point, node.transform.position);
        return point;
    }

    public void RenderAdjacentCurves()
    {
        if(node.index != null && parentSpline != null)
        {
            //Render connection to previous node
            if(node.index == 0)
            {
                if (parentSpline.connectiveness == BezierSpline.SplineLink.Loop)
                    RenderCurve(parentSpline.points[parentSpline.points.Count - 1], node);
            }
            else
            {
                RenderCurve(parentSpline.points[node.index.Value - 1], node);
            }
            //Render connection to next node
            if(node.index == parentSpline.points.Count - 1)
            {
                if (parentSpline.connectiveness == BezierSpline.SplineLink.Loop)
                    RenderCurve(node, parentSpline.points[0]);
            }
            else
            {
                RenderCurve(node, parentSpline.points[node.index.Value + 1]);
            }
        }
    }

    public static void RenderCurve(CurveNode lhs, CurveNode rhs)
    {
        Handles.color = Color.white;
        Vector2 lineStart = CurveNode.GetInvervalPosition(lhs, rhs, 0f);
        for (int i = 0; i < lineRenderSteps; i++)
        {
            Vector2 lineEnd = CurveNode.GetInvervalPosition(lhs, rhs, (i + 1) / (float)lineRenderSteps);
            Handles.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;
        }
    }
}

#endif