using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour {
    private LevelGen levelGen;

    private void Awake()
    {
        levelGen = GetComponentInParent<LevelGen>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnMouseDown()
    {
        transform.Translate(new Vector3(0f, 0.2f, 0f));
        Neighbors.ForEach(x => x.transform.Translate(new Vector3(0f, 0.1f, 0f)));
    }


    private void OnMouseUp()
    {
        transform.Translate(new Vector3(0f, -0.2f, 0f));
        Neighbors.ForEach(x => x.transform.Translate(new Vector3(0f, -0.1f, 0f)));
    }


    private bool selected;
    public void ToggleSelect()
    {
        selected = !selected;
    }
    /************************************************************************************************
     *                  Static Methods                                                              */

    public const float RADIUS = 1.0f;

    private static readonly List<Vector3> directions = new List<Vector3>
    {
        Quaternion.Euler(0f, 30f, 0f) * Vector3.left * RADIUS,
        Quaternion.Euler(0f, 90f, 0f) * Vector3.left * RADIUS,
        Quaternion.Euler(0f, 150f, 0f) * Vector3.left * RADIUS,
        Quaternion.Euler(0f, 210f, 0f) * Vector3.left * RADIUS,
        Quaternion.Euler(0f, 270f, 0f) * Vector3.left * RADIUS,
        Quaternion.Euler(0f, 330f, 0f) * Vector3.left * RADIUS,
    };
    public static List<Vector3> Directions
    {
        get { return directions; }
    }


    /// <summary>
    /// Gets the tile that corresponds to the given position, or returns null if none exists.
    /// </summary>
    /// <param name="position">Where to test for a tile</param>
    /// <param name="inParent">The parent object to search within</param>
    /// <returns>The tile containing the given position, or null if there was none</returns>
    public static HexTile GetHex(Vector3 position, Transform inParent)
    {
        Collider[] colliders = inParent.GetComponentsInChildren<Collider>();
        foreach (Collider test in colliders)
        {
            if (test.bounds.Contains(position) && test.GetComponent<HexTile>() != null)
                return test.GetComponent<HexTile>();
        }
        return null;
    }


    /************************************************************************************************
     *                      Dynamic Methods                                                         */

    /// <summary>
    /// Fetches a list of each neighboring position
    /// </summary>
    public List<Vector3> NeighborPositions
    {
        get {
            List<Vector3> neighbors = new List<Vector3>();
            for (int i = 0; i < Directions.Count; i++)
            {
                neighbors.Add(Directions[i] + transform.position);
            }
            return neighbors;
        }
    }


    /// <summary>
    /// Fetches a list of each existing neighboring tile
    /// </summary>
    public List<HexTile> Neighbors
    {
        get { List<HexTile> neighborList = new List<HexTile>();
            foreach (Vector3 position in NeighborPositions)
                if (position != null)
                    neighborList.Add(GetHex(position, levelGen.transform));
            return neighborList;
        }
    }


    /// <summary>
    /// Recursively creates new tiles to fill all open neighboring positions up to the level bound edges
    /// </summary>
    public void GenerateNeighbors()
    {
        foreach (Vector3 neighbor in NeighborPositions)
        {
            if (! levelGen.bounds.Contains(neighbor)) continue;
            if (GetHex(neighbor, levelGen.transform) != null) continue;

            HexTile newHex = levelGen.AddTile(neighbor);
            newHex.GenerateNeighbors();
        }
    }
}