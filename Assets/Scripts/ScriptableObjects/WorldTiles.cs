using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/World Tile Settings")]
public class WorldTiles : ScriptableObject
{
    public List<GameObject> startingTiles;
    public List<GameObject> bossTiles;
    public List<GameObject> standardTiles;

    public GameObject GetStartingTile() {
        return startingTiles[Random.Range(0, startingTiles.Count - 1)];
    }

    public GameObject GetBossTile() {
        return bossTiles[Random.Range(0, bossTiles.Count - 1)];
    }

    public GameObject GetStandardTile() {
        return standardTiles[Random.Range(0, standardTiles.Count - 1)];
    }
}
