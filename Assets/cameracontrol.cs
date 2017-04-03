using UnityEngine;
using System.Collections; 

public class cameracontrol : MonoBehaviour {

    Player ply;
    level lvl;

	// Use this for initialization
	void Start () {
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lvl = GameObject.FindGameObjectWithTag("Level").GetComponent<level>();
	}
	
	// Update is called once per frame
	void Update () {
	    if ((ply.transform.position.x < transform.position.x - 2 && lvl.lefp < transform.position.x - 7.75f) || !(lvl.rgtp > transform.position.x + 7.75f)) {
            transform.position = new Vector3(transform.position.x - 2f * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if ((ply.transform.position.x > transform.position.x + 2 && lvl.rgtp > transform.position.x + 7.75f) || !(lvl.lefp < transform.position.x - 7.75f)) {
            transform.position = new Vector3(transform.position.x + 2f * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}
