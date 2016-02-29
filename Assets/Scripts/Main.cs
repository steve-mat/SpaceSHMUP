using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    public static Main S;

    public GameObject[] prefabEnemies;

    public float enemySpawnPerSecond = 0.5f;
    public float enemySpawnPadding = 1.5f;

    public float enemySpawnRate;

    public WeaponDefinition[] weaponDefinitions;
    public WeaponType[] activeWeaponTypes;

    public static Dictionary<WeaponType, WeaponDefinition> W_DEFS;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[] { WeaponType.BLASTER, WeaponType.BLASTER, WeaponType.SPREAD, WeaponType.SHIELD };

    void Awake() {

        S = this;

        Utils.SetCameraBounds(Camera.main);

        enemySpawnRate = 1f / enemySpawnPerSecond;

        Invoke("SpawnEnemy", enemySpawnRate);

        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions) {
            W_DEFS[def.type] = def;
        }

    }

    void Start() {

        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for(int i = 0; i < weaponDefinitions.Length; i++) {
            activeWeaponTypes[i] = weaponDefinitions[i].type;
        }

    }

    public void SpawnEnemy() {

        int randomIndex = Random.Range(0, prefabEnemies.Length);
        GameObject enemy = Instantiate(prefabEnemies[randomIndex]);

        Vector3 pos = Vector3.zero;
        float xMin = Utils.camBounds.min.x + enemySpawnPadding;
        float xMax = Utils.camBounds.max.x - enemySpawnPadding;

        pos.x = Random.Range(xMin, xMax);
        pos.y = Utils.camBounds.max.y + enemySpawnPadding;
        enemy.transform.position = pos;

        Invoke("SpawnEnemy", enemySpawnRate);

    }

    public void DelayedRestart(float delay) {
        Invoke("Restart", delay);
    }

    public void Restart() {
        SceneManager.LoadScene("Scene0");
    }

    public static WeaponDefinition GetWeaponDefinition(WeaponType wepType) {

        if(W_DEFS.ContainsKey(wepType)) {
            return W_DEFS[wepType];
        }

        return new WeaponDefinition();

    }

    internal void ShipDestroyed(Enemy enemy) {

        if(Random.value <= enemy.powerUpDropChance) {
            int i = Random.Range(0, powerUpFrequency.Length);
            WeaponType powerUpType = powerUpFrequency[i];

            GameObject go = Instantiate(prefabPowerUp);
            PowerUp powerUp = go.GetComponent<PowerUp>();

            powerUp.SetType(powerUpType);

            powerUp.transform.position = enemy.transform.position;
        }

    }
}
