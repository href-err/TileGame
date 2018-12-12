using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selector : MonoBehaviour {
    private GameController gameController;
    private LevelGen levelGen;

    private List<HexTile> selectedPath;
    private HexTile pathStart;

    private bool pathSelectMode;
    public bool PathSelectMode
    {
        get { return pathSelectMode; }
    }

    void Awake () {
        selectedPath = new List<HexTile>();
        gameController = GetComponent<GameController>();
        levelGen = GetComponent<LevelGen>();
	}


    public void SelectTarget(HexTile tile)
    {
        selectedPath.ForEach(x => x.ToggleSelect());

        selectedPath = HexTile.GetPath(pathStart, tile);

        selectedPath.ForEach(x => x.ToggleSelect());
    }

    
    public void TogglePathSelectMode()
    {
        pathSelectMode = !pathSelectMode;

        if (pathSelectMode) EnterPathSelectMode();

        if (!pathSelectMode) ExitPathSelectMode();
    }


    public void EnterPathSelectMode()
    {
        HexTile startTile = gameController.ActivePlayer.Location;
        if (startTile == null)
        {
            TogglePathSelectMode();
            return;
        }

        pathStart = startTile;
    }

    public void ExitPathSelectMode()
    {

    }
}
