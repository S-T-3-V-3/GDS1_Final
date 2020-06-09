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
    public List<Color> rockColors;

    public Transform startLocation;
    public bool isInitialized = false;

    void Start() {
        foreach(MeshRenderer rock in rocks) {
            rock.material.color = rockColors[Random.Range(0,1)]; 
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

                if (currentDepth > 0)
                    connection.otherTile.AddNeighbours(this, currentDepth);
            }
            else if (connection.otherTile != caller) {
                if (currentDepth > 0)
                    connection.otherTile.AddNeighbours(this, currentDepth);
            }
        }
    }

    public void RemoveNeighbours(Tile caller, int currentDepth) {
        currentDepth--;

        foreach (Connection connection in connections) {
            if (connection.otherTile != caller && connection.otherTile != null) {
                if (currentDepth > 0) {
                    connection.otherTile.RemoveNeighbours(this, currentDepth);
                }
                else if (connection.otherTile != null) {
                    connection.otherTile.RemoveNeighbours(this, currentDepth);
                    GameObject.Destroy(connection.otherTile.gameObject);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!isInitialized) return;

        if (other.gameObject.GetComponent<PlayerController>() != null) {
            RemoveNeighbours(this, GameManager.Instance.gameSettings.tiles.pastTileDepth);
            AddNeighbours(this, GameManager.Instance.gameSettings.tiles.futureTileDepth);
        }
    }
}

[System.Serializable]
public class Connection {
    public Transform transform;
    public Tile otherTile = null;
    public bool isBlocked = false;
}