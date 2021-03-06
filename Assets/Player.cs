﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player : MonoBehaviour {

    public bool respawning;

    string color = "Pink";
    bool grounded, jumping, bursted, ceilinged, fly, dead;

    int killcount;

    float dir;
    float transitiontime;

    float yvel, xvel, flytime, burstime;
    cameracontrol cam;
    GameObject fireBurst, burstCandle;
    level lvl;

    Animator anim, UIanim;

    Vector2 startposition;

    // Use this for initialization
    void Start() {
        yvel = 0f;
        xvel = 0f;
        dir = 1f;
        startposition = transform.position;
        anim = this.GetComponent<Animator>();
        UIanim = GameObject.FindGameObjectWithTag("Trickery").GetComponent<Animator>();
        lvl = GameObject.FindGameObjectWithTag("Level").GetComponent<level>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameracontrol>();
        killcount = 0;
        grounded = false;
        jumping = false;
    }

    // Update is called once per frame
    void Update() {
        if (!dead) {
            RaycastHit2D grnd;
            grounded = false;

            if (transform.FindChild("SpriteFirePlrBurst" + color)) {
                fireBurst = transform.FindChild("SpriteFirePlrBurst" + color).gameObject;
            }

            if (!transform.FindChild("SpriteFire" + color)) {
                GameObject gobj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFire" + color), transform.position, Quaternion.Euler(-90f, 0, 0), this.transform) as GameObject;
                gobj.name = "SpriteFire" + color;
            }

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
                    anim.SetBool("boost", false);

                    // If the distance to the ground is less than center point, become centered above collider.
                    if (grnd.distance < 0.5f) {
                        transform.position = new Vector2(transform.position.x, grnd.collider.transform.position.y + 1f);
                    }
                    // If burst has finished and we're on the ground, turn off the burst effect
                    if ((flytime <= 0 || burstime <= 0) && !bursted) {
                        if (fireBurst != null) {
                            fireBurst.GetComponent<ParticleSystem>().loop = false;
                        }
                        if (burstCandle != null) {
                            burstCandle.GetComponent<ParticleSystem>().loop = false;
                        }
                    }
                }
                // If not on the ground
                else {
                    // Calculations depend on yvel not being 0
                    if (Mathf.Round(yvel) == 0) yvel = -1f;
                    // If we're ascending, check for hitting a ceiling
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
                    // Fixed gravity multiplication for falling
                    else {
                        yvel = yvel * 1.098f;
                    }
                }
            }

            /*
            Speed modifier for being on the ground or in the air
            Our speed if we fall off a ledge is severely inhibited, good for stairs
            If jumping our horizontal speed is slightly inhibited as air control is lost
            */
            float grndmod;
            if (!jumping) {
                grndmod = grounded ? 1f : 2.5f;
            }
            else {
                grndmod = 1.25f;
                if (yvel < 0) grndmod = 2.5f;
            }
            if (bursted) grndmod *= 2f;
            if (fly) grndmod *= 3f;

            // Doing the jump
            float speed = 0f;
            if (!jumping || yvel <= 0f || flytime > 0f) {
                if (burstime <= 0f) {
                    if (Input.GetKey(KeyCode.LeftArrow)) {
                        dir = -1f;
                        if (transform.position.x > lvl.lefp) speed = jumping ? 2.5f : 3f;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow)) {
                        dir = 1f;
                        if (transform.position.x < lvl.rgtp) speed = jumping ? 2.5f : 3f;
                    }
                }
                else {
                    yvel = 0f;
                    burstime -= Time.fixedDeltaTime;
                }
            }
            else {
                speed = Mathf.Abs(xvel) > 0 ? 3f : 0f;
                if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
                    speed /= 2f;
                }
            }

            // Check for legal movement by casting forward
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
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFireBurst" + color), transform.position, Quaternion.Euler(-90f, 0, 0), this.transform) as GameObject;
                obj.GetComponent<ParticleSystem>().loop = false;
                yvel = 7.5f;
                if (flytime > 0f) {
                    burstCandle.GetComponent<ParticleSystem>().startLifetime = 0.25f;
                }
            }

            // When Jump is released while in the air
            if ((Input.GetKeyUp(KeyCode.X) || Input.GetKeyDown(KeyCode.X)) && !grounded) {
                if (yvel > 0f) {
                    yvel = 0f;
                }
                if (bursted || flytime > 0f) {
                    burstime = 0.125f;
                    if (flytime > 0f) {
                        burstime = flytime / 1.15f;
                        flytime = 0f;
                    }
                    xvel = 20f * dir;
                    bursted = false;
                    anim.SetBool("boost", true);
                    //Debug.Log("Whoosh, dir = " + dir);
                }
                if (flytime > 0f) {
                    burstCandle.GetComponent<ParticleSystem>().startLifetime = 0.15f;
                }
            }

            // While holding down Jump
            if (Input.GetKey(KeyCode.X)) {
                if (Mathf.Round(yvel) == 0f && flytime <= 0f && bursted) {
                    flytime = 0.35f;
                    bursted = false;
                    fly = true;
                    burstCandle = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFire" + color), transform.position, Quaternion.Euler(-90f, 0, 0), this.transform) as GameObject;

                    ParticleSystem bcps = burstCandle.GetComponent<ParticleSystem>();
                    bcps.simulationSpace = ParticleSystemSimulationSpace.Local;
                    bcps.startLifetime = 0.25f;
                    bcps.GetComponent<ParticleSystem>().startSpeed = 10f;
                }
                if (flytime > 0f) {
                    yvel = ceilinged ? 0f : 8f;
                    flytime -= Time.fixedDeltaTime;
                    if (flytime <= 0f) {
                        if (fireBurst != null) fireBurst.GetComponent<ParticleSystem>().loop = false;
                        if (burstCandle != null) burstCandle.GetComponent<ParticleSystem>().loop = false;
                    }
                }
            }

            // If Burst is pressed while grounded and not bursted, begin burst mode
            // If Burst is pressed while in burst mode, burst forward
            if (Input.GetKeyDown(KeyCode.Z)) {
                if (grounded && !bursted) {
                    fireBurst = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFireBurst" + color), transform.position, Quaternion.Euler(-90f, 0, 0), this.transform) as GameObject;
                    fireBurst.name = "SpriteFirePlrBurst" + color;
                    bursted = true;
                    //Debug.Log("Bursted");
                }
                else if (bursted && grounded) {
                    burstime = 0.1f;
                    xvel = 25f * dir;
                    bursted = false;
                }
            }

            // Animation Control Settings
            if (yvel > 0) {
                anim.SetInteger("ydir", 1);
            }
            else if (yvel < 0) {
                anim.SetInteger("ydir", -1);
            }
            else anim.SetInteger("ydir", 0);

            if (burstime < 0f) {
                anim.SetBool("boost", false);
            }

            if (grounded && Mathf.Abs(xvel) > 0) {
                anim.SetBool("walking", true);
            }
            else anim.SetBool("walking", false);

            transform.localScale = new Vector3(-dir, 1, 1);

            if ((dir == -1 && transform.position.x < lvl.lefp) || (dir == 1 && transform.position.x > lvl.rgtp)) xvel = 0f;
            transform.position = new Vector2(transform.position.x + xvel * Time.fixedDeltaTime, transform.position.y + yvel * Time.fixedDeltaTime);

            if (transform.position.y < -500f) {
                transform.position = startposition;
                xvel = 0f;
                yvel = 0f;
                grounded = false;
                bursted = false;
            }
        }
        // Kill the player in a burst of flames
        else {
            anim.SetBool("dead", true);
            GameObject frobj;
            

            if (transform.FindChild("SpriteFire" + getColor())) {
                frobj = transform.FindChild("SpriteFire" + getColor()).gameObject;
                frobj.GetComponent<ParticleSystem>().loop = false;

                if (!GameObject.Find("deathfire0")) {
                    for (int i = 0; i < 3; i++) {
                        frobj = GameObject.Instantiate(Resources.Load<GameObject>("Particles/SpriteFireBurst" + color), transform.position, Quaternion.Euler(-90f, 0, 0)) as GameObject;
                        frobj.name = "deathfire" + i;
                    }
                }
            }
            else {
                if (GameObject.Find("deathfire0")) {
                    for (int i = 0; i < 3; i++) {
                        frobj = GameObject.Find("deathfire" + i);
                        frobj.GetComponent<ParticleSystem>().loop = false;
                    }
                }
                else {
                    transform.position = new Vector2(1000f, 1000f);
                    UIanim.SetBool("screenBlack", true);
                    transitiontime -= Time.fixedDeltaTime;
                    respawning = true;
                    if (transitiontime <= 0f) {
                        respawning = false;
                        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Foe")) {
                            g.GetComponent<EnemyGeneric>().revive = true;
                        }
                        UIanim.SetBool("screenBlack", false);
                        revive();
                    }
                }
            }
        }
    }

    public void kill() {
        dead = true;
        transitiontime = 1f;
    }

    void revive() {
        dead = false;
        anim.SetBool("dead", false);
        transform.position = startposition;
    }

    public bool getBursting() {
        if (bursted || burstime > 0f || flytime > 0f) {
            return true;
        }
        else return false;
    }

    public void setColor(string clr) {
        color = clr;
        if (fireBurst != null) fireBurst.GetComponent<ParticleSystem>().loop = false;
    }

    public void setKills(int k) {
        killcount = k;
    }

    public string getColor() {
        return color;
    }

    public bool getGrounded() {
        return grounded;
    }
    
    public bool getJumping() {
        return jumping;
    }

    public bool isAlive() {
        return !dead;
    }

    public int getKills() {
        return killcount;
    }

    public float getYVel() {
        return yvel;
    }

    public void refreshBurst() {
        burstime += 0.075f;
    }
}