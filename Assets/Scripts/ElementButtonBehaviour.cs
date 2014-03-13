using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class ElementButtonBehaviour : MonoBehaviour {

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
			elementSelector.GetComponent<ElementSelector>().SetNew(GetComponent<SpriteRenderer>().sprite);
			elementSelector.SetActive(false);
		}
	}
}
