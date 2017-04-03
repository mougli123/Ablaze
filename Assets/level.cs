﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[System.Serializable]
public class level : MonoBehaviour {

    public float lowp, lefp, rgtp, hihp;

	// Use this for initialization
	void Start () {
        foreach (GameObject j in GameObject.FindGameObjectsWithTag("Block")) {
            if (j.transform.position.x < lefp) lefp = j.transform.position.x;
            if (j.transform.position.x > rgtp) rgtp = j.transform.position.x;
            if (j.transform.position.y < lowp) lowp = j.transform.position.y;
            if (j.transform.position.y > hihp) hihp = j.transform.position.y;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!Application.isPlaying) {
            transform.position = new Vector2(0, 0);
            foreach (GameObject j in GameObject.FindGameObjectsWithTag("Block")) {
                j.transform.position = new Vector2(Mathf.RoundToInt(j.transform.position.x), Mathf.RoundToInt(j.transform.position.y));
                j.gameObject.transform.parent = this.transform;
                if (j.transform.position.x < lefp) lefp = j.transform.position.x;
                if (j.transform.position.x > rgtp) rgtp = j.transform.position.x;
                if (j.transform.position.y < lowp) lowp = j.transform.position.y;
                if (j.transform.position.y > hihp) hihp = j.transform.position.y;
            }
        }
	}
}
