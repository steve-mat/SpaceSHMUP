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
    public Renderer collarRenderer;
    public float lastShot;

    void Start() {

        collar = transform.Find("Collar").gameObject;
        collarRenderer = collar.GetComponent<Renderer>();

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

    private void Fire() {
        


    }

    public WeaponType type {
        get {
            return _type;
        }
        set {
            SetType(value);
        }
    }

    private void SetType(WeaponType _type) {

        this._type = _type;
        if(type == WeaponType.NONE) {
            this.gameObject.SetActive(false);
            return;
        } else {
            this.gameObject.SetActive(true);
        }

        def = Main.GetWeaponDefinition(_type);
        collarRenderer.material.color = def.color;
        lastShot = 0f;

    }

    public Projectile MakeProjectile() {

    }
}
