using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierSpline : MonoBehaviour {

    public enum SplineLink
    {
        Single,
        Loop
    }
    public List<CurveNode> points;
    public List<float> distances;
    public float distanceMultiplier = 1f;

    public SplineLink connectiveness;
    [SerializeField]
    public float StartEndColliderSize = 5f;

    private static Vector2 initialSpawnLocation = new Vector2(-1, 0);
    private const int spawnStep = 2;

    public void Reset()
    {
        points = new List<CurveNode>();
        distances = new List<float>();
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
        distances.Add(10f);
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
                points[i].gameObject.tag = "Curve";
            }
            else
            {
                points[i].gameObject.name = "Curve Node";
                if(col != null)
                {
                    DestroyImmediate (col); //Destroys this collider
                }
            }
        }
    }

    #region Distance Cludge
    public float _TotalDistance()
    {
        float totalDistance = 0f;
        foreach(float distance in distances)
        {
            totalDistance += distance;
        }
        return totalDistance;
    }

    public void FixDistances()
    {
        if(distances == null)
        {
            distances = new List<float>();
        }
        while(distances.Count >= points.Count)
        {
            distances.RemoveAt(distances.Count - 1);
        }
        for(int i = distances.Count; i < points.Count - 1; i++)
        {
            distances.Add(10f);
        }
    }
    public float getTotalProgress(int lhsIndex, int rhsIndex)
    {
        float distance = 0f;
        for(int i = lhsIndex; i < rhsIndex; i++)
        {
            distance += distances[i];
        }
        return distance / _TotalDistance();
    }
    #endregion

    public Vector2 GetPosition(float progress)
    {
        int i;
        GetNodeProgress(progress, out progress, out i);
        return CurveNode.GetInvervalPosition(points[i], points[i + 1], progress);
    }

    public Vector2 GetVelocity(float progress)
    {
        int i;
        GetNodeProgress(progress, out progress, out i);

        return CurveNode.GetFirstDerivative(points[i], points[i + 1], progress);
    }

    private void GetNodeProgress (float progress, out float nodeProgress, out int nodeindex )
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

        nodeProgress = progress;
        nodeindex = i;
    }
}
