using UnityEngine;

public class KidController : MonoBehaviour
{
	public bool CanMove;
	public float StunDuration = 1f;
	public float Speed;
	private float _stunTimer;
	private GameObject _stunnedBy;
	private Rigidbody2D _rb;
	private Vector2 _vector2;
	private readonly Vector3 _zAxis = new Vector3(0, 0, 1);
	private bool _isGrabbed;
	private Animator _animator;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_stunnedBy = GameObject.Find("Player1 (Granny)");
		_stunTimer = StunDuration + 1;
		CanMove = true;
		_rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		_stunTimer += Time.deltaTime;
		if (CanMove)
		{
			_rb.velocity = _vector2 * Speed;
		}
		else
		{
			if (_isGrabbed)
			{
				RotateAroundPlayer();
			}

			if (_stunTimer > StunDuration)
			{
				CanMove = true;
				_isGrabbed = false;
			}
		}

		_animator.SetBool("isStunned", !CanMove);
	}

	private void RotateAroundPlayer()
	{
		_rb.velocity = _vector2 * 0;
		transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
			_stunnedBy.transform.position, _stunnedBy.GetComponent<PlayerController>().Speed * Time.deltaTime);
		transform.RotateAround(_stunnedBy.transform.position, _zAxis, 3);
	}

	public void NewDirection(Vector2 newVector)
	{
		_vector2 = newVector;
	}

	public void Grabbed(GameObject grabbedBy)
	{
		if (!CanBeGrabbed()) return;
		CanMove = false;
		_isGrabbed = true;
		_stunTimer = -1;
		_stunnedBy = grabbedBy;
	}

	public void Thrown(GameObject thrownBy)
	{
		var v = thrownBy.GetComponent<PlayerController>().GetThrowAngle();
		_isGrabbed = false;
		CanMove = false;
		_stunTimer = 0;
		_rb.AddForce(v * 1000);
	}

	public void ResetMoveVector()
	{
		_vector2 = new Vector2(1, 0);
	}

	private bool CanBeGrabbed()
	{
		return _stunTimer > StunDuration + 1;
	}
}
