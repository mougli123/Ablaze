using UnityEngine;
using System.Collections;

[System.Serializable]
public class salt : MonoBehaviour {

    public string color;
    Player ply;

	// Use this for initialization
	void Start () {
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (ply.getBursting() && isNear()) {
            //Debug.Log(color);
            if (color != ply.getColor()) {
                GameObject frobj;
                if (ply.transform.FindChild("SpriteFire" + ply.getColor())) {
                    frobj = ply.transform.FindChild("SpriteFire" + ply.getColor()).gameObject;
                    frobj.GetComponent<ParticleSystem>().loop = false;
                }

                if (ply.transform.FindChild("SpriteFirePlrBurst" + ply.getColor())) {
                    frobj = ply.transform.FindChild("SpriteFirePlrBurst" + ply.getColor()).gameObject;
                    frobj.GetComponent<ParticleSystem>().loop = false;
                }

                ply.setColor(color);

                if (!ply.transform.FindChild("SpriteFire" + color)) {
                    frobj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFire" + color), ply.transform.position, Quaternion.Euler(-90f, 0, 0), ply.transform) as GameObject;
                    frobj.name = "SpriteFire" + color;
                }

                if (!ply.transform.FindChild("SpriteFirePlrBurst" + color)) {
                    frobj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFireBurst" + color), ply.transform.position, Quaternion.Euler(-90f, 0, 0), ply.transform) as GameObject;
                    frobj.name = "SpriteFirePlrBurst" + color;
                }

            }
        }
	}

    bool isNear() {
        if (Mathf.RoundToInt(ply.transform.position.x) == transform.position.x && Mathf.RoundToInt(ply.transform.position.y) == transform.position.y) {
            return true;
        }
        else return false;
    }
}
