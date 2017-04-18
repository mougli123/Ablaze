using UnityEngine;
using System.Collections;

public class lantern : MonoBehaviour {

    static bool allit;
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
                frobj.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                litcolor = color;
                lit = true;
            }
            else {
                if (litcolor != color) {
                    GameObject.Find("SpriteLantFire" + litcolor).GetComponent<ParticleSystem>().loop = false;
                    frobj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFire" + color), transform.position, Quaternion.Euler(-90f, 0, 0), transform) as GameObject;
                    frobj.name = "SpriteLantFire" + color;
                    frobj.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                    litcolor = color;
                    lit = true;
                }
            }

            allit = true;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Puzzle")) {
                lantern lant;
                if (lant = g.GetComponent<lantern>()) {
                    if (!lant.getlit()) allit = false;
                }
            }
            if (lantern.allit) {
                GameObject.Find("door").GetComponent<Animator>().SetBool("doorisopen", true);
            }
        }

        if (!lantern.allit) GameObject.Find("door").GetComponent<Animator>().SetBool("doorisopen", false);
    }

    bool isNear() {
        if (Mathf.RoundToInt(ply.transform.position.x) == transform.position.x  && Mathf.Abs(transform.position.y - ply.transform.position.y) <= 1) {
            return true;
        }
        else return false;
    }

    public bool getlit() {
        return lit;
    }
}
