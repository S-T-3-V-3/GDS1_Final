using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public Transform startLocation;

    void OnDrawGizmos() {
        if (startTransform == null || endTransform == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startTransform.position, 0.4f);
        Gizmos.DrawSphere(startTransform.position + startTransform.forward * 0.4f, 0.3f);
        Gizmos.DrawSphere(startTransform.position + startTransform.forward * 0.7f, 0.2f);
        Gizmos.DrawSphere(startTransform.position + startTransform.forward * 0.9f, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endTransform.position, 0.4f);
        Gizmos.DrawSphere(endTransform.position + endTransform.forward * 0.4f, 0.3f);
        Gizmos.DrawSphere(endTransform.position + endTransform.forward * 0.7f, 0.2f);
        Gizmos.DrawSphere(endTransform.position + endTransform.forward * 0.9f, 0.1f);
    }
}