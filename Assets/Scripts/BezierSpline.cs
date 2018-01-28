using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierSpline : MonoBehaviour {

    public enum SplineLink
    {
        Single,
        Loop
    }
    public List<CurveNode> points;
    public SplineLink connectiveness;
    [SerializeField]
    public float StartEndColliderSize = 5f;

    private static Vector2 initialSpawnLocation = new Vector2(-1, 0);
    private const int spawnStep = 2;

    public void Reset()
    {
        points = new List<CurveNode>();
        AddNodeToList(_GenerateNodeLocal(new Vector2(-1, 0)));
        AddNodeToList(_GenerateNodeLocal(new Vector2(1, 0)));
    }

    #region Data Get/Set
    public int GetIndexOfNode(CurveNode node)
    {
        return points.FindIndex(listNode => listNode == node);
    }
    private CurveNode _GenerateNodeLocal(Vector2 spawnLocation)
    {
        GameObject newObject = Instantiate(new GameObject(), this.transform);
        newObject.transform.localPosition = spawnLocation;
        newObject.name = "Curve Node";
        newObject.tag = "Curve";
        CurveNode node = newObject.AddComponent<CurveNode>();
        CircleCollider2D nodeCollider = newObject.AddComponent<CircleCollider2D>();
        nodeCollider.radius = StartEndColliderSize;
        nodeCollider.isTrigger = true;
        node.inTangent = new Vector2(-1, 0);
        node.outTangent = new Vector2(1, 0);
        return newObject.GetComponent<CurveNode>();
    }

    private void AddNodeToList(CurveNode newNode)
    {
        newNode.index = points.Count;
        points.Add(newNode);
    }

    public void AddNode()
    {
        Vector2 newLocation = new Vector2(initialSpawnLocation.x + (2 * points.Count),
            initialSpawnLocation.y);
        AddNodeToList((_GenerateNodeLocal(newLocation)));
    }
    #endregion

    public void UpdateStartAndEndColliders()
    {
        for(int i = 0; i < points.Count; i++)
        {
            CircleCollider2D col = points[i].gameObject.GetComponent<CircleCollider2D>();
            if (i == 0 || i == points.Count - 1)
            {
                if(col == null)
                {
                    col = points[i].gameObject.AddComponent<CircleCollider2D>();
                    col.isTrigger = true;
                }
                col.radius = StartEndColliderSize;
                points[i].gameObject.name = (i == 0 ? "Start Node" : "End Node");
            }
            else
            {
                points[i].gameObject.name = "Curve Node";
                if(col != null)
                {
                    DestroyImmediate(col); //Destroys this collider
                }
            }
        }
    }
    public Vector2 GetPosition(float progress)
    {
        int i;
        if (progress >= 1f)
        {
            progress = 1f;
            i = points.Count - 2;
        }
        else
        {
            progress = Mathf.Clamp01(progress) * (points.Count - 1);
            i = Mathf.FloorToInt(progress);
            progress -= i;
        }
        return CurveNode.GetInvervalPosition(points[i], points[i + 1], progress);
    }

    public Vector2 GetVelocity(float progress)
    {
        int i;
        if (progress >= 1f)
        {
            progress = 1f;
            i = points.Count - 2;
        }
        else
        {
            progress = Mathf.Clamp01(progress) * (points.Count - 1);
            i = (int)progress;
            progress -= i;
        }

        return CurveNode.GetFirstDerivative(points[i], points[i + 1], progress);
    }
}
