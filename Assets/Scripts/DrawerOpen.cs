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
	private SphereCollider[] colliders;
	private StoryConstruct storyConstruct;

	// Use this for initialization 
	void Start () {
		if(transform.FindChild("StoryController")){
			storyConstruct = transform.FindChild("StoryController").GetComponent<StoryConstruct>();
		}
		colliders = transform.GetComponentsInChildren<SphereCollider>();
		if(locked) { maxOpenDistance = 0.02f;}
		minOpenPosition = transform.localPosition.z;
		GetComponent<TapGesture>().StateChanged += HandleStateChanged;
		GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChanged;
	}
	
	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if (e.State == Gesture.GestureState.Recognized)	{
			if(minOpenPosition-transform.localPosition.z > -0.001){
				StartCoroutine("OpenDrawer");
			} else {
				StartCoroutine("CloseDrawer");
			}
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
				MoveCameraToStory();
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
		if (e.State == Gesture.GestureState.Recognized && !locked && iTween.Count() == 0)	{
			if(lastDirection > 0 && minOpenPosition+maxOpenDistance-transform.localPosition.z >= 0.001){
				OpenDrawer();
			}
			else if (lastDirection < 0 && minOpenPosition-transform.localPosition.z <= -0.001) {
				CloseDrawer();
			}
		}
	}
	void ActivateStoryController(){
		storyConstruct.gameObject.SetActive(true);
		storyConstruct.UpdatePage(transform.FindChild("StoryController").GetComponent<StoryConstruct>().pageSegments[0]);
		storyConstruct.transform.parent = null;
		Camera.main.GetComponent<CameraControl>().SetBackButtonActive(true);
	}
	void MoveCameraToStory(){
		if(transform.FindChild("StoryController") && iTween.Count () == 0){
			GameObject currentStoryToStart = transform.FindChild("StoryController").gameObject;
			AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Storyintro"),true);
			Camera.main.GetComponent<CameraControl>().SetFollowState(false);
			Camera.main.GetComponent<CameraControl>().backButton.GetComponent<GoBackButton>().storyConstruct = currentStoryToStart.GetComponent<StoryConstruct>();
			iTween.MoveTo(Camera.main.gameObject, iTween.Hash("path", transform.FindChild("StoryController").GetComponent<StoryConstruct>().path,
			                                                  "time", 5,
			                                                  "looktarget", currentStoryToStart.transform.FindChild("Focuspoint").gameObject.transform,
			                                                  "easetype", iTween.EaseType.easeInOutSine,
			                                                  "oncompletetarget", gameObject,
			                                                  "oncomplete", "ActivateStoryController"));
		}
	}
	void OpenDrawer(){
		AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Drawer_open_from_closed"));
		foreach(SphereCollider c in colliders){
			c.enabled = false;
		}
		StartCoroutine("OpenDrawerCoroutine");
	}
	IEnumerator OpenDrawerCoroutine(){
		while(true){
			if(minOpenPosition+maxOpenDistance-transform.localPosition.z < 0.001){
				foreach(SphereCollider c in colliders){
					c.enabled = true;
				}
				MoveCameraToStory();
				AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Drawer_fully_open"));
				yield break;
			} else {
				transform.position += transform.forward * Time.deltaTime;
				yield return null;
			}
		}
	}
	public void CloseDrawer(){
		AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Drawer_open_from_closed"));
		if(storyConstruct != null){
			iTween.Stop();
			Camera.main.GetComponent<CameraControl>().SetFollowState(true);
			storyConstruct.RestartStory();
			storyConstruct.textMesh.text = " ";
			storyConstruct.transform.parent = storyConstruct.originalParent.transform;
			storyConstruct.gameObject.SetActive(false);
			storyConstruct.SetAmbience("Ambiens_clock");
		}
		foreach(SphereCollider c in colliders){
			c.enabled = false;
		}
		StartCoroutine("CloseDrawerCoroutine");
	}
	IEnumerator CloseDrawerCoroutine(){
		while(true){
			if(minOpenPosition-transform.localPosition.z > -0.001){
				foreach(SphereCollider c in colliders){
					c.enabled = true;
				}
				AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Drawer_fully_open"));
				yield break;
			} else {
				transform.position -= transform.forward * Time.deltaTime;
				yield return null;
			}
		}
	}

}
