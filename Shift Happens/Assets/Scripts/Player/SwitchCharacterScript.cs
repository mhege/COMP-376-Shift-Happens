using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacterScript : MonoBehaviour
{

	// referenses to controlled game objects
	public GameObject avatar1, avatar2, avatar3;
	public Animator animator1, animator2, animator3;
	Animator animator;

	public SwitchBar switchBar;
	bool shift = false;

	// variable contains which avatar is on and active
	int whichAvatarIsOn = 1;
	private float nextTime = 0;
	public float cooldownTime = 3;
	public bool switchAvailable = true;

	// Use this for initialization
	void Start()
	{
		switchBar.SetMaxCooldown(cooldownTime);
		animator = animator1;
		//animator.SetBool("Switch", shift);
		// enable first avatar and disable another one
		avatar1.gameObject.SetActive(true);
		avatar2.gameObject.SetActive(false);
		avatar3.gameObject.SetActive(false);
	}

	void Update()
	{
        if (switchAvailable)
        {
			if (Time.time > nextTime)
			{
				switchBar.SetCooldown(cooldownTime);
				if (Input.GetButtonDown("Switch"))
				{
					shift = true;
					switchBar.GetComponent<AudioSource>().Play();
					SwitchAvatar();
					nextTime = Time.time + cooldownTime;
					switchBar.SetCooldown(0);
				}
			}
			else
				switchBar.SetCooldown(cooldownTime - (nextTime - Time.time));
		}
		


		if (whichAvatarIsOn == 1)
		{
			animator1.SetBool("Shift", shift);
			animator = animator1;
			
		}
		else if (whichAvatarIsOn == 2)
		{
			animator2.SetBool("Shift", shift);
			animator = animator2;
			
		}
		else if (whichAvatarIsOn == 3)
		{
			animator3.SetBool("Shift", shift);
			animator = animator3;
			
		}
	}

	// public method to switch avatars by pressing UI button
	public void SwitchAvatar()
	{

		// processing whichAvatarIsOn variable
		switch (whichAvatarIsOn)
		{

			// if the first avatar is on
			case 1:

				// then the second avatar is on now
				whichAvatarIsOn = 2;

				// disable the first one and anable the second one
				avatar1.gameObject.SetActive(false);
				avatar2.gameObject.SetActive(true);
				avatar3.gameObject.SetActive(false);
				break;

			// if the second avatar is on
			case 2:

				// then the third avatar is on now
				whichAvatarIsOn = 3;

				// disable the second one and anable the first one
				avatar1.gameObject.SetActive(false);
				avatar2.gameObject.SetActive(false);
				avatar3.gameObject.SetActive(true);
				break;
			case 3:

				// then the third avatar is on now
				whichAvatarIsOn = 1;

				// disable the second one and anable the first one
				avatar1.gameObject.SetActive(true);
				avatar2.gameObject.SetActive(false);
				avatar3.gameObject.SetActive(false);
				break;
		}

	}

	public int getActiveAvatar()
	{
		return whichAvatarIsOn;
	}

	public Animator getActiveAnimator()
	{
		return animator;
	}

}
