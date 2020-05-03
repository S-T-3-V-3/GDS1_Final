using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileManager : MonoBehaviour
{
    public List<Tile> currentTiles;
    public int maxTiles;

    void Start() {
        if (currentTiles == null)
            currentTiles = new List<Tile>();

        currentTiles.Add(GameObject.Instantiate(GameManager.Instance.gameSettings.worldTiles.GetStartingTile()).GetComponent<Tile>());

        while (currentTiles.Count < maxTiles) {
            AddTile();
        }
    }

    void AddTile() {
        GameObject newTile = GameManager.Instance.gameSettings.worldTiles.GetStandardTile();
        Tile currentTile = GameObject.Instantiate(newTile).GetComponent<Tile>();
        Tile previousTile = currentTiles.Last();
        currentTiles.Add(currentTile);
        currentTile.transform.rotation = previousTile.endTransform.rotation;
        currentTile.transform.position = previousTile.endTransform.position - currentTile.startTransform.position;
    }
}
