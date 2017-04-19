using UnityEngine;
using System.Collections;

public class Chester : MonoBehaviour {

    bool dead;

    Player ply;
    cameracontrol cam;
    Animator anim;

    Vector3 strtpos;

    float yvel, xvel, dir = 1f, rolltime, deathtime, spawntime;

    bool jumped, grounded;

    // Use this for initialization
    void Start () {
        ply = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameracontrol>();
        anim = this.GetComponent<Animator>();

        strtpos = transform.position;
        dead = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!dead) {
            anim.SetBool("dead", false);
            if (ply.isAlive()) {
                if (Vector3.Distance(ply.transform.position, transform.position) < 1f) {
                    if (!ply.getBursting()) {
                        ply.kill();
                        cam.shaketime = 0.25f;
                    }
                    else {
                        dead = true;
                        deathtime = 1f;
                        ply.refreshBurst();
                        anim.SetBool("dead", true);
                        cam.shaketime = 0.25f;
                    }
                }

                if (rolltime <= 0f) {
                    dir = transform.position.x < ply.transform.position.x ? -1 : 1;
                }
                anim.SetBool("left", dir < 0 ? false : true);

                RaycastHit2D grnd;
                grounded = false;

                {
                    var i = -0.4f;    // Iterator value
                    do {
                        // Raycast Downward to find ground
                        grnd = Physics2D.Raycast(transform.position + new Vector3(i, 0, 0), Vector2.down, 0.5f);
                        grounded = (grnd.collider != null);
                        i += 0.4f;
                    }
                    while (i <= 0.4f && !grounded);
                }
                
                // If on the ground
                if (grounded) {
                    yvel = 0;
                    if (grnd.distance < 0.5f) {
                        transform.position = new Vector2(transform.position.x, grnd.collider.transform.position.y + 1f);
                    }
                }
                else {
                    // Calculations depend on yvel not being 0
                    if (Mathf.Round(yvel) == 0) yvel = -1f;
                    // If we're ascending, check for hitting a ceiling
                    else if (yvel > 0) {
                        RaycastHit2D ceiling;
                        var i = -0.45f;
                        do {
                            ceiling = Physics2D.Raycast(transform.position + new Vector3(i, 0, 0), Vector2.up, 0.55f);
                            i += 0.45f;
                        }
                        while (i <= 0.5f && ceiling.collider == null);
                        if (ceiling.collider != null) {
                            yvel = 0;
                        }
                        else {
                            yvel = yvel * 0.9f;
                        }
                    }
                    // Fixed gravity multiplication for falling
                    else {
                        yvel = yvel * 1.098f;
                    }
                }

                if (rolltime <= 0f) {
                    if (jumped) {
                        anim.SetBool("rolling", false);
                        xvel = 0f;
                        yvel = 7.5f;
                        jumped = false;
                    }
                    if (transform.GetChild(0).GetComponent<Renderer>().isVisible) {
                        if (ply.getGrounded() && Mathf.Abs(ply.transform.position.y - transform.position.y) <= 0.65f && grounded) {
                            anim.SetBool("rolling", true);
                            rolltime = 5f;
                            yvel = 7.5f;
                        }
                    }
                }
                else {
                    if (grounded) {
                        jumped = true;
                    }
                    if (jumped) {
                        RaycastHit2D movechek = Physics2D.Raycast(transform.position, Vector2.right * -dir, 0.5f);
                        if (movechek.collider != null) {
                            transform.position = new Vector2(movechek.collider.transform.position.x + dir, transform.position.y);
                            dir *= -1;
                        }
                        xvel = -dir * 6f;
                        rolltime -= Time.fixedDeltaTime;
                    }
                }


                transform.position = new Vector2(transform.position.x + xvel * Time.fixedDeltaTime, transform.position.y + yvel * Time.fixedDeltaTime);
            }

        }
        else {
            deathtime -= Time.fixedDeltaTime;
            if (deathtime < 0) {
                transform.position = new Vector2(0f, -1000f);
                anim.SetBool("dead", false);
            }
        }

        if (GetComponent<EnemyGeneric>().revive) {
            revive();
            GetComponent<EnemyGeneric>().revive = false;
        }
    }

    void revive() {
        dead = false;
        rolltime = 0f;
        jumped = false;
        anim.SetBool("rolling", false);
        yvel = 0f;
        xvel = 0f;
        transform.position = strtpos;
    }
}
