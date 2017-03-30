using UnityEngine;
using System.Collections;

public class colorBlock : MonoBehaviour {

    public string color;
    Player ply;

    // Use this for initialization
    void Start() {
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update () {
	    if (ply.getColor() != color) {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Puzzle/" + color + "BlockEmpt");
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Puzzle/" + color + "BlockFill");
            GetComponent<BoxCollider2D>().enabled = true;
        }
	}
}
