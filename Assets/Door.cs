using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Door : MonoBehaviour {

    public bool open;
    public string exitLevelName;

    private float transitiontime = 0f;
    private bool transitioning;

    Animator anim;
    Player ply;

	// Use this for initialization
	void Start () {
        if (!GameObject.FindGameObjectWithTag("Trickery")) {
            GameObject.Instantiate(Resources.Load("Scripting/Interface"));
        }
        anim = GameObject.FindGameObjectWithTag("Trickery").GetComponent<Animator>();

        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (open && Vector3.Distance(ply.transform.position,transform.position) <= 0.1f && ply.getGrounded()) {
            anim.SetBool("screenBlack", true);

            transitiontime = 1f;
            transitioning = true;
        }

        if (transitiontime > 0f) {
            transitiontime -= Time.fixedDeltaTime;
        }

        if (transitiontime <= 0f && transitioning) {
            SceneManager.LoadScene(exitLevelName, LoadSceneMode.Single);
        }
        
	}
}
