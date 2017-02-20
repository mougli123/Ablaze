using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    bool grounded, jumping, bursted;

    float dir;

    public float yvel, xvel;

    Vector2 startposition;

	// Use this for initialization
	void Start () {
        yvel = 0f;
        xvel = 0f;
        dir = 1f;
        startposition = transform.position;
        grounded = false;
        jumping = false;
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit2D grnd;
        grounded = false;
        float i = -0.4f;
        do {
            grnd = Physics2D.Raycast(transform.position + new Vector3(i, 0, 0), Vector2.down, 0.5f);
            grounded = (grnd.collider != null);
            i += 0.4f;
        }
        while (i <= 0.4f && !grounded);

        if (grounded) {
            yvel = 0;
            jumping = false;
            if (grnd.distance <= 0.5f) {
                transform.position = new Vector2(transform.position.x, grnd.collider.transform.position.y + 1f);
            }
        }
        else {
            if (Mathf.Round(yvel) == 0) yvel = -1f;
            else if (yvel > 0) {
                RaycastHit2D ceiling;
                i = -0.45f;
                do {
                    ceiling = Physics2D.Raycast(transform.position + new Vector3(i, 0, 0), Vector2.up, 0.55f);
                    i += 0.45f;
                }
                while (i <= 0.5f && ceiling.collider == null);
                if (ceiling.collider != null) yvel = 0;
                else yvel = yvel * 0.9f;
            }
            else {
                yvel = yvel * 1.098f;
            }
        }

        float grndmod;
        if (!jumping) {
            grndmod = grounded ? 1f : 2.5f;
        }
        else grndmod = 1.25f;
        float speed = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) {
            dir = -1f;
            speed = 3f;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            dir = 1f;
            speed = 3f;
        }

        RaycastHit2D movechek = Physics2D.Raycast(transform.position, Vector2.right * dir, 0.5f);
        if (movechek.collider != null) {
            transform.position = new Vector2(movechek.collider.transform.position.x - dir, transform.position.y);
            xvel = 0;
        }
        else xvel = dir * speed / grndmod;

        if (Input.GetKeyDown(KeyCode.X) && grounded) {
            jumping = true;
            yvel = 12.5f;
        }
        if (Input.GetKeyUp(KeyCode.X) && !grounded && yvel > 0f) {
            yvel = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Z) && grounded && !bursted) {
            GameObject fireBurst = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFireBurstPink"), transform.position, Quaternion.Euler(-90f, 0, 0), this.transform) as GameObject;
            bursted = true;
        }

        transform.position = new Vector2(transform.position.x + xvel*Time.deltaTime, transform.position.y + yvel*Time.deltaTime);

        if (transform.position.y < -500f) {
            transform.position = startposition;
        }
	}
}