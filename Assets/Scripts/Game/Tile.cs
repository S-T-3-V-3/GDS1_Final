using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    public List<Connection> connections;
    public List<MeshRenderer> rocks;
    public List<MeshRenderer> groundRocks;

    public Transform startLocation;
    public bool isInitialized = false;
    public bool hasActivated = false;

    void Start() {
        //Set a material for the rocks and floors from the game manager
        foreach(MeshRenderer rock in rocks)
        {
            rock.material = GameManager.Instance.rockMaterials[Random.Range(0,GameManager.Instance.rockMaterials.Count)]; 
        }
        foreach(MeshRenderer ground in groundRocks)
        {
            ground.material = GameManager.Instance.groundMaterials[Random.Range(0,GameManager.Instance.groundMaterials.Count)]; 
        }
    }

    void OnDrawGizmos() {
        foreach (Connection n in connections.Where(x => x.transform != null)) {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(n.transform.position + n.transform.forward * 0.9f, 0.1f);
            Gizmos.DrawSphere(n.transform.position + n.transform.forward * 0.7f, 0.2f);
            Gizmos.DrawSphere(n.transform.position + n.transform.forward * 0.4f, 0.3f);
            Gizmos.DrawSphere(n.transform.position, 0.4f); 
        }
    }

    public void BlockAllConnections(Tile caller) {
        foreach (Connection connection in connections) {
            if (connection.otherTile == null) {
                BlockConnection(connection);
            }
            else if (connection.otherTile != caller) {
                BlockConnection(connection);
            }
        }
    }

    public void AddNeighbours(Tile caller, int currentDepth) {
        currentDepth--;

        foreach (Connection connection in connections) {
            if (connection.otherTile == null) {
                connection.otherTile = GameObject.Instantiate(GameManager.Instance.tileManager.GetStandardTile(),GameManager.Instance.tileManager.transform).GetComponent<Tile>();
                int connectionIndex = Random.Range(0,connection.otherTile.connections.Count);
                connection.otherTile.connections[connectionIndex].otherTile = this;
                Transform connectingTransform = connection.otherTile.connections[connectionIndex].transform;
                connection.otherTile.transform.position = connection.transform.position - connectingTransform.localPosition;
                float angle = connection.transform.eulerAngles.y + 180 - Mathf.Abs(connectingTransform.eulerAngles.y - connection.otherTile.transform.eulerAngles.y);
                connection.otherTile.transform.RotateAround(connectingTransform.position,Vector3.up,angle);
                connection.otherTile.isInitialized = true;

                if (currentDepth > 0) {
                    connection.otherTile.AddNeighbours(this, currentDepth);

                    if (connection.block != null)
                        UnblockConnection(connection);
                }
                else {
                    connection.otherTile.BlockAllConnections(this);
                }
            }
            else if (connection.otherTile != caller) {
                if (currentDepth > 0) {
                    connection.otherTile.AddNeighbours(this, currentDepth);
                    
                    if (connection.block != null)
                        UnblockConnection(connection);
                }
                else {
                    connection.otherTile.BlockAllConnections(this);
                }
            }
        }
    }

    // Must traverse a minimum of 3 tiles already explored OR 3 unexplored tiles before removing neighbours
    public void RemoveNeighbours(Tile caller, int exploredDepth, int unexploredDepth, bool force = false) {
        if (hasActivated) exploredDepth--;
        else unexploredDepth--;
        int currentDepth = Mathf.Min(exploredDepth, unexploredDepth);

        foreach (Connection connection in connections) {
            if (connection.otherTile != caller && connection.otherTile != null) {
                if (currentDepth > 0) {
                    connection.otherTile.RemoveNeighbours(this, exploredDepth, unexploredDepth);
                }
                else if (connection.otherTile != null) {
                    connection.otherTile.RemoveNeighbours(this, exploredDepth, unexploredDepth, true);
                    GameObject.Destroy(connection.otherTile.gameObject);
                    
                    if (!force)
                        BlockConnection(connection);
                }
            }
        }
    }

    void BlockConnection(Connection connection) {
        if (connection.block != null) return;

        connection.block = GameObject.Instantiate(GameManager.Instance.TileBlockingPrefab, connection.transform.position, connection.transform.rotation, this.transform);
    }

    void UnblockConnection(Connection connection) {
        if (connection.block == null) return;

        GameObject.Destroy(connection.block);
    }

    void OnTriggerEnter(Collider other) {
        if (!isInitialized) return;
        if (hasActivated) return;

        if (other.gameObject.GetComponent<PlayerController>() != null) {
            StartCoroutine(DoAddNeighbour());
            hasActivated = true;
        }
    }

    IEnumerator DoAddNeighbour() {
        RemoveNeighbours(this, GameManager.Instance.gameSettings.tiles.pastTileDepth, GameManager.Instance.gameSettings.tiles.pastTileDepth);
        AddNeighbours(this, GameManager.Instance.gameSettings.tiles.futureTileDepth);
        yield return null;
    }
}

[System.Serializable]
public class Connection {
    public Transform transform;
    public Tile otherTile = null;
    public GameObject block;
}