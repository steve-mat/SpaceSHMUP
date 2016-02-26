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
    private Rigidbody rbody;
    private Renderer cubeRenderer;
    private Collider cubeCollider;
    public Vector3 rotPerSecond;
    public float birthTime;

    void Awake() {

        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rbody = GetComponent<Rigidbody>();
        cubeRenderer = cube.GetComponent<Renderer>();
        cubeCollider = cube.GetComponent<Collider>();

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0f;
        vel.Normalize();
        vel = vel * Random.Range(driftMinMax.x, driftMinMax.y);
        rbody.velocity = vel;

        transform.rotation = Quaternion.identity;
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));

        InvokeRepeating("CheckOffScreen", 2.0f, 2.0f);

        birthTime = Time.time;

    }

    void Update() {

        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        if(u >= 1) {
            Destroy(this.gameObject);
            return;
        }

        if(u > 0) {
            Color c = cubeRenderer.material.color;
            c.a = 1f - u;
            cubeRenderer.material.color = c;
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

    }

    public void SetType(WeaponType type) {

        WeaponDefinition def = Main.GetWeaponDefinition(type);

        cubeRenderer.material.color = def.color;

        letter.text = def.letter;

        this.type = type;

    }

    public void AbsorbedBy(GameObject target) {
        Destroy(this.gameObject);
    }

    private void CheckOffScreen() {

        if(Utils.ScreenBoundsCheck(cubeCollider.bounds, BoundsTest.OFFSCREEN) != Vector3.zero) {
            Destroy(this.gameObject);
        }

    }


}
