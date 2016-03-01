using UnityEngine;
using System.Collections;

[System.Serializable]
public class Part {

    public string name;
    public float health;
    public string[] protectedBy;

    public GameObject go;
    public Material mat;

}

public class Enemy_4 : Enemy {

    public Vector3[] points;
    public float timeStart;
    public float duration = 4f;

    public Part[] parts;

    void Start() {

        points = new Vector3[2];
        points[0] = pos;
        points[1] = pos;

        InitMovement();

        Transform t;
        foreach(Part part in parts) {
            t = transform.Find(part.name);
            if(t != null) {
                part.go = t.gameObject;
                part.mat = part.go.GetComponent<Renderer>().material;
            }
        }

    }

    private void InitMovement() {

        Vector3 p1 = Vector3.zero;

        float esp = Main.S.enemySpawnPadding;

        Bounds cBounds = Utils.camBounds;

        p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

        points[0] = points[1];
        points[1] = p1;

        timeStart = Time.time;
    }

    public override void Move() {

        float u = (Time.time - timeStart) / duration;
        if(u >= 1) {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);

        pos = (1 - u) * points[0] + u * points[1];

        base.Move();
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

                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part partHit = FindPart(goHit);
                if(partHit == null) {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    partHit = FindPart(goHit);
                }

                if(partHit.protectedBy != null) {
                    foreach(string s in partHit.protectedBy) {
                        if(!Destroyed(s)) {
                            Destroy(other);
                            return;
                        }
                    }
                }

                partHit.health = partHit.health - Main.W_DEFS[p.type].damageOnHit;
                ShowLocalizedDamage(partHit.mat);
                if(partHit.health <= 0) {
                    partHit.go.SetActive(false);
                }

                bool allDestroyed = true;
                foreach(Part part in parts) {
                    if(!Destroyed(part)) {
                        allDestroyed = false;
                        break;
                    }
                }

                if(allDestroyed == true) {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }

    }

    private bool Destroyed(GameObject go) {
        return Destroyed(FindPart(go));
    }

    private bool Destroyed(Part part) {

        if(part == null) {
            return true;
        }
        return part.health <= 0;

    }

    private bool Destroyed(string s) {
        return Destroyed(FindPart(s));
    }

    private Part FindPart(GameObject go) {
        foreach(Part part in parts) {
            if(part.go == go) {
                return part;
            }
        }
        return null;
    }

    private Part FindPart(string s) {
        foreach(Part part in parts) {
            if(part.name == s) {
                return part;
            }
        }
        return null;
    }

    private void ShowLocalizedDamage(Material m) {
        m.color = Color.red;
        remainingDamageFrames = showDamageForFrames;
    }
}
