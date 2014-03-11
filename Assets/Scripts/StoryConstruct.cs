using UnityEngine;
using System.Collections;

public class StoryConstruct : MonoBehaviour {
	
	public StoryElement[] chars;
	public StoryElement[] objs;
	public StoryElement[] locs;

	public StorySegment[] storySegments;

	private AudioClip[] storyClips;
	private AudioSource audioSource;
	private int segmentNum;
	private int storyNum;



	// Use this for initialization
	void Start () {
		if(!gameObject.GetComponent<AudioSource>()){
			audioSource = gameObject.AddComponent<AudioSource>();
		} else {
			audioSource = gameObject.GetComponent<AudioSource>();
		}
		storyClips = Resources.LoadAll<AudioClip>("Audio/Kingdom/Story");
		audioSource.clip = storyClips[0];
		audioSource.Play();
		segmentNum++;
	}
	
	// Update is called once per frame
	void Update () {
		if(!audioSource.isPlaying){
			NextClip();
		}
	}

	void NextClip(){
		switch(storySegments[segmentNum].type){
		case StorySegment.ElementType.Story:
			storyNum++;
			audioSource.clip = storyClips[storyNum];
			break;
		case StorySegment.ElementType.Character:
			audioSource.clip = chars[storySegments[segmentNum].elementID].audioClips[(int)storySegments[segmentNum].wordClass];
			break;
		default:
			break;
		}
		audioSource.Play();
		segmentNum++;
	}
}
