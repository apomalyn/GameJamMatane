using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

	private bool toright = true;
	private bool facedown = true;
	private Animator character_animator;
	private Rigidbody2D character_body;
	public int tileSize = 16;
	public AudioSource attackSound;
	public AudioClip quack;
	
	// Use this for initialization
	void Start ()
	{
		this.character_body = this.GetComponent<Rigidbody2D>();
		this.character_animator = this.GetComponent<Animator>();
		attackSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
		character_animator.SetBool("Attack", false);
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			character_body.transform.localPosition = Vector3.MoveTowards(character_body.transform.localPosition, new Vector3(character_body.transform.localPosition.x, character_body.transform.localPosition.y+tileSize, character_body.transform.localPosition.z), 5*Time.deltaTime );
			//move vers le haut
		}else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			character_body.transform.localPosition = Vector3.MoveTowards(character_body.transform.localPosition, new Vector3(character_body.transform.localPosition.x, character_body.transform.localPosition.y-tileSize, character_body.transform.localPosition.z), 5*Time.deltaTime );
			//move vers le bas
		}else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (!toright)
			{
				flip();
			}
			character_body.transform.localPosition = Vector3.MoveTowards(character_body.transform.localPosition, new Vector3(character_body.transform.localPosition.x+tileSize, character_body.transform.localPosition.y, character_body.transform.localPosition.z), 5*Time.deltaTime );
			//move vers la droite
		}else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (toright)
			{
				flip();
			}
			character_body.transform.localPosition = Vector3.MoveTowards(character_body.transform.localPosition, new Vector3(character_body.transform.localPosition.x-tileSize, character_body.transform.localPosition.y, character_body.transform.localPosition.z), 5*Time.deltaTime );
			//move vers la gauche
		}else if (Input.GetKeyDown(KeyCode.Space))
		{
			//attack
			//Attack sound
			attackSound.PlayOneShot(quack);
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
