using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {

	public Transform[] paths;

	// Use this for initialization
	void Start () {
		//iTween.MoveTo(Camera.main.gameObject, iTween.Hash("path", paths, "time", 5, "looktarget", gameObject.transform));
		//iTween.ScaleTo(gameObject, iTween.Hash("x", 2));
		//iTween.MoveTo(gameObject, iTween.Hash("position", paths[0]));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")){
			iTween.MoveTo(Camera.main.gameObject, iTween.Hash("path", paths, "time", 5, "looktarget", gameObject.transform));
		}
	
	}
}
