﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBall : MonoBehaviour {
	public bool isPlayer = false;
	public GameObject pfFreezer;
	public GameObject pfRadar;
	GameObject objFreeze;


	float freezeTime;//время до окончания заморозки
	float motionForce = 5f;
	float ultraForceTime;
	float ultraFreezeRadiusTime=0;

	// Use this for initialization
	void Awake(){
		if (isPlayer){
			gameObject.GetComponent<scrBot>().enabled = false;
			gameObject.GetComponent<scrController>().enabled = true;
			gameObject.GetComponent<scrMoveCamera>().enabled = true;
			gameObject.GetComponent<Renderer>().material.color = new Color32(177,253,184,255);
			gameObject.tag = "player";
		}
		else{
			gameObject.GetComponent<scrBot>().enabled = true;
			gameObject.GetComponent<scrController>().enabled = false;
			gameObject.GetComponent<scrMoveCamera>().enabled = false;
			gameObject.GetComponent<Renderer>().material.color = new Color32(248,212,147,255);
			gameObject.tag = "bot";
			GameObject pfR = Instantiate(pfRadar, gameObject.transform.position, Quaternion.identity);
			pfR.transform.parent = gameObject.transform;
			pfR.name = "radar1";//FindTarget
            pfR.GetComponent<scrRadar>().SetMode(1);

            GameObject pfR2 = Instantiate(pfRadar, gameObject.transform.position, Quaternion.identity);
            pfR2.transform.parent = gameObject.transform;
            pfR2.name = "radar2";//is need jump
            pfR2.GetComponent<scrRadar>().SetMode(2);
        }
	}

	void Start () {
		//objFreeze = gameObject.transform.Find("wave").gameObject;
		if (pfFreezer == null) Debug.LogError("pfFreeze is null");
		objFreeze = null;
		motionForce = scrGlobal.motionForce; //initial force is same for all
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 20f;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if (freezeTime>0){
			freezeTime -= Time.fixedDeltaTime;
			if (freezeTime <= 0){
				if (isPlayer){
					gameObject.GetComponent<Renderer>().material.color = new Color32(177,253,184,255);
				} else {
					gameObject.GetComponent<Renderer>().material.color = new Color32(248,212,147,255);
				}
			}
		}
		if (ultraForceTime>0){
			ultraForceTime -= Time.fixedDeltaTime;
			if (ultraForceTime<=0) motionForce = scrGlobal.motionForce;
		}
		if (ultraFreezeRadiusTime>0){
			ultraFreezeRadiusTime -= Time.fixedDeltaTime;
		}

		if (gameObject.transform.position.y < -30f){
			doDeath();
		}
	}

	public void jump(){
		if (isFreeze()) return;
		if (gameObject.transform.position.y < 1.1f){
			gameObject.transform.GetComponent<Rigidbody>().AddForce(0,50,0);
		}
	}

	/// <summary>
	/// Move ball in the direction.
	/// Work for player controller and for bot too.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void go(Vector3 direction){
		if (isFreeze()) return;
		if (transform.position.y <= 1f && gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < scrGlobal.maxSpeedSqr){ //если не в полёте и скорость не максимальная
			Vector3 tV3 = new Vector3(direction.x,0,direction.z);
			tV3 = tV3.normalized * motionForce;
			gameObject.transform.GetComponent<Rigidbody>().AddForce(tV3);
		}
	}

	public void freezerRun(){
		if (isFreeze()) return;
		if (transform.position.y <= 1f && objFreeze == null){
			objFreeze = GameObject.Instantiate(pfFreezer,new Vector3(transform.position.x,0.05f,transform.position.z), Quaternion.identity);
			if (objFreeze != null){
				if (ultraFreezeRadiusTime<=0){
					objFreeze.GetComponent<scrFreezer>().freezerRun(scrGlobal.freezerRadius, gameObject);
				} else objFreeze.GetComponent<scrFreezer>().freezerRun(scrGlobal.ultraFreezerRadius, gameObject);
			} else Debug.Log("objFreeze is null");
		}
	}

	public bool isFreeze(){
		if (freezeTime > 0){
			return true;
		} else return false;
	}

	public void doFreeze(){
		if (!isFreeze()) {
			freezeTime = scrGlobal.freezeTime;
			gameObject.GetComponent<Renderer>().material.color = new Color32(148,112,247,255);
		}
	}

	public void SetUltraForce(){
		ultraForceTime += scrGlobal.ultraForceTime;
		motionForce = scrGlobal.ultraMotionForce;
	}

	public void setUltraFreezeRadius(){
		ultraFreezeRadiusTime = scrGlobal.ultraFreezerRadiusTime;
	}

	public void doDeath(){
		gameObject.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
		gameObject.transform.GetComponent<Rigidbody>().ResetInertiaTensor();
		gameObject.transform.position = new Vector3(0,10,0);

	}
}
