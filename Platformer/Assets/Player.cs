using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private bool _SpaceDown;
	private bool _LeftDown;
	private bool _RightDown;
	private bool _LeftKill;
	private bool _RightKill;
	
	public float _JumpStrength = 300.0f;
	public float _LeftRightStrength = 20.0f;
	public float _velocityDamping = 0.85f;
	
	private int _JumpCount;
	public float _JumpScale;
	
	void Start () 
	{
		_SpaceDown = false;
		_LeftDown = false;
		_RightDown = false;
		
		_LeftKill = false;
		_RightKill = false;
		
		_JumpCount = 0;
		_JumpScale = 1.0f;
	}
	
	void Update()
	{
		
		if(Input.GetKeyDown("space") && _JumpCount < 2)
		{
			_SpaceDown = true;
			++_JumpCount;
			_JumpScale = 1.0f / (float)(_JumpCount+1);
		}
		
		_LeftDown = Input.GetKey ("left");
		_RightDown = Input.GetKey ("right");
	}
	
	void FixedUpdate () 
	{
		if(_SpaceDown)
		{
			this.GetComponent<Rigidbody>().AddExplosionForce(_JumpStrength * _JumpScale, transform.position,0.0f);
			_SpaceDown = false;
		}
		
		if(_LeftDown && !_LeftKill)
		{
			this.GetComponent<Rigidbody>().AddForce(Vector3.left * _LeftRightStrength);
		}
		
		if(_RightDown && !_RightKill)
		{
			this.GetComponent<Rigidbody>().AddForce(Vector3.right * _LeftRightStrength);
		}
		
		Vector3 vVelocity = this.GetComponent<Rigidbody>().velocity;
		vVelocity.x *= _velocityDamping;
		this.GetComponent<Rigidbody>().velocity = vVelocity;
	}	
	
	void OnCollisionStay(Collision collisionInfo) 
	{
		_RightKill = false;
		_LeftKill = false;
		
		for(int i = 0; i < collisionInfo.contacts.Length; ++i)
		{
			
			if(collisionInfo.contacts[i].normal.y >= 0.1f)
			{
				_JumpCount = 0;
				_JumpScale = 1.0f;
			}
			
			if(collisionInfo.contacts[i].normal.x > 0.1f)
			{
				_LeftKill = true;
			}
			
			if(collisionInfo.contacts[i].normal.x < -0.1f)
			{
				_RightKill = true;
			}
			
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		Application.LoadLevel ("Game");
	}
}
