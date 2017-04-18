using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[System.Serializable]
public class level : MonoBehaviour {

    public float lowp, lefp, rgtp, hihp;

	// Use this for initialization
	void Start () {
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
    }
	
	// Update is called once per frame
	void Update () {
        if (!Application.isPlaying) {
            transform.position = new Vector2(0, 0);
            Start();
        }
	}
}
