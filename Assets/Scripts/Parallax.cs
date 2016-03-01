using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

    public GameObject pointOfInterest;
    public GameObject[] panels;
    public float scrollSpeed = -30f;
    public float motionMult = 0.25f;

    private float panelHeight;
    private float depth;

    void Start() {

        panelHeight = panels[0].transform.localScale.y;
        depth = panels[0].transform.position.x;

        panels[0].transform.position = new Vector3(0, 0, depth);
        panels[1].transform.position = new Vector3(0, panelHeight, depth);

    }

    void Update() {

        float tX = 0f;
        float tY = 0f;
        tY = Time.time * scrollSpeed % panelHeight + (panelHeight * 0.5f);

        if(pointOfInterest != null) {
            tX = -pointOfInterest.transform.position.x * motionMult;
        }

        panels[0].transform.position = new Vector3(tX, tY, depth);
        if(tY >= 0) {
            panels[1].transform.position = new Vector3(tX, tY - panelHeight, depth);
        } else {
            panels[1].transform.position = new Vector3(tX, tY + panelHeight, depth);
        }
    }


}
