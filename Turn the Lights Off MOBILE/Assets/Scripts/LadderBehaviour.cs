using System;
using UnityEngine;

public class LadderBehaviour : MonoBehaviour {
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			other.GetComponent<PlayerController>().CanClimb(true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			other.GetComponent<PlayerController>().CanClimb(false);
		}
	}
}
