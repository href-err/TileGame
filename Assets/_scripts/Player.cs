using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private static readonly Vector3 offset = new Vector3(0f, 1f, 0f);

    private HexTile location;
    public HexTile Location
    {
        get { return location; }
        set { Move(value); }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Move(HexTile destination)
    {
        transform.position = destination.transform.position + offset;
        location = destination;
    }
}
