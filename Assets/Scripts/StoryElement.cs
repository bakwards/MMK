using UnityEngine;
using System.Collections;

[System.Serializable]
public class StoryElement {

	public string elementName;
	private int identifier;
	public AudioClip[] audioClips; //singular-determinate, singular-nondeterminate, plural-determinate, plural-nondeterminate
}
