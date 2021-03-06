﻿using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class PausePlayBehaviour : MonoBehaviour {

	public StoryConstruct storyConstruct;

	void Start () {
		GetComponent<PressGesture>().StateChanged += HandleStateChanged;
	}
	
	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){		
		if (e.State == Gesture.GestureState.Recognized)	{
			storyConstruct.HardPausePlay();
		}
	}
}
