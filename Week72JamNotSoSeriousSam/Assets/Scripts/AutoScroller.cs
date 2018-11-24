using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Autoscrolls background
/// </summary>
public class AutoScroller : MonoBehaviour {

    public float speed = 3;

    void FixedUpdate() {
        transform.position -= new Vector3(speed / 100, 0, 0);
    }
}
