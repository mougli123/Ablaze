﻿using UnityEngine;
using System.Collections; 

[System.Serializable()]
[ExecuteInEditMode]
public class cameracontrol : MonoBehaviour {

    Player ply;
    level lvl;

    public float target; // range from -3 to 3, positive meants the player should be below the camera, negative means player should be above the camera.

    public float shaketime = 0f;
    public bool hardshake;

    float shake = 0.1f;

    private float lowp, lefp, rgtp, hihp;

    /*
    Depending on the type of level you might want the player to start at the bottom and go up or start at the top and go down.
    For most cases target should always be positive.

    1) If there is no scrolling vertically, target should be positive
    2) If the player will be going up more than down, target should be positive
    3) Only if the player will be almost exclusively going down should targe be negative


    The good looking numbers are 2.5 and -2.5, don't bother with anything else even though you can.
    Do not make Target any larger than the camera can see or things'll go not your way friend.
    */

    // Use this for initialization
    void Start () {
        if (GameObject.FindGameObjectWithTag("Player")) {
            ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            lvl = GameObject.FindGameObjectWithTag("Level").GetComponent<level>();


            // ensure the camera starts out inside the level
            if (GameObject.FindGameObjectWithTag("Block")) {
                GameObject[] arg = GameObject.FindGameObjectsWithTag("Block");
                rgtp = lefp = arg[0].transform.position.x;
                lowp = hihp = arg[0].transform.position.y;

                foreach (GameObject j in arg) {
                    if (j.transform.position.x < lefp) lefp = j.transform.position.x;
                    if (j.transform.position.x > rgtp) rgtp = j.transform.position.x;
                    if (j.transform.position.y < lowp) lowp = j.transform.position.y;
                    if (j.transform.position.y > hihp) hihp = j.transform.position.y;
                }
            }

            if (target > 0) {
                transform.position = new Vector3((lefp + rgtp) / 2f, lowp + 4, transform.position.z);
            }
            else {
                transform.position = new Vector3((lefp + rgtp) / 2f, hihp - 4, transform.position.z);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (ply) {
            float xchange = 0;
            float lerpy = transform.position.y;

            if (lvl.lefp < transform.position.x - 7.75f) {
                if (ply.transform.position.x < transform.position.x - 2 && lvl.lefp < transform.position.x - 7.75f) {
                    xchange = -3f * Time.deltaTime;
                }
            }
            if (lvl.rgtp > transform.position.x + 7.75f) {
                if (ply.transform.position.x > transform.position.x + 2 && lvl.rgtp > transform.position.x + 7.75f) {
                    xchange = 3f * Time.deltaTime;
                }
            }

            if (ply.isAlive()) {
                lerpy = ply.transform.position.y + target;
            }

            if ((lvl.hihp <= transform.position.y + 4 && lerpy > transform.position.y) || (lvl.lowp >= transform.position.y - 4 && lerpy < transform.position.y)) {
                lerpy = Mathf.RoundToInt(transform.position.y);
            }


            if (shaketime > 0f) {
                if (hardshake && Mathf.Abs(shake) != 0.25f) shake = 0.25f;
                xchange += shake;

                if (hihp - lowp > 13) lerpy += shake;

                shake *= -1;
                shaketime -= Time.fixedDeltaTime;
            }
            else {
                hardshake = false;
                shake = 0.1f;
            }

            if (ply.transform.position.x < 1000f) {
                transform.position = new Vector3(transform.position.x + xchange, Mathf.Lerp(transform.position.y, lerpy, 0.5f), transform.position.z);
            }
        }

    }
}
