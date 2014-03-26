﻿using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class GoBackButton : MonoBehaviour {

	public StoryConstruct storyConstruct;

	void Start () {
		GetComponent<PressGesture>().StateChanged += HandleStateChanged;
	}
	
	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){		
		if (e.State == Gesture.GestureState.Recognized)	{
			Camera.main.GetComponent<CameraControl>().SetFollowState(true);
			storyConstruct.RestartStory();
			storyConstruct.textMesh.text = " ";
			storyConstruct.transform.parent = storyConstruct.originalParent.transform;
			storyConstruct.gameObject.SetActive(false);
		}
	}
}
