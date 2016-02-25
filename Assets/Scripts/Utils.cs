using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum BoundsTest {
    CENTER,
    ONSCREEN,
    OFFSCREEN
};

public class Utils : MonoBehaviour {

    //========================================== BOUNDS FUNCTIONS ==========================================\\

    /// <summary>
    /// Creates bounds that encapsulate the two Bounds passed in.
    /// </summary>
    /// <param name="b0">Set of bounds</param>
    /// <param name="b1">Set of bounds to encapsulate with b0</param>
    /// <returns></returns>
    public static Bounds BoundsUnion(Bounds b0, Bounds b1) {

        if(b0.size == Vector3.zero && b1.size != Vector3.zero) {                //b0 is zero but b1 is nonzero, return nonzero - b1
            return b1;
        } else if(b0.size != Vector3.zero && b1.size == Vector3.zero) {         //b0 is nonzero but b1 is zero, return nonzero - b0
            return b0;
        } else if(b0.size == Vector3.zero && b1.size == Vector3.zero) {         //both have no size return either one
            return b0;
        }

        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return b0;

    }

    public static Bounds CombineBoundsOfChildren(GameObject go) {

        Bounds b = new Bounds(Vector3.zero, Vector3.zero);

        Renderer goRenderer = go.GetComponent<Renderer>();
        Collider goCollider = go.GetComponent<Collider>();

        if(goRenderer != null) {
            b = BoundsUnion(b, goRenderer.bounds);
        }

        if(goCollider != null) {
            b = BoundsUnion(b, goCollider.bounds);
        }

        foreach(Transform t in go.transform) {
            b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
        }

        return b;
    }

    public static Bounds camBounds {

        get {
            if(_camBounds.size == Vector3.zero) {
                SetCameraBounds();
            }
            return _camBounds;
        }

    }

    private static Bounds _camBounds;

    public static void SetCameraBounds(Camera cam = null) {

        if(cam == null) {
            cam = Camera.main;
        }

        Vector3 topLeft = new Vector3(0, 0, 0);
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);

        Vector3 boundTopLeftNear = cam.ScreenToWorldPoint(topLeft);
        Vector3 boundBottomRightFar = cam.ScreenToWorldPoint(bottomRight);

        boundTopLeftNear.z = boundTopLeftNear.z + cam.nearClipPlane;
        boundBottomRightFar.z = boundBottomRightFar.z + cam.farClipPlane;

        Vector3 center = (boundTopLeftNear + boundBottomRightFar) / 2f;
        _camBounds = new Bounds(center, Vector3.zero);

        _camBounds.Encapsulate(boundTopLeftNear);
        _camBounds.Encapsulate(boundBottomRightFar);


    }

    public static Vector3 ScreenBoundsCheck(Bounds bound, BoundsTest test = BoundsTest.CENTER) {
        return BoundsInBoundsCheck(bound, camBounds, test);
    }

    public static Vector3 BoundsInBoundsCheck(Bounds littleBound, Bounds bigBound, BoundsTest test = BoundsTest.ONSCREEN) {

        Vector3 centerOfLittleBnd = littleBound.center;

        Vector3 offset = Vector3.zero;

        switch(test) {
            case BoundsTest.CENTER:
                if(bigBound.Contains(centerOfLittleBnd)) {
                    return Vector3.zero;
                }

                if(centerOfLittleBnd.x > bigBound.max.x) {
                    offset.x = centerOfLittleBnd.x - bigBound.max.x;
                } else if(centerOfLittleBnd.x < bigBound.min.x) {
                    offset.x = centerOfLittleBnd.x - bigBound.min.x;
                }

                if(centerOfLittleBnd.y > bigBound.max.y) {
                    offset.y = centerOfLittleBnd.y - bigBound.max.y;
                } else if(centerOfLittleBnd.y < bigBound.min.y) {
                    offset.y = centerOfLittleBnd.y - bigBound.min.y;
                }

                if(centerOfLittleBnd.z > bigBound.max.z) {
                    offset.z = centerOfLittleBnd.z - bigBound.max.z;
                } else if(centerOfLittleBnd.z < bigBound.min.z) {
                    offset.z = centerOfLittleBnd.z - bigBound.min.z;
                }

                return offset;

            case BoundsTest.ONSCREEN:
                if(bigBound.Contains(littleBound.min) && bigBound.Contains(littleBound.max)) {
                    return Vector3.zero;
                }

                if(littleBound.max.x > bigBound.max.x) {
                    offset.x = littleBound.max.x - bigBound.max.x;
                } else if(littleBound.min.x < bigBound.min.x) {
                    offset.x = littleBound.min.x - bigBound.min.x;
                }

                if(littleBound.max.y > bigBound.max.y) {
                    offset.y = littleBound.max.y - bigBound.max.y;
                } else if(littleBound.min.y < bigBound.min.y) {
                    offset.y = littleBound.min.y - bigBound.min.y;
                }

                if(littleBound.max.z > bigBound.max.z) {
                    offset.z = littleBound.max.z - bigBound.max.z;
                } else if(littleBound.min.z < bigBound.min.z) {
                    offset.z = littleBound.min.z - bigBound.min.z;
                }

                return offset;

            case BoundsTest.OFFSCREEN:
                bool cMin = bigBound.Contains(littleBound.min);
                bool cMax = bigBound.Contains(littleBound.max);

                if(cMin == true || cMax == true) {
                    return Vector3.zero;
                }

                if(littleBound.min.x > bigBound.max.x) {
                    offset.x = littleBound.min.x - bigBound.max.x;
                } else if(littleBound.max.x < bigBound.min.x) {
                    offset.x = littleBound.max.x - bigBound.min.x;
                }

                if(littleBound.min.y > bigBound.max.y) {
                    offset.y = littleBound.min.y - bigBound.max.y;
                } else if(littleBound.max.y < bigBound.min.y) {
                    offset.y = littleBound.max.y - bigBound.min.y;
                }

                if(littleBound.min.z > bigBound.max.z) {
                    offset.z = littleBound.min.z - bigBound.max.z;
                } else if(littleBound.max.z < bigBound.min.z) {
                    offset.z = littleBound.max.z - bigBound.min.z;
                }

                return offset;
        }

        return Vector3.zero;

    }

    //========================================== TRANSFORM FUNCTIONS ==========================================\\

    public static GameObject FindTaggedParent(GameObject go) {

        if(go.tag != "Untagged") {
            return go;
        }

        if(go.transform.parent == null) {
            return null;
        }

        return FindTaggedParent(go.transform.parent.gameObject);

    }

    public static GameObject FindTaggedParent(Transform t) {
        return FindTaggedParent(t.gameObject);
    }



    //========================================== MATERIAL FUNCTIONS ==========================================\\
    /// <summary>
    /// Gets all materials of a given GameObject
    /// </summary>
    /// <param name="go">Game object you want the materials from</param>
    /// <returns></returns>
    public static Material[] GetAllMaterials(GameObject go) {
        List<Material> materials = new List<Material>();
        Renderer gameObjRenderer = go.GetComponent<Renderer>();
        if(gameObjRenderer != null) {
            materials.Add(gameObjRenderer.material);
        }
        foreach(Transform t in go.transform) {
            materials.AddRange(GetAllMaterials(t.gameObject));
        }
        return materials.ToArray();
    }

}
