using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    public static CameraManager instance;
    public GameObject player = null;
    private Vector3 lastPlayerPosition = new Vector3(0,0,-10);
    void Awake() {
        if (instance != null) {
            DestroyImmediate(gameObject);
        }
        else {
            instance = this;
        }
    }

    private void LateUpdate() {
        if (player != null) {
            lastPlayerPosition = player.transform.position;
        }
        var tempPos = new Vector3(lastPlayerPosition.x, lastPlayerPosition.y, -10);
        transform.position = tempPos;
//        transform.position = transform.position + new Vector3(0.01f, 0, 0);
    }
}
