using UnityEngine;
using System.Collections;

public class lantern : MonoBehaviour {

    Player ply;

    bool lit;
    string litcolor;

    // Use this for initialization
    void Start() {
        lit = false;
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update () {
        if (isNear()) {
            string color = ply.getColor();
            GameObject frobj;

            if (!lit) {
                frobj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFire" + color), transform.position, Quaternion.Euler(-90f, 0, 0), transform) as GameObject;
                frobj.name = "SpriteLantFire" + color;
                frobj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                litcolor = color;
                lit = true;
            }
            else {
                if (litcolor != color) {
                    GameObject.Find("SpriteLantFire" + litcolor).GetComponent<ParticleSystem>().loop = false;
                    frobj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFire" + color), transform.position, Quaternion.Euler(-90f, 0, 0), transform) as GameObject;
                    frobj.name = "SpriteLantFire" + color;
                    frobj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    litcolor = color;
                    lit = true;
                }
            }
        }
	}

    bool isNear() {
        if (Mathf.RoundToInt(ply.transform.position.x) == transform.position.x  && (Mathf.RoundToInt(ply.transform.position.y) == transform.position.y || Mathf.RoundToInt(ply.transform.position.y) == transform.position.y - 1)) {
            return true;
        }
        else return false;
    }
}
