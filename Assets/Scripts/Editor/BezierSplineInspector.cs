using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor {
    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const int lineRenderSteps = 15; //

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        spline = target as BezierSpline;
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddNode();
            EditorUtility.SetDirty(spline);
        }
    }

    private void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = (Tools.pivotRotation == PivotRotation.Local ?
            spline.transform.rotation : Quaternion.identity);

        if(spline.points.Count > 1)
        {
            ShowFullCurve();
        }
        for(int i = 0; i < spline.points.Count; i++)
        {
            SetNodeGizmo(i);
        }
    }

    public void ShowFullCurve()
    {
        for(int i = 0; i < spline.points.Count - 1; i++)
        {
            RenderCurve(spline.points[i], spline.points[i + 1]);
        }
        if(spline.connectiveness == BezierSpline.SplineLink.Loop)
        {
            RenderCurve(spline.points[spline.points.Count - 1], spline.points[0]);
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

    #region Individual Node Rendering
    private enum SelectedCurveType
    {
        outTangent,
        inTangent,
        nodeRoot,
        unknown
    }

    private static Color[] nodeDisplayColors =
    {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private int selectedNodeIndex;
    private SelectedCurveType selectedUI;
    private const float handleRootSize = 0.06f;
    private const float handleTangentSize = 0.04f;
    private const float pickSize = 0.06f;

    private Vector2 GetTangent(CurveNode node, SelectedCurveType type)
    {
        if (type == SelectedCurveType.inTangent) return node.inTangent;
        return node.outTangent;
    }

    private void SetTangent(int index, SelectedCurveType type, Vector2 value)
    {
        if (type == SelectedCurveType.inTangent)
        {
            spline.points[index].inTangent = value;
        }
        else
        {
            spline.points[index].outTangent = value;
        }
    }

    private void SetNodeGizmo(int index)
    {
        SetInTangentGizmo(index);
        SetOutTangentGizmo(index);
        SetNodeRootGizmo(index);
    }

    private Vector2 SetInTangentGizmo(int index)
    {
        return SetTangentGizmo(index, SelectedCurveType.inTangent);
    }

    private Vector2 SetOutTangentGizmo(int index)
    {
        return SetTangentGizmo(index, SelectedCurveType.outTangent);
    }

    private Vector2 SetTangentGizmo(int index, SelectedCurveType requestedUI)
    {
        Vector2 tangent = GetTangent(spline.points[index], requestedUI);
        Handles.color = nodeDisplayColors[index % nodeDisplayColors.Length];
        Vector2 point = spline.points[index].transform.TransformPoint(tangent);
        if (Handles.Button(point, handleRotation, handleTangentSize, pickSize, Handles.DotHandleCap))
        {
            selectedUI = requestedUI;
            selectedNodeIndex = index;
        }
        if (selectedUI == requestedUI && selectedNodeIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(spline.points[index]);
                EditorUtility.SetDirty(spline);
                tangent = spline.points[index].transform.InverseTransformPoint(point);
                SetTangent(index, requestedUI, tangent);
            }
        }

        Handles.DrawLine(point, spline.points[index].transform.position);
        return point;
    }

    private Vector2 SetNodeRootGizmo(int index)
    {
        Handles.color = nodeDisplayColors[index % nodeDisplayColors.Length];
        Vector2 point = spline.points[index].position;
        float size = handleRootSize;
        if(index == 0)
        {
            size *= 2;
        }
        if (Handles.Button(point, handleRotation, size, pickSize, Handles.DotHandleCap))
        {
            selectedUI = SelectedCurveType.nodeRoot;
            selectedNodeIndex = index;
        }
        if(selectedUI == SelectedCurveType.nodeRoot && selectedNodeIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if(EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(spline.points[index]);
                EditorUtility.SetDirty(spline);
                spline.points[index].position = point;
            }
        }
        return point;
    }
    #endregion
}
