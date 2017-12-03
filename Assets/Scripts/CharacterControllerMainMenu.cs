using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class CharacterControllerMainMenu : MonoBehaviour
{

	private bool toright = true;
	private bool facedown = true;
	private Animator character_animator;
	private Rigidbody2D character_body;
	private float speed = 5f;
	public AudioSource attackSound;
	public AudioClip quack;
	public GameObject menuLogo;
	public GameObject menuLogo2;
	private bool logo;
	
	// Use this for initialization
	void Start ()
	{
		this.character_body = this.GetComponent<Rigidbody2D>();
		this.character_animator = this.GetComponent<Animator>();
		attackSound = GetComponent<AudioSource>();
		logo = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		character_animator.SetBool("Attack", false);
		if (Input.anyKeyDown)
		{
			if (logo)
			{
				logo = false;
				Destroy(menuLogo2);
				Destroy(menuLogo);
			}
		}
		if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
			{
				character_body.transform.Translate(0.005f, 0.005f, 0);
			}
			else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
			{
				character_body.transform.Translate(-0.005f, 0.005f, 0);
			}
			else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
			{
				character_body.transform.Translate(0.005f, -0.005f, 0);
			}
			else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
			{
				character_body.transform.Translate(-0.005f, -0.005f, 0);
			}
			else if (Input.GetKey(KeyCode.UpArrow))
			{
				//move vers le haut
				character_body.transform.Translate(0, 0.005f, 0);
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				character_body.transform.Translate(0, -0.005f, 0);
				//move vers le bas
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				if (!toright)
				{
					flip();
				}
				character_body.transform.Translate(0.005f, 0, 0);
				//move vers la droite
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				if (toright)
				{
					flip();
				}
				character_body.transform.Translate(-0.005f, 0, 0);
				//move vers la gauche
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				//attack
				//Attack sound
				attackSound.PlayOneShot(quack, 0.3f);
				//Attack animation
				character_animator.SetBool("Attack", true);


			}
		
	}

	void flip()
	{
		toright = !toright;
		Vector3 scale = this.transform.localScale;
		scale.x *= -1;
		this.transform.localScale = scale;
	}
}
