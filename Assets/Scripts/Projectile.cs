using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour {

    [SerializeField] private WeaponType _type;

    private Renderer rend;
    private Collider coll;

    void Awake() {

        rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
        InvokeRepeating("CheckOffscreen", 2f, 2f);

    }

    public WeaponType type {

        get {
            return _type;
        }

        set {
            SetType(value);
        }

    }

    private void SetType(WeaponType eType) {

        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;

    }

    private void CheckOffscreen() {

        if(Utils.ScreenBoundsCheck(coll.bounds, BoundsTest.OFFSCREEN) != Vector3.zero) {
            Destroy(this.gameObject);
        }

    }
}
