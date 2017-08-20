﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrRadar : MonoBehaviour {


	GameObject goTarget;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "bot" || col.gameObject.tag == "player"){
			//if (col.gameObject.transform.position.y <=1.1f){ //не используем - ловим цель и в т.ч. в прыжке
			if ( !col.gameObject.GetComponent<scrBall>().isFreeze()){ //если не в заморозке
				goTarget = col.gameObject;
				//gameObject.GetComponentInParent<scrBot>().SetTarget(col.gameObject); //установим цель
			}
		}
	}

	public GameObject FindTarget(float raRadius){ 
		float rr = 0;
		Component cRCol = gameObject.GetComponent<SphereCollider>();
		goTarget = null;

		while (rr < raRadius && goTarget == null){
			((SphereCollider)cRCol).radius += 0.5f;
		} 
		((SphereCollider)cRCol).radius = 0;
		return goTarget;
	}
}
