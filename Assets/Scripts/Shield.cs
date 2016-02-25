using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public float rotationsPerSecond = 0.1f;

    public int levelShown = 0;
    Renderer shieldMat;

	// Use this for initialization
	void Start () {

        shieldMat = GetComponent<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {

        int currentLevel = Mathf.FloorToInt(Hero.S.shieldLevel);

        if(levelShown != currentLevel) {
            levelShown = currentLevel;
            Material mat = shieldMat.material;
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }

        float rZ = (rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);

	}
}
