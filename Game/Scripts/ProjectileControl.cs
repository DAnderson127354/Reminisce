using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    public float lifespan = 2f;
	     
	void Start()
	{
		Destroy(gameObject, lifespan);
	}

}
