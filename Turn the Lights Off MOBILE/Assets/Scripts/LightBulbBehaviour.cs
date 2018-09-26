using UnityEngine;

public class LightBulbBehaviour : MonoBehaviour
{

	public GameObject gameHandler;
	public GameObject light;
	private bool isOn;
	private float counter;
	private float timeToPoint;

	void Start()
	{
		isOn = false;
		light.SetActive(false);
		counter = 0;
		timeToPoint = 3;
	}
	
	void FixedUpdate()
	{
		if (isOn)
		{
			counter += Time.deltaTime;
			if (counter >= timeToPoint)
			{
				gameHandler.GetComponent<GameHandler>().BulbPoint();
				counter = 0;
			}	
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			TurnOff();
		}
		
		else if (other.tag.Equals("Kid"))
		{
			if (other.GetComponent<KidController>().CanMove)
			{
				TurnOn();
			}
		}
	}

	public void TurnOff()
	{
		counter = 0;
		isOn = false;
		light.SetActive(false);
	}

	public void TurnOn()
	{
		isOn = true;
		light.SetActive(true);
	}
	
}
