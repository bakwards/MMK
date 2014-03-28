using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class RestartButton : MonoBehaviour {
	
	void Start () {
		GetComponent<PressGesture>().StateChanged += HandleStateChanged;
	}
	
	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){		
		if (e.State == Gesture.GestureState.Recognized)	{
			Debug.Log ("Press!");
		}
	}
}
