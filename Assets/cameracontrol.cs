using UnityEngine;
using System.Collections; 

[System.Serializable()]
public class cameracontrol : MonoBehaviour {

    Player ply;
    level lvl;

    public float target; // range from -3 to 3, positive meants the player should be below the camera, negative means player should be above the camera.

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
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lvl = GameObject.FindGameObjectWithTag("Level").GetComponent<level>();

        if (target > 0) transform.position = new Vector3((lvl.lefp + lvl.rgtp) / 2f, lvl.lowp + 4, transform.position.z);
        else transform.position = new Vector3((lvl.lefp + lvl.rgtp) / 2f, lvl.hihp - 4, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        float xchange = 0;
        if (lvl.lefp < transform.position.x - 7.75f) {
            if (ply.transform.position.x < transform.position.x - 2 && lvl.lefp < transform.position.x - 7.75f) {
                xchange = -2f * Time.deltaTime;
            }
        }
        if (lvl.rgtp > transform.position.x + 7.75f) {
            if (ply.transform.position.x > transform.position.x + 2 && lvl.rgtp > transform.position.x + 7.75f) {
                xchange = 2f * Time.deltaTime;
            }
        }

        float lerpy = ply.transform.position.y + target;
        if ((lvl.hihp <= transform.position.y + 4 && lerpy > transform.position.y) || (lvl.lowp >= transform.position.y - 4 && lerpy < transform.position.y)) {
            lerpy = transform.position.y;
        }

        transform.position = new Vector3(transform.position.x + xchange, Mathf.Lerp(transform.position.y, lerpy, 0.5f), transform.position.z);
    }
}
