using UnityEngine;

public class KidNavigator : MonoBehaviour
{

	public bool GoLeft;
	public bool GoRight;
	public bool GoUp;
	public bool GoDown;
	private string _directions;
	
	void Start ()
	{
		_directions = "";
		if (GoLeft)
		{
			_directions += "l";
		}

		if (GoRight)
		{
			_directions += "r";
		}

		if (GoUp)
		{
						// no
			_directions += "uu";
		}

		if (GoDown)
		{
			_directions += "d";
		}
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Kid"))
		{
			other.GetComponent<KidController>().NewDirection(GetValidVector());
		}
	}

	private Vector2 GetValidVector()
	{
		int index = Random.Range(0, _directions.Length);
		char dir = _directions[index];
		if (dir == 'l')
		{
			return new Vector2(-1, 0);
		}
		if (dir == 'r')
		{
			return new Vector2(1, 0);
		}
		if (dir == 'u')
		{
			return new Vector2(0, 1);
		}
		if (dir == 'd')
		{
			return new Vector2(0, -1);
		}
		return new Vector2(0, 0);
	}
}
