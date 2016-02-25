using UnityEngine;
using System.Collections;


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

	



}
