using System.Collections;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
	public float SpeedBoost;
	public float BoostDuration;
	public GameObject particle;
	public GameObject[] Kids;

	void Update()
	{
		if (Kids == null || Kids.Length == 0)
		{
			Kids = GameObject.FindGameObjectsWithTag("Kid");
			foreach(GameObject kid in Kids)
			{
				Physics2D.IgnoreCollision(kid.GetComponent<Collider2D>(), gameObject.GetComponent<CircleCollider2D>());
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			StartCoroutine(PickUp(other));
		}
	}

	IEnumerator PickUp(Collider2D player)
	{
		Instantiate(particle, transform.position, transform.rotation);

		PlayerController pc = player.GetComponent<PlayerController>();
		pc.Speed += SpeedBoost;

		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
		
		yield return new WaitForSeconds(BoostDuration);

		pc.Speed -= SpeedBoost;
		Destroy(gameObject);

	}
}
