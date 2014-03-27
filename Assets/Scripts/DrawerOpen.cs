using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;

public class DrawerOpen : MonoBehaviour {

	public bool locked = false;
	private Vector2 lastDrawPosition;
	private float maxOpenDistance = 0.5f;
	private float minOpenPosition;
	private float lastDirection = 0;

	// Use this for initialization 
	void Start () {
		if(locked) { maxOpenDistance = 0.02f;}
		minOpenPosition = transform.localPosition.z;
		GetComponent<PressGesture>().StateChanged += HandleStateChanged;
		GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChanged;
	
	}
	
	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if (e.State == Gesture.GestureState.Recognized)	{
		}
	}
	private void HandlePanStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		SimplePanGesture gesture = sender as SimplePanGesture;
		float deltaPosition = 0;
		float deltaDrawDown = 0;
		if(e.State == Gesture.GestureState.Began){
			lastDrawPosition = gesture.ScreenPosition;
		} else {
			deltaDrawDown = lastDrawPosition.y - gesture.ScreenPosition.y;
			deltaPosition = lastDrawPosition.x - gesture.ScreenPosition.x;
			lastDrawPosition = gesture.ScreenPosition;
		}
		if(!float.IsNaN(deltaPosition)){
			deltaPosition *= 0.005f*Vector3.Dot(transform.right, transform.position-Camera.main.transform.position);
			deltaPosition += 0.005f * deltaDrawDown;
			deltaPosition = Mathf.Clamp(deltaPosition,
		                            minOpenPosition-transform.localPosition.z,
		                            minOpenPosition+maxOpenDistance-transform.localPosition.z);
			Vector3 newPosition = transform.position + transform.forward*deltaPosition;
			rigidbody.MovePosition(newPosition);
			if(minOpenPosition+maxOpenDistance-transform.localPosition.z < 0.001 && !locked && lastDirection > 0){
				AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Drawer_fully_open"));
				if(transform.FindChild("StoryController")){
					transform.FindChild("StoryController").gameObject.SetActive(true);
					transform.FindChild("StoryController").GetComponent<StoryConstruct>().originalParent = gameObject;
					transform.FindChild("StoryController").GetComponent<StoryConstruct>().UpdatePage(transform.FindChild("StoryController").GetComponent<StoryConstruct>().pageSegments[0]);
					//Camera.main.GetComponent<CameraControl>().enabled = false;
					Camera.main.GetComponent<CameraControl>().SetFollowState(false);
					iTween.MoveTo(Camera.main.gameObject, iTween.Hash("path", transform.FindChild("StoryController").GetComponent<StoryConstruct>().path,
					                                                  "time", 5,
					                                                  "looktarget", transform.FindChild("StoryController").gameObject.transform.FindChild("Focuspoint").gameObject.transform,
					                                                  "easetype", iTween.EaseType.easeInOutSine,
					                                                  "oncompletetarget", gameObject,
					                                                  "oncomplete", "TestFunction"));
					transform.FindChild("StoryController").parent = null;
				}
			}
			if(minOpenPosition-transform.localPosition.z > -0.001 && !locked){
				if(lastDirection < 0){
					AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Drawer_fully_open"));
				} else if (lastDirection > 0){
					AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Drawer_open_from_closed"));
				}
			}
			if(minOpenPosition+maxOpenDistance-transform.localPosition.z < 0.01 && locked && !AudioController.Instance.mainAudioSource.isPlaying){
				AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Drawer_locked"));
			}
			lastDirection = Mathf.Sign (deltaPosition);
		}
		if (e.State == Gesture.GestureState.Recognized)	{
			if(lastDirection > 0 && minOpenPosition+maxOpenDistance-transform.localPosition.z >= 0.01){
			} else if (lastDirection < 0 && minOpenPosition-transform.localPosition.z >= 0.01) {
			}
			Debug.Log (minOpenPosition-transform.localPosition.z);
		}
	}
	void TestFunction(){
		//Debug.Log("Done!");
	}
}
