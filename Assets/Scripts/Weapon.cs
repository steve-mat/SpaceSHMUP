using UnityEngine;
using System.Collections;
using System;

public enum WeaponType {
    NONE,
    BLASTER,
    SPREAD,
    PHASER,
    MISSILE,
    LASER,
    SHIELD
};

[System.Serializable]
public class WeaponDefinition {
    public WeaponType type = WeaponType.NONE;
    public string letter;                           //letter to show on the power-up
    public Color color = Color.white;               //color of Collar and power-up
    public GameObject projectilePrefab;             //prefab for the projectiles
    public Color projectileColor = Color.white;     //color of projectile
    public float damageOnHit = 0f;                  //amount of damage
    public float continuousDamage = 0f;             //DPS (Laser)
    public float delayBetweenShots = 0f;            //delay between each shot
    public float velocity = 20f;                    //speed of each shot
}

public class Weapon : MonoBehaviour {

    public static Transform PROJECTILE_ANCHOR;

    [SerializeField] private WeaponType _type = WeaponType.BLASTER;

    public WeaponDefinition def;
    public GameObject collar;
    //public Renderer collarRenderer;
    public float lastShot;

    void Awake() {

        collar = transform.Find("Collar").gameObject;

    }

    void Start() {

        //collarRenderer = collar.GetComponent<Renderer>();

        SetType(_type);

        if(PROJECTILE_ANCHOR == null) {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject parentGO = transform.parent.gameObject;
        if(parentGO.tag == "Hero") {
            Hero.S.fireDelegate = Hero.S.fireDelegate + Fire;
        }

    }

    public WeaponType type {
        get {
            return _type;
        }
        set {
            SetType(value);
        }
    }

    public void SetType(WeaponType _type) {

        this._type = _type;
        if(_type == WeaponType.NONE) {
            gameObject.SetActive(false);
            return;
        } else {
            gameObject.SetActive(true);
        }

        def = Main.GetWeaponDefinition(_type);
        collar.GetComponent<Renderer>().material.color = def.color;
        lastShot = 0f;

    }

    public Projectile MakeProjectile() {

        GameObject go = Instantiate(def.projectilePrefab);
        if(transform.parent.gameObject.tag == "Hero") {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        } else {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShot = Time.time;

        return p;
    }


    private void Fire() {

        if(gameObject.activeInHierarchy == false) {
            return;
        }

        if(Time.time - lastShot < def.delayBetweenShots) {
            return;
        }

        Projectile p;
        switch(type) {
            case WeaponType.BLASTER:
                p = MakeProjectile();
                Rigidbody pRbodyBlaster = p.GetComponent<Rigidbody>();
                pRbodyBlaster.velocity = Vector3.up * def.velocity;
                break;
            case WeaponType.SPREAD:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(-0.2f, 0.9f, 0) * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity =  new Vector3(0.2f, 0.9f, 0) * def.velocity;
                break;
        }

    }

}
