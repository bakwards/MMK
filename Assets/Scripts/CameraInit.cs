using UnityEngine;
using System.Collections;

public class CameraInit : MonoBehaviour {

	public CameraControl cameraControl;
	public MeshRenderer fadePlane;
	public SpriteRenderer logoSprite;
	public AudioClip introSound;

	// Use this for initialization
	void Start () {
		AudioController.Instance.PlayClip(introSound);
		Color newColor = fadePlane.material.color;
		newColor.a = 1;
		fadePlane.material.color = newColor;
		logoSprite.gameObject.SetActive(true);
		iTween.MoveFrom(logoSprite.gameObject, iTween.Hash("islocal", true, "y", -5, "time", 2.8));
		cameraControl.smoothFollow.rotationDamping = 300;
		cameraControl.smoothFollow.heightDamping = 1.1f;
		cameraControl.smoothFollow.height = 20;
		cameraControl.smoothFollow.distance = 10;
		cameraControl.controlTarget.angularDrag = 0.2f;
		cameraControl.controlTarget.AddTorque(Vector3.up*300);
		cameraControl.gameObject.GetComponent<BoxCollider>().enabled = false;
		cameraControl.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad > 3){
			Color spriteColor = logoSprite.color;
			spriteColor.a = logoSprite.color.a - Time.deltaTime;
			logoSprite.color = spriteColor;
		}
		if(fadePlane.material.color.a > 0){
			Color newColor = fadePlane.material.color;
			newColor.a = newColor.a - Time.timeSinceLevelLoad*0.3f;
			fadePlane.material.color = newColor;
		}
		cameraControl.smoothFollow.height = 2.5f;
		if(cameraControl.smoothFollow.distance > 5){
			cameraControl.smoothFollow.distance -= Time.deltaTime*1.3f;
		} else {
			cameraControl.smoothFollow.distance = 5;
		}
		if(cameraControl.controlTarget.angularVelocity.y < 3){
			if(cameraControl.controlTarget.angularDrag < 2){
				cameraControl.controlTarget.angularDrag += Time.deltaTime*2;
			} else {
				Color spriteColor = logoSprite.color;
				spriteColor.a = 0;
				logoSprite.color = spriteColor;
				cameraControl.controlTarget.angularDrag = 2;
				cameraControl.smoothFollow.rotationDamping = 3;
				cameraControl.enabled = true;
				cameraControl.gameObject.GetComponent<BoxCollider>().enabled = true;
				cameraControl.smoothFollow.heightDamping = 4;
				cameraControl.smoothFollow.distance = 5;
				this.enabled = false;
			}
		}
	}
}
