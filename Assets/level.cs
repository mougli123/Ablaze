using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class level : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    foreach (GameObject j in GameObject.FindGameObjectsWithTag("Block")) {
            j.transform.position = new Vector2(Mathf.RoundToInt(j.transform.position.x), Mathf.RoundToInt(j.transform.position.y));
            j.gameObject.transform.parent = this.transform;
        }
	}
}
