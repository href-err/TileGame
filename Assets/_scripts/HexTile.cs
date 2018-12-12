using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexTile : MonoBehaviour {
    private LevelGen levelGen;
    private GameController gameController;
    private Selector selector;

    private void Awake()
    {
        levelGen = GetComponentInParent<LevelGen>();
        gameController = GetComponentInParent<GameController>();
        selector = GetComponentInParent<Selector>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnMouseDown()
    {
        selector.TogglePathSelectMode();
        selector.SelectTarget(this);
    }


    private void OnMouseEnter()
    {
        //if (selector.PathSelectMode) selector.SelectTarget(this);
    }


    private void OnMouseUp()
    {
        
    }


    private bool selected;
    public void ToggleSelect()
    {
        selected = !selected;

        if (selected) transform.Translate(new Vector3(0f, 0.2f, 0f));

        if (!selected) transform.Translate(new Vector3(0f, -0.2f, 0f));
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

    public static List<HexTile> GetPath(HexTile start, HexTile end)
    {
        Dictionary<HexTile, int> fromStart = start.RangeFrom;
        Dictionary<HexTile, int> fromEnd = end.RangeFrom;

        int maxLength = fromStart[end];
        List<HexTile> path = new List<HexTile>();

        for (int stepCount = 0; stepCount <= maxLength; stepCount++)
        {
            HexTile step = null;

            foreach (HexTile tile in fromStart.Keys)
            {
                if (fromStart[tile] >= stepCount) continue;
                if (fromEnd[tile] >= maxLength - stepCount) continue;
                if (fromStart[tile] + fromEnd[tile] > maxLength) continue;

                step = tile;
            }

            path.Add(step);
        }

        return path;
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
            {
                if (position == null) continue;
                HexTile neighbor = GetHex(position, levelGen.transform);

                if (neighbor == null) continue;
                neighborList.Add(neighbor);
            }
            return neighborList;
        }
    }


    private Dictionary<HexTile, int> tileRanges;
    public Dictionary<HexTile, int> RangeFrom
    {
        get
        {
            if (tileRanges.Count != levelGen.Tiles.Count)
                SetRanges();

            return tileRanges;
        }
    }

    private void SetRanges()
    {
        List<HexTile> allTiles = levelGen.Tiles;
        Dictionary<HexTile, int> ranges = new Dictionary<HexTile, int>();

        int distance = 0;
        ranges.Add(this, distance);

        while (ranges.Count <= allTiles.Count)
        {
            distance++;

            List<HexTile> searchedTiles = ranges.Keys.ToList<HexTile>();
            foreach (HexTile tile in searchedTiles)
            {
                foreach (HexTile neighbor in tile.Neighbors)
                {
                    if (ranges.ContainsKey(neighbor)) continue;
                    ranges.Add(neighbor, distance);
                }
            }
        }

        tileRanges = ranges;
    }


    /// <summary>
    /// Recursively creates new tiles to fill all open neighboring positions up to the level bound edges
    /// </summary>
    public void GenerateNeighbors()
    {
        foreach (Vector3 neighbor in NeighborPositions)
        {
            if (! levelGen.Bounds.Contains(neighbor)) continue;
            if (GetHex(neighbor, levelGen.transform) != null) continue;

            HexTile newHex = levelGen.AddTile(neighbor);
            newHex.GenerateNeighbors();
        }
    }
}