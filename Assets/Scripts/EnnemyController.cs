using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{

	public float tileSize = 0.16f;
	public GameObject[] pathPoint; //tableau qui va contenir les gameObjects représentant le chemin a suivre
	private int currentPathPoint = 0; //point courant du chemin que nous tentons de rejoindre
	public static float vitesse = 0.5f;
	private Vector3 posStart;
	
	[SerializeField]
	private int life;
	private Rigidbody2D ennemy_body;
	
	
	// Use this for initialization
	void Start ()
	{
		ennemy_body = GetComponent<Rigidbody2D>();
		posStart = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		deplacement();
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player") 
		{	
			life -= 1;
			if (life <= 0) 
			{
				destroy();
			}
		}
	}

	void deplacement()
	{
		
		if (currentPathPoint < pathPoint.Length) //si le chemin n'est pas parcouru en entier
		{
			Vector3 target = pathPoint [currentPathPoint].transform.position; //vecteur vers le prochain point du chemin a suivre
			Vector3 moveDirection = target - transform.position; //vecteur qui va représenter la direction à suivre pour notre ennemi
			Vector3 velocity = ennemy_body.velocity;
			
			if (moveDirection.magnitude < 0.11)  //si on est rendu au point
			{
				currentPathPoint++; //on passe au point suivant
			} 
			else 
			{
				velocity = moveDirection.normalized * vitesse; //on détermine une vélocité en fonction de la vitesse et de la direction à suivre
			}
			ennemy_body.velocity = velocity; //on applique cette vélocité à notre ennemi
		}
		else  //si le chemin est terminé
		{
			currentPathPoint = 0;
		}
	}

	void destroy()
	{
		Destroy(gameObject);
	}
}
