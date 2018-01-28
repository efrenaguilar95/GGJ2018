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

    private static Vector2 initialSpawnLocation = new Vector2(-1, 0);
    private const int spawnStep = 2;

    public void Reset()
    {
        points = new List<CurveNode>();
        AddNodeToList(_GenerateNodeLocal(new Vector2(-1, 0)));
        AddNodeToList(_GenerateNodeLocal(new Vector2(1, 0)));
    }

    public int GetIndexOfNode(CurveNode node)
    {
        return points.FindIndex(listNode => listNode == node);
    }
    private CurveNode _GenerateNodeLocal(Vector2 spawnLocation)
    {
        GameObject newObject = Instantiate(new GameObject(), this.transform);
        newObject.transform.localPosition = spawnLocation;
        newObject.name = "Curve Node";
        CurveNode node = newObject.AddComponent<CurveNode>();
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
}
