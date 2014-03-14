using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class DrawerTouchBehaviour : MonoBehaviour {

	private float openDistance = 1;
	private Vector3 openPosition;
	private bool open = false;
	private Vector3 closedPosition;

	public GameObject storyController;
	public GameObject touchTarget;
	public SmoothFollow cameraScript;
	public CameraControl controlOne;
	public CameraControl controlTwo;

	// Use this for initialization
	void Start () {
		closedPosition = transform.position;
		openPosition = closedPosition + -Vector3.forward * openDistance;
		GetComponent<TapGesture>().StateChanged += HandleStateChanged;
	
	}
	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if (e.State == Gesture.GestureState.Recognized)	{
			open = !open;
			controlOne.enabled = !open;
			controlTwo.enabled = !open;
			if(open){
				cameraScript.target = storyController.transform;
				storyController.SetActive(true);
				storyController.transform.position = transform.position-Vector3.forward+Vector3.up*0.42f;
				storyController.GetComponent<StoryConstruct>().RestartStory();
			} else {
				cameraScript.target = touchTarget.transform;
				storyController.SetActive(false);
			}
		}
	}

	void Update(){
		if(open && Vector3.Distance(transform.position, openPosition) > 0.01f){
			cameraScript.distance = 1.1f;
			cameraScript.height = 0.4f;
			transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime);
		} else if (Vector3.Distance(transform.position, closedPosition) > 0.01f) {
			transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime);
		}
	}
}
