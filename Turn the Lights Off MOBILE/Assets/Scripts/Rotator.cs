﻿using UnityEngine;

public class Rotator : MonoBehaviour
{
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward, -2);
	}
}
