using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGen : MonoBehaviour {
    public GameObject tileObject;
    public Bounds bounds;

    private List<HexTile> hexTiles;

	// Use this for initialization
	void Start () {
        hexTiles = new List<HexTile>();
        bounds = GetComponent<Collider>().bounds;

        HexTile firstTile = AddTile(Vector3.zero);
        firstTile.GenerateNeighbors();
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnMouseDown()
    {
        
    }

    public HexTile AddTile(Vector3 location)
    {
        GameObject newTile = Instantiate(tileObject, transform);

        newTile.transform.position = location;

        return newTile.GetComponent<HexTile>();
    }
}

