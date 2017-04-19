using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class titlescreenstart : MonoBehaviour {

    Animator anim;
    private float transitiontime = 0f;
    private bool transitioning;

    // Use this for initialization
    void Start () {
        anim = GameObject.FindGameObjectWithTag("Trickery").GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            anim.SetBool("screenBlack", true);

            transitiontime = 1f;
            transitioning = true;
        }

        if (transitiontime > 0f) {
            transitiontime -= Time.fixedDeltaTime;
        }

        if (transitiontime <= 0f && transitioning) {
            SceneManager.LoadScene("1-1", LoadSceneMode.Single);
        }
    }
}
