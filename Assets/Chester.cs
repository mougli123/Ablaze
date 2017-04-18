using UnityEngine;
using System.Collections;

public class Chester : MonoBehaviour {

    bool dead;

    Player ply;
    cameracontrol cam;
    Animator anim;

    // Use this for initialization
    void Start () {
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameracontrol>();
        anim = this.GetComponent<Animator>();

        dead = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!dead) {
            if (ply.isAlive()) {

                if (Vector3.Distance(ply.transform.position, transform.position) < 1f) {
                    if (!ply.getBursting()) {
                        ply.kill();
                        cam.shaketime = 0.25f;
                    }
                    else {
                        dead = true;
                    }
                }

            }

        }
        else {

        }
	}
}
