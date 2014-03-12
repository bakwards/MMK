using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class ClickStoryElement : MonoBehaviour {

	public GameObject elementSelector;

	// Use this for initialization
	void Start () {
		GetComponent<PressGesture>().StateChanged += HandleStateChanged;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if (e.State == Gesture.GestureState.Recognized)	{
			Debug.Log ("I'm pressed");
			elementSelector.SetActive(true);
			elementSelector.transform.position = transform.position;
			elementSelector.GetComponent<ElementSelector>().SetOrigin(GetComponent<SpriteRenderer>().sprite.name);
		}
	}
}
