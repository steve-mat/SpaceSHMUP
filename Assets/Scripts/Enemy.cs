using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour {

    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int scoreForDestroying = 100;


    public Bounds bounds;
    public Vector3 boundsCenterOffset;

    void Awake() {

        InvokeRepeating("CheckOffscreen", 0f, 2f);

    }

    void Update() {

        Move();

    }

    public virtual void Move() {

        Vector3 tempPos = pos;
        tempPos.y = tempPos.y - speed * Time.deltaTime;
        pos = tempPos;

    }

    public Vector3 pos {

        get {
            return transform.position;
        }

        set {
            transform.position = value;
        }
    }

    private void CheckOffscreen() {

        if(bounds.size == Vector3.zero) {
            bounds = Utils.CombineBoundsOfChildren(gameObject);
            boundsCenterOffset = bounds.center - transform.position;
        }

        bounds.center = transform.position + boundsCenterOffset;

        Vector3 offset = Utils.ScreenBoundsCheck(bounds, BoundsTest.OFFSCREEN);
        if(offset != Vector3.zero && offset.y < 0) {
            Destroy(this.gameObject);
        }

    }
}
