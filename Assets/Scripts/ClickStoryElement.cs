using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class ClickStoryElement : MonoBehaviour {

	public GameObject elementSelector;
	public int identifier;

	void Start () {
		GetComponent<PressGesture>().StateChanged += HandleStateChanged;
	}

	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if (e.State == Gesture.GestureState.Recognized)	{
			AudioController.Instance.PlayClip((AudioClip)Resources.Load ("Audio/SoundFX/Blop"));
			elementSelector.SetActive(true);
			elementSelector.transform.position = transform.position;
			elementSelector.GetComponent<ElementSelector>().SetOrigin(GetComponent<SpriteRenderer>());
		}
	}
}
