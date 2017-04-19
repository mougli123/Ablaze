using UnityEngine;
using System.Collections;

public class lantern : MonoBehaviour {

    static bool allit;
    Player ply;

    int numlanterns;

    bool lit;
    string litcolor;

    float littime;

    GameObject fir;

    // Use this for initialization
    void Start() {
        lit = false;
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        numlanterns = 0;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Puzzle")) {
            lantern lant;
            if (lant = g.GetComponent<lantern>()) {
                numlanterns += 1;
            }
        }
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
                fir = frobj;
                littime = 5f * numlanterns;
            }
            else {
                if (litcolor != color) {
                    fir.GetComponent<ParticleSystem>().loop = false;
                    frobj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFire" + color), transform.position, Quaternion.Euler(-90f, 0, 0), transform) as GameObject;
                    frobj.name = "SpriteLantFire" + color;
                    frobj.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                    fir = frobj;
                    litcolor = color;
                    lit = true;
                }
                littime = 5f * numlanterns;
            }

            allit = true;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Puzzle")) {
                lantern lant;
                if (lant = g.GetComponent<lantern>()) {
                    if (!lant.getlit()) allit = false;
                }
            }
            if (lantern.allit) {
                GameObject dr = GameObject.Find("door");
                dr.GetComponent<Animator>().SetBool("doorisopen", true);
                dr.GetComponent<Door>().open = true;
            }
        }

        if (!lantern.allit) {
            GameObject dr = GameObject.Find("door");
            dr.GetComponent<Animator>().SetBool("doorisopen", false);
            dr.GetComponent<Door>().open = false;
        }

        littime -= Time.fixedDeltaTime;
        if (littime <= 0f && lit) {
            lit = false;
            allit = false;
            if (fir) {
                fir.GetComponent<ParticleSystem>().loop = false;
            }
            
        }
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
