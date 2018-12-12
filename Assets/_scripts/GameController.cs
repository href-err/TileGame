using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {
    private const int SPAWN_DISTANCE = 4;

    private List<Player> players;
    public List<Player> Players { get { return players; } }

    private Player activePlayer;
    public Player ActivePlayer { get { return activePlayer; } }

    private LevelGen levelGen;

    public int playerCount;

    public GameObject playerObject;


    // Use this for initialization
    void Awake()
    {
        levelGen = GetComponent<LevelGen>();

        players = new List<Player>();
    }


    void Start()
    {
        levelGen.CreateLevel();
		for (int i = 0; i < playerCount; i++)
        {
            SpawnPlayer();
        }

        activePlayer = players.First();
	}
	

	// Update is called once per frame
	void Update () {
		
	}


    private void SpawnPlayer()
    {
        List<HexTile> availableTiles = levelGen.Tiles;
        List < HexTile > allTiles = levelGen.Tiles;

        foreach (Player player in Players)
        {
            Dictionary<HexTile, int> playerRange = player.Location.RangeFrom;

            foreach (HexTile tile in allTiles)
            {
                if (playerRange[tile] < SPAWN_DISTANCE || playerRange[tile] > SPAWN_DISTANCE + 1)
                    availableTiles.Remove(tile);
            }
        }

        int spawnIndex = (int)(Random.value * availableTiles.Count);

        Player newPlayer = Instantiate(playerObject, transform).GetComponent<Player>();

        players.Add(newPlayer);

        newPlayer.Location = availableTiles[spawnIndex];
    }

}
