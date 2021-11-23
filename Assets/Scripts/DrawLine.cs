using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {
    public GameObject linePrefab;
    public GameObject wheelPrefab;
    private GameObject drawLine = null;
    private GameObject carLine = null;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> fingerPositions = new List<Vector2>();
    private Rigidbody2D rigidbody2D;
    private Vector3 lastPlayerPosition = new Vector3(0,0,0);

    
//    private LineRenderer lineRendererCar;
//    private EdgeCollider2D edgeColliderCar;
//    private List<Vector3> fingerPositionsCar = new List<Vector3>();

    private void Start() {
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            CameraManager.instance.player = null;
            CreateLine();
        }
        if (Input.GetMouseButton(0)) {
            var tempFingerPos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x,Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
            if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .01f) {
                UpdateLine(tempFingerPos);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            var leftWheel = Instantiate(wheelPrefab, new Vector3(fingerPositions[0].x, fingerPositions[0].y, 0), Quaternion.identity, drawLine.transform);
            var rightWheel = Instantiate(wheelPrefab, new Vector3(fingerPositions[fingerPositions.Count-1].x, fingerPositions[fingerPositions.Count-1].y, 0), Quaternion.identity, drawLine.transform);
            var leftJoint = leftWheel.GetComponent<WheelJoint2D>();
            leftJoint.connectedBody = rigidbody2D;
            leftJoint.connectedAnchor = fingerPositions[0];
            var rightJoint = rightWheel.GetComponent<WheelJoint2D>();
            rightJoint.connectedBody = rigidbody2D;
            rightJoint.connectedAnchor = fingerPositions[fingerPositions.Count-1];
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            CameraManager.instance.player = leftWheel;
        }
    }

//    private void LateUpdate() {
//        if (drawLine != null) {
//            lastPlayerPosition = drawLine.transform.position;
//        }
//        transform.position = lastPlayerPosition;
//    }

    private void CreateLine() {
        if (drawLine != null) {
            Destroy(drawLine);
            Destroy(carLine);
        }
        drawLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = drawLine.GetComponentInChildren<LineRenderer>();
        edgeCollider = drawLine.GetComponentInChildren<EdgeCollider2D>();
        rigidbody2D = drawLine.GetComponent<Rigidbody2D>();
        fingerPositions.Clear();
        var tempFingerPos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
        fingerPositions.Add(tempFingerPos);
        fingerPositions.Add(tempFingerPos);
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
        edgeCollider.points = fingerPositions.ToArray();
    }
    

    private void UpdateLine(Vector2 newFingerPos) {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount-1, newFingerPos);
        edgeCollider.points = fingerPositions.ToArray();
    }
}