using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[System.Serializable]
public class level : MonoBehaviour {

    public float lowp, lefp, rgtp, hihp;

    Animator anim;
    
	// Use this for initialization
	void Start () {
        if (!GameObject.FindGameObjectWithTag("Trickery")) {
            GameObject.Instantiate(Resources.Load("Scripting/Interface"));
        }
        anim = GameObject.FindGameObjectWithTag("Trickery").GetComponent<Animator>();
        anim.SetBool("screenBlack", true);

        if (GameObject.FindGameObjectWithTag("Block")) {
            GameObject[] arg = GameObject.FindGameObjectsWithTag("Block");
            rgtp = lefp = arg[0].transform.position.x;
            lowp = hihp = arg[0].transform.position.y;

            foreach (GameObject j in arg) {
                j.transform.position = new Vector2(Mathf.RoundToInt(j.transform.position.x), Mathf.RoundToInt(j.transform.position.y));
                j.gameObject.transform.parent = this.transform;
                if (j.transform.position.x < lefp) lefp = j.transform.position.x;
                if (j.transform.position.x > rgtp) rgtp = j.transform.position.x;
                if (j.transform.position.y < lowp) lowp = j.transform.position.y;
                if (j.transform.position.y > hihp) hihp = j.transform.position.y;
            }

            foreach (GameObject j in GameObject.FindGameObjectsWithTag("Puzzle")) {
                j.transform.position = new Vector2(Mathf.RoundToInt(j.transform.position.x), Mathf.RoundToInt(j.transform.position.y));
                j.gameObject.transform.parent = this.transform;
            }
        }
        anim.SetBool("screenBlack", false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!Application.isPlaying) {
            transform.position = new Vector2(0, 0);
            Start();
        }
	}
}
