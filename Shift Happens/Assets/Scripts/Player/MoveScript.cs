using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveScript : MonoBehaviour 
{
	public SwitchCharacterScript switchCharacterScript;

	// Move speed variable available to set in Inspector
	public float humanMoveSpeed = 3f;
	public float humanDashSpeed = 3f;
	public float alienMoveSpeed = 5f;
	public float alienDashSpeed = 5f;
	public float robotMoveSpeed = 1f;
	public float robotDashSpeed = 1.5f;
	public float moveSpeed;
	public float dfltSpeedHuman;
	public float dfltSpeedAlien;
	public float dfltSpeedRobot;
	public float dfltDashHuman;
	public float dfltDashAlien;
	public float dfltDashRobot;

	float dashSpeed;
	float dash = 1;

	float dashTime = 0;


	Animator animator;
	public Rigidbody2D rb;
	public Camera cam;

	Vector2 movement;

	bool facingHorizontal, facingVertical;

    private void Start()
    {
		facingHorizontal = false;
		facingVertical = true;
		dfltSpeedHuman = humanMoveSpeed;
		dfltSpeedAlien = alienMoveSpeed;
		dfltSpeedRobot = robotMoveSpeed;
		dfltDashHuman = humanDashSpeed;
		dfltDashAlien = alienDashSpeed;
		dfltDashRobot = robotDashSpeed;

	}
    // Update is called once per frame
    void Update () 
	{
		// get input from left and right arrow keys
		movement.x = Input.GetAxis ("Horizontal");
		movement.y = Input.GetAxis("Vertical");

		// Set animator and movement speed according to selected character
		if (switchCharacterScript.getActiveAvatar() == 1)
		{
			moveSpeed = humanMoveSpeed;
			dashSpeed = humanDashSpeed;
		}
		else if (switchCharacterScript.getActiveAvatar() == 2)
		{
			moveSpeed = alienMoveSpeed;
			dashSpeed = alienDashSpeed;
		}
		else if (switchCharacterScript.getActiveAvatar() == 3)
		{
			moveSpeed = robotMoveSpeed;
			dashSpeed = robotDashSpeed;
		}
		animator = switchCharacterScript.getActiveAnimator();


		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			dash = dashSpeed;
			dashTime = 0.1f;
		}

		if (dashTime <= 0)
			dash = 1;
		else
			dashTime -= Time.deltaTime;

		// Set facing direction 
		if (movement.x != 0)
		{
			facingHorizontal = true;
			facingVertical = false;
		}

		if (movement.y != 0)
		{
			facingVertical = true;
			facingHorizontal = false;
		}
		animator.SetBool("Vertical", facingVertical);
		animator.SetBool("Horizontal", facingHorizontal);
		animator.SetFloat("Speed", movement.sqrMagnitude);
	}

	void FixedUpdate()
	{
		rb.MovePosition(rb.position + movement * moveSpeed * dash * Time.fixedDeltaTime);
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
			switch (switchCharacterScript.getActiveAvatar())
			{
				case 1:
					if (!GameObject.Find("Human").GetComponent<AudioSource>().isPlaying)
					{
						GameObject.Find("Human").GetComponent<AudioSource>().Play();
					}
					break;
				case 2:
					if (!GameObject.Find("Alien").GetComponent<AudioSource>().isPlaying)
					{
						GameObject.Find("Alien").GetComponent<AudioSource>().Play();
					}
					break;
				case 3:
					if (!GameObject.Find("Robot").GetComponent<AudioSource>().isPlaying)
					{
						GameObject.Find("Robot").GetComponent<AudioSource>().Play();
					}
					break;
				default:
					break;
			}
			
		}
			
	}

	public bool getFacingHorizontal()
    {
		return facingHorizontal;
	}

	public bool getFacingVertical()
	{
		return facingVertical;
	}

}
