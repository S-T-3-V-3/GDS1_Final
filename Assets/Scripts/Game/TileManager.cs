using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileManager : MonoBehaviour
{
    public Tile rootTile;
    public int maxWeight = 0;

    WorldTiles worldTiles;

    void Awake() {
        worldTiles = GameManager.Instance.gameSettings.tiles;

        // Add Standard Tiles
        foreach(WeightedTile wt in worldTiles.standardTiles) {
            maxWeight += wt.tileWeight;
        }

        rootTile = GameObject.Instantiate(this.GetStartingTile(),this.transform).GetComponent<Tile>();
    }

    void Start() {
        rootTile.AddNeighbours(null, GameManager.Instance.gameSettings.tiles.futureTileDepth);
    }

    public GameObject GetStartingTile() {
        return worldTiles.startingTiles[Random.Range(0, worldTiles.startingTiles.Count)].tilePrefab;
    }

    public GameObject GetBossTile() {
        return worldTiles.bossTiles[Random.Range(0, worldTiles.bossTiles.Count)].tilePrefab;
    }

    public GameObject GetStandardTile() {
        int selectedWeight = Random.Range(0,maxWeight + 1); // Unity random.range is garbage, eat some horse poop
        int tileIndex = 0;

        for (int i = 0; i < worldTiles.standardTiles.Count; i++) {
            selectedWeight -= worldTiles.standardTiles[i].tileWeight;

            if (selectedWeight <= 0 && worldTiles.standardTiles[i].tileWeight > 0) {
                tileIndex = i;
                i = worldTiles.standardTiles.Count;
            }
        }

        return worldTiles.standardTiles[tileIndex].tilePrefab;
    }
}
