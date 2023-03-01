using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {


     LineRenderer lineRenderer;
    float counter;
    float dist;

    public Transform origin;
    public Transform destination;

    public float lineDropSpeed = 0.0000000001f;


	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetWidth(.025f, .025f);
        lineRenderer.SetPosition(1, destination.position);
    }
	
}
