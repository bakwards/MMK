using UnityEngine;
using System.Collections;

public class StoryConstruct : MonoBehaviour {

	public string storyName;

	public string[] characters;
	public string[] objects;
	public string[] locations;
	
	private StoryElement[] chars;
	private StoryElement[] objs;
	private StoryElement[] locs;

	public StorySegment[] storySegments;
	public int[] pageSegmentStart;
	private int currentPage;
	private int firstStoryOnPage;

	private AudioClip[] storyClips;
	private AudioSource audioSource;
	private int segmentNum;
	private int storyNum;

	private bool pause = false;


	// Use this for initialization
	void Start () {
		chars = new StoryElement[characters.Length];
		objs = new StoryElement[objects.Length];
		locs = new StoryElement[locations.Length];
		int i = 0;
		foreach(string c in characters){
			string audioPath = "Audio/" + storyName + "/Characters/" + c;
			chars[i] = new StoryElement();
			chars[i].audioClips = Resources.LoadAll<AudioClip>(audioPath);
			i++;
		}
		i = 0;
		foreach(string l in locations){
			string audioPath = "Audio/" + storyName + "/Locations/" + l;
			locs[i] = new StoryElement();
			locs[i].audioClips = Resources.LoadAll<AudioClip>(audioPath);
			i++;
		}
		i = 0;
		foreach(string o in objects){
			string audioPath = "Audio/" + storyName + "/Things/" + o;
			objs[i] = new StoryElement();
			objs[i].audioClips = Resources.LoadAll<AudioClip>(audioPath);
			i++;
		}
		if(!gameObject.GetComponent<AudioSource>()){
			audioSource = gameObject.AddComponent<AudioSource>();
		} else {
			audioSource = gameObject.GetComponent<AudioSource>();
		}
		storyClips = Resources.LoadAll<AudioClip>("Audio/" + storyName + "/Story");
		NextClip();
	}
	
	// Update is called once per frame
	void Update () {
		if(!audioSource.isPlaying && !pause && segmentNum < storyClips.Length){
			NextClip();
		}
	}

	void NextClip(){
		StorySegment nextSegment = storySegments[segmentNum];
		switch(nextSegment.type){
		case StorySegment.ElementType.Story:
			audioSource.clip = storyClips[storyNum];
			storyNum++;
			break;
		case StorySegment.ElementType.Character:
			audioSource.clip = chars[nextSegment.elementID].audioClips[(int)nextSegment.wordClass];
			break;
		case StorySegment.ElementType.Location:
			audioSource.clip = locs[nextSegment.elementID].audioClips[(int)nextSegment.wordClass];
			break;
		case StorySegment.ElementType.Thing:
			audioSource.clip = objs[nextSegment.elementID].audioClips[(int)nextSegment.wordClass];
			break;
		default:
			break;
		}
		nextSegment.title = audioSource.clip.name;
		audioSource.Play();
		segmentNum++;
		if(currentPage < pageSegmentStart.Length && segmentNum > pageSegmentStart[currentPage+1]){
			currentPage++;
			firstStoryOnPage = storyNum-1;
		}
	}
	
	public void Pause(){
		pause = true;
	}
	public void Play(){
		if(!audioSource.isPlaying) {
			segmentNum = pageSegmentStart[currentPage];
			storyNum = firstStoryOnPage;
			NextClip();
		}
		pause = false;
	} 

	public void ChangeElement(int i, string newElement){
		characters[i] = newElement;
		string audioPath = "Audio/" + storyName + "/Characters/" + newElement;
		chars[i].audioClips = Resources.LoadAll<AudioClip>(audioPath);
	}
}
