using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Settings/World Tile Settings")]
public class WorldTiles : ScriptableObject
{
    public int futureTileDepth = 3;
    public int pastTileDepth = 3;
    [Space]
    
    public List<WeightedTile> startingTiles;
    public List<WeightedTile> bossTiles;
    public List<WeightedTile> standardTiles;
}

[System.Serializable]
public class WeightedTile {
    public GameObject tilePrefab;
    public int tileWeight = 1;
}