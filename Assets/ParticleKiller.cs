using UnityEngine;
using System.Collections;

public class ParticleKiller : MonoBehaviour {

    ParticleSystem rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this != null) {
            if (!rend.loop && rend.particleCount <= 0) {
                GameObject.Destroy(this.gameObject);
            }
        }
	}
}
