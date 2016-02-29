using UnityEngine;
using System.Collections;
using System;

public class Hero : MonoBehaviour {

    public static Hero S;

    public float speed = 30.0f;

    public float rollMult = -45.0f;
    public float pitchMult = 30.0f;

    Vector3 pos = Vector3.zero;
    public float _shieldLevel = 1.0f;

    public Bounds bounds;

    public GameObject lastTriggeredGO = null;

    public float gameRestartDelay = 2f;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    public Weapon[] weapons;


    void Awake() {

        S = this;
        bounds = Utils.CombineBoundsOfChildren(gameObject);

        ClearWeapons();
        weapons[0].SetType(WeaponType.BLASTER);
        
    }

    // Update is called once per frame
    void Update () {

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        pos = transform.position;
        pos.x = pos.x + xAxis * speed * Time.deltaTime;
        pos.y = pos.y + yAxis * speed * Time.deltaTime;
        transform.position = pos;

        bounds.center = transform.position;
        Vector3 offset = Utils.ScreenBoundsCheck(bounds, BoundsTest.ONSCREEN);
        if(offset != Vector3.zero) {
            pos = pos - offset;
            transform.position = pos;
        }

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        if(Input.GetAxis("Jump") == 1 && fireDelegate != null) {
            fireDelegate();
        }

    }

    void OnTriggerEnter(Collider other) {

        GameObject parentGO = Utils.FindTaggedParent(other.gameObject);
        if(parentGO != null) {
            if(parentGO == lastTriggeredGO) {
                return;
            }
            lastTriggeredGO = parentGO;

            if(parentGO.tag == "Enemy") {
                shieldLevel--;
                Destroy(parentGO);
            } else if(parentGO.tag == "PowerUp") {
                AbsorbPowerUp(parentGO);
            } else {
                Debug.Log("Triggered: " + parentGO.name);
            }
        } else {
            Debug.Log("Triggered: " + other.gameObject.name);
        }
    }

    public float shieldLevel {

        get {
            return _shieldLevel;
        }

        set {
            _shieldLevel = Mathf.Min(value, 4);
            if(value < 0) {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }

    }


    private void AbsorbPowerUp(GameObject go) {

        PowerUp pwrUp = go.GetComponent<PowerUp>();
        switch(pwrUp.type) {
            case WeaponType.SHIELD:
                shieldLevel++;
                break;
            default:
                if(pwrUp.type == weapons[0].type) {
                    Weapon wep = GetEmptyWeaponSlot();
                    if(wep != null) {
                        wep.SetType(pwrUp.type);
                    }
                } else {
                    ClearWeapons();
                    weapons[0].SetType(pwrUp.type);
                }
                break;
        }
        pwrUp.AbsorbedBy(gameObject);

    }

    private Weapon GetEmptyWeaponSlot() {
        for(int i = 0; i < weapons.Length; i++) {
            if(weapons[i].type == WeaponType.NONE) {
                return weapons[i];
            }
        }
        return null;
    }

    private void ClearWeapons() {
        foreach(Weapon wp in weapons) {
            wp.SetType(WeaponType.NONE);
        }
    }

}
