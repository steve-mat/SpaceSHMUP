using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 6.0f;
    public float fadeTime = 4f;

    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    void Awake() {



    }

    void Update() {



    }

    public void SetType(WeaponType type) {

    }

    public void AbsorbedBy(GameObject target) {

    }

    private void CheckOffScreen() {

    }


}
