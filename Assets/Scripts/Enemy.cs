using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour {

    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int scoreForDestroying = 100;

    public int showDamageForFrames = 2;
    public float powerUpDropChance = 1f;

    public Bounds bounds;
    public Vector3 boundsCenterOffset;

    public Color[] originalColors;
    public Material[] materials;
    public int remainingDamageFrames = 0;

    void Awake() {

        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for(int i = 0; i < materials.Length; i++) {
            originalColors[i] = materials[i].color;
        }

        InvokeRepeating("CheckOffscreen", 0f, 2f);

    }

    void Update() {

        Move();

        if(remainingDamageFrames > 0) {
            remainingDamageFrames--;
            if(remainingDamageFrames == 0) {
                UnShowDamage();
            }
        }

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

    void OnCollisionEnter(Collision coll) {
        
        GameObject other = coll.gameObject;
        switch(other.tag) {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                bounds.center = transform.position + boundsCenterOffset;
                if(bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, BoundsTest.OFFSCREEN) != Vector3.zero) {
                    Destroy(other);
                    break;
                }

                health = health - Main.W_DEFS[p.type].damageOnHit;
                ShowDamage();
                if(health <= 0) {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }

    }

    private void ShowDamage() {

        foreach(Material m in materials) {
            m.color = Color.red;
        }
        remainingDamageFrames = showDamageForFrames;

    }

    private void UnShowDamage() {

        for(int i = 0; i < materials.Length; i++) {
            materials[i].color = originalColors[i];
        }

    }
}
