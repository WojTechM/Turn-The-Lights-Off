using UnityEngine;

public class ResetHandler : MonoBehaviour
{

	private ParticleSystem _ps;
	private GameObject[] _spawnPoints;

	public GameObject PowerUp;
	public GameObject[] PowerUpSpawnPoints;

	private GameHandler _gameHandler;

	void Start()
	{
		_gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
		_ps = GetComponent<ParticleSystem>();
		PowerUpSpawnPoints = GameObject.FindGameObjectsWithTag("PowerUpSpawn");
		_spawnPoints = GameObject.FindGameObjectsWithTag("KidSpawn");
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.tag.Equals("Kid")) return;
		HandleKidReset(other);
		HandlePowerUpSpawn();
		_gameHandler.UpdateScore(10);
	}

	private void HandleKidReset(Collider2D other)
	{
		_ps.Play();
		int index = Random.Range(0, _spawnPoints.Length);
		other.transform.position = _spawnPoints[index].transform.position;
		other.GetComponent<KidController>().ResetMoveVector();
	}

	private void HandlePowerUpSpawn()
	{
		int index = Random.Range(0, PowerUpSpawnPoints.Length);
		GameObject spawnPoint = PowerUpSpawnPoints[index];
		Instantiate(PowerUp, spawnPoint.transform.position, spawnPoint.transform.rotation);
	}
}
