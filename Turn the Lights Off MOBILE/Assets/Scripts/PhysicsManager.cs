using UnityEngine;

public class PhysicsManager : MonoBehaviour {
	
	
	public Collider2D[] CollidersInoringEachOther;
	
	void Start () {
		for (int i = 0; i < CollidersInoringEachOther.Length - 1; i++)
		{
			for (int j = i + 1; j < CollidersInoringEachOther.Length; j++)
			{
				Physics2D.IgnoreCollision(CollidersInoringEachOther[i], CollidersInoringEachOther[j]);
			}
		}
	}
}
