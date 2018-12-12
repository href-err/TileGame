using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGen : MonoBehaviour {
    public GameObject tileObject;

    private Bounds bounds;
    public Bounds Bounds { get { return bounds; } }

    private List<HexTile> tiles;
    public List<HexTile> Tiles
    {
        get { return tiles; }
    }

    // Use this for initialization
    void Awake()
    {
        bounds = GetComponent<Collider>().bounds;
        tiles = new List<HexTile>();
    }


    public void CreateLevel()
    { 
        HexTile firstTile = AddTile(Vector3.zero);
        firstTile.GenerateNeighbors();
	}


    public HexTile AddTile(Vector3 location)
    {
        GameObject newTileObject = Instantiate(tileObject, transform);

        newTileObject.transform.position = location;

        HexTile newTile = newTileObject.GetComponent<HexTile>();

        tiles.Add(newTile);

        return newTile;
    }
}

