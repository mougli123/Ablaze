using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    bool grounded, jumping, bursted, ceilinged, fly;

    float dir;

    float yvel, xvel, flytime, burstime;
    GameObject fireBurst, burstCandle;

    Vector2 startposition;

    // Use this for initialization
    void Start() {
        yvel = 0f;
        xvel = 0f;
        dir = 1f;
        startposition = transform.position;
        grounded = false;
        jumping = false;
    }

    // Update is called once per frame
    void Update() {
        RaycastHit2D grnd;
        grounded = false;

        // If Not Bursting Forward
        if (burstime <= 0f) {
            var i = -0.4f;    // Iterator value
            do {
                // Raycast Downward to find ground
                grnd = Physics2D.Raycast(transform.position + new Vector3(i, 0, 0), Vector2.down, 0.5f);
                grounded = (grnd.collider != null);                                                         
                i += 0.4f;
            }
            while (i <= 0.4f && !grounded);

            // If on the ground
            if (grounded) {
                yvel = 0;
                burstime = 0f;
                flytime = 0f;
                jumping = false;
                fly = false;

                // If the distance to the ground is less than center point, become centered above collider.
                if (grnd.distance < 0.5f) {
                    transform.position = new Vector2(transform.position.x, grnd.collider.transform.position.y + 1f);
                }
                if ((flytime <= 0 || burstime <= 0) && !bursted) {
                    if (fireBurst != null) {
                        fireBurst.GetComponent<ParticleSystem>().loop = false;
                    }
                    if (burstCandle != null) {
                        burstCandle.GetComponent<ParticleSystem>().loop = false;
                    }
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
                    if (ceiling.collider != null) {
                        yvel = 0;
                        ceilinged = true;
                    }
                    else {
                        ceilinged = false;
                        yvel = yvel * 0.9f;
                    }
                }
                else {
                    yvel = yvel * 1.098f;
                }
            }
        }

        float grndmod;
        if (!jumping) {
            grndmod = grounded ? 1f : 2.5f;
        }
        else grndmod = 1.25f;
        if (bursted) grndmod *= 2f;
        if (fly) grndmod *= 3f;

        float speed = 0f;
        if (burstime <= 0f) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                dir = -1f;
                speed = 3f;
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                dir = 1f;
                speed = 3f;
            }
        }
        else {
            yvel = 0f;
            burstime -= Time.deltaTime;
        }

        RaycastHit2D movechek = Physics2D.Raycast(transform.position, Vector2.right * dir, 0.5f);
        if (movechek.collider != null) {
            transform.position = new Vector2(movechek.collider.transform.position.x - dir, transform.position.y);
            xvel = 0;
        }
        else {
            if (burstime <= 0) {
                xvel = dir * speed / grndmod;
            }
        }

        // When Jump is pressed
        if (Input.GetKeyDown(KeyCode.X) && grounded) {
            jumping = true;
            yvel = 13.5f;
            if (flytime > 0f) {
                burstCandle.GetComponent<ParticleSystem>().startLifetime = 0.25f;
            }
        }

        // When Jump is released while in the air
        if (Input.GetKeyUp(KeyCode.X) && !grounded) {
            if (yvel > 0f) {
                yvel = 0f;
            }
            if (bursted) {
                burstime = 0.10f;
                xvel = 35f*dir;
                bursted = false;
                Debug.Log("Whoosh, dir = " + dir);
            }
            if (flytime > 0f) {
                burstCandle.GetComponent<ParticleSystem>().startLifetime = 0.15f;
            }
        }

        // While holding down Jump
        if (Input.GetKey(KeyCode.X)) {
            if (yvel <= 0f && flytime <= 0f && bursted) {
                flytime = 0.5f;
                bursted = false;
                fly = true;
                burstCandle = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFirePink"), transform.position, Quaternion.Euler(-90f, 0, 0), this.transform) as GameObject;

                ParticleSystem bcps = burstCandle.GetComponent<ParticleSystem>();
                bcps.simulationSpace = ParticleSystemSimulationSpace.Local;
                bcps.startLifetime = 0.25f;
                bcps.GetComponent<ParticleSystem>().startSpeed = 10f;
            }
            if (flytime > 0f) {
                yvel = ceilinged ? 0f : 5f;
                flytime -= Time.deltaTime;
                if (flytime <= 0f) {
                    fireBurst.GetComponent<ParticleSystem>().loop = false;
                    burstCandle.GetComponent<ParticleSystem>().loop = false;
                }
            }
        }

        // If Burst is pressed while grounded and not bursted, begin burst mode
        if (Input.GetKeyDown(KeyCode.Z) && grounded && !bursted) {
            fireBurst = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFireBurstPink"), transform.position, Quaternion.Euler(-90f, 0, 0), this.transform) as GameObject;
            bursted = true;
            Debug.Log("Bursted");
        }

        transform.position = new Vector2(transform.position.x + xvel * Time.deltaTime, transform.position.y + yvel * Time.deltaTime);

        if (transform.position.y < -500f) {
            transform.position = startposition;
        }
    }
}