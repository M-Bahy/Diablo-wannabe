using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTransformAfter : MonoBehaviour
{
	public Transform particle;
	public float time;
    
	// Use this for initialization
	void Start () {
		Invoke("Create", time);
	}

	void Create()
	{
        Vector3 v = transform.position;
        v.y = 0;
		Destroy(Instantiate(particle, v + transform.forward * 20, transform.rotation).gameObject, 5); 
	}
}
