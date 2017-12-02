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
	
	// Use this for initialization
	void Start ()
	{
		this.character_body = this.GetComponent<Rigidbody2D>();
		this.character_animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
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
			character_body.transform.localPosition = Vector3.MoveTowards(character_body.transform.localPosition, new Vector3(character_body.transform.localPosition.x+tileSize, character_body.transform.localPosition.y, character_body.transform.localPosition.z), 5*Time.deltaTime );
			//move vers la droite
		}else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			character_body.transform.localPosition = Vector3.MoveTowards(character_body.transform.localPosition, new Vector3(character_body.transform.localPosition.x-tileSize, character_body.transform.localPosition.y, character_body.transform.localPosition.z), 5*Time.deltaTime );
			//move vers la gauche
		}
	}
}
