using UnityEngine;
using System.Collections;

public class Ledge : MonoBehaviour {

    Player ply;

    BoxCollider2D col;

    // Use this for initialization
    void Start() {
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        if (ply.isAlive()) {
            float d = Vector3.Distance(ply.transform.position, transform.position);
            if ((d < 1.5f && ply.transform.position.y < transform.position.y + 0.5f) || (d < 2.0f && ply.getYVel() > 0f)) {
                col.enabled = false;
            }
            else col.enabled = true;

        }
    }
}
