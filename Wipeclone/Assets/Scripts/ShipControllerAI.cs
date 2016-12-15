﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipControllerAI : MonoBehaviour {

	public Transform pathTransform;

	private ShipController shipController;
	private List<Vector3> path;
	private int pathIndex;
	private float slowDown = 1f;
	private GameObject debugSphere;
	// Use this for initialization
	void Awake () {
		shipController = GetComponent<ShipController> ();
		pathIndex = 2;
		Transform[] pathChilds = pathTransform.GetComponentsInChildren<Transform> ();
		path = new List<Vector3> ();
		for (int i = 0; i < pathChilds.Length; i++) {
			if (pathChilds [i] != pathTransform.transform) {
				path.Add (pathChilds [i].position);
			}
		}
		debugSphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		debugSphere.transform.localScale = Vector3.one * 10;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 directionToWaypoint = transform.InverseTransformPoint(path[pathIndex]).normalized ;

		directionToWaypoint = directionToWaypoint / directionToWaypoint.z;

		shipController.powerInput = directionToWaypoint.z * slowDown;
		shipController.turnInput = directionToWaypoint.x;

		//debugSphere.transform.position = path[pathIndex];

		Debug.DrawLine (transform.position, transform.position + directionToWaypoint * 10);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "waypoint") {
			pathIndex++;
			if (pathIndex == path.Count) {
				pathIndex = 0;
			}
			print ("We are in a waypoint, index:" + pathIndex);
		} else if (other.tag == "slow") {
			slowDown = 0.8f;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "slow") {
			slowDown = 1f;
		}
	}
}