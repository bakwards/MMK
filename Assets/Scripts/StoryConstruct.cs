using UnityEngine;
using System.Collections;
using tk2dRuntime;

public class StoryConstruct : MonoBehaviour {

	public bool debugActive;
	public AudioSource ambienceSource;

	public string storyName;

	public string[] characters;
	public string[] objects;
	public string[] locations;
	
	private StoryElement[] chars;
	private StoryElement[] objs;
	private StoryElement[] locs;
	
	private ClickStoryElement[] characterGraphics;
	private ClickStoryElement[] locationGraphics;
	private ClickStoryElement[] thingGraphics;

	public StorySegment[] storySegments;
	public PageSegment[] pageSegments;
	//public int[] pageSegmentStart;
	private int currentPage;
	public int firstStoryOnPage;

	private AudioClip[] storyClips;
	private AudioSource audioSource;
	private int segmentNum;
	public int storyNum;

	public GameObject originalParent;
	private bool pause = false;
	public bool lookingForFirstStory = true;
	public tk2dTextMesh textMesh;
	public Transform[] path;


	// Use this for initialization
	void Start () {
		Transform storyGraphics = transform.FindChild("StoryGraphics").transform;
		characterGraphics = storyGraphics.FindChild("Characters").transform.GetComponentsInChildren<ClickStoryElement>() as ClickStoryElement[];
		locationGraphics = storyGraphics.FindChild("Locations").transform.GetComponentsInChildren<ClickStoryElement>() as ClickStoryElement[];
		thingGraphics = storyGraphics.FindChild("Things").transform.GetComponentsInChildren<ClickStoryElement>() as ClickStoryElement[];
		GeneratePageContent();
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
		gameObject.SetActive(debugActive);
		UpdatePage(pageSegments[currentPage]);
		GeneratePageContent();
		SetAmbience(locations[0]);
	}
	
	// Update is called once per frame
	void Update () {
		if(!audioSource.isPlaying && !pause && segmentNum < storySegments.Length){
			NextClip();
		}
	}

	void NextClip(){
		if(currentPage+1 < pageSegments.Length && segmentNum >= pageSegments[currentPage+1].pageSegmentStart){
			currentPage++;
			UpdatePage(pageSegments[currentPage]);
			GeneratePageContent();
			lookingForFirstStory = true;
		}
		StorySegment nextSegment = storySegments[segmentNum];
		switch(nextSegment.type){
		case StorySegment.ElementType.Story:
			if(lookingForFirstStory){
				firstStoryOnPage = storyNum;
				lookingForFirstStory = false;
			}
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
	}
	
	public void Pause(){
		pause = true;
	}
	public void Play(){
		//if(!audioSource.isPlaying) {
			segmentNum = pageSegments[currentPage].pageSegmentStart;
			if(!lookingForFirstStory){
				storyNum = firstStoryOnPage;
			}
			NextClip();
		//}
		UpdatePage(pageSegments[currentPage]);
		pause = false;
	} 
	
	public void HardPausePlay(){
		pause = !pause;
		if(pause){
			audioSource.Pause();
		} else {
			audioSource.Play();
		}
	}

	public void ChangeElement(int i, string newElement, string type){ //change element, not character - take sprite parent name as argument
		string audioPath = "Audio/" + storyName + "/" + type + "/" + newElement;
		switch (type) {
		case "Characters":
			chars[i].audioClips = Resources.LoadAll<AudioClip>(audioPath);
			characters[i] = newElement;
			break;
		case "Locations":
			locs[i].audioClips = Resources.LoadAll<AudioClip>(audioPath);
			locations[i] = newElement;
			SetAmbience(newElement);
			break;
		case "Things":
			objs[i].audioClips = Resources.LoadAll<AudioClip>(audioPath);
			objects[i] = newElement;
			break;
		default:
			break;
		}
	}

	public void UpdatePage(PageSegment page){
		string newString = page.pageText;
		foreach(int c in page.characters){
			string stringCheck = "C"+c;
			newString = newString.Replace(stringCheck, characters[c]);
		}
		foreach(int t in page.things){
			string stringCheck = "T"+t;
			newString = newString.Replace(stringCheck, objects[t]);
		}
		newString = newString.Replace("L"+page.location, locations[page.location]);
		//Debug.Log (newString);
		textMesh.text = newString;
	}

	void GeneratePageContent(){
		//get children from transform, iterate their id content, activate all and deactivate those not on page.
		foreach(ClickStoryElement c in characterGraphics){
			c.gameObject.SetActive(false);
			foreach(int i in pageSegments[currentPage].characters){
				if(i == c.identifier){
					c.gameObject.SetActive(true);
				}
			}
		}
		foreach(ClickStoryElement l in locationGraphics){
			l.gameObject.SetActive(false);
			if(pageSegments[currentPage].location == l.identifier){
				l.gameObject.SetActive(true);
			}
		}
		foreach(ClickStoryElement t in thingGraphics){
			t.gameObject.SetActive(false);
			foreach(int i in pageSegments[currentPage].things){
				if(i == t.identifier){
					t.gameObject.SetActive(true);
				}
			}
		}
	}

	public void SetAmbience(string name){
		ambienceSource.clip = (AudioClip)Resources.Load("Audio/SoundFX/" + name);
		ambienceSource.Play();
	}

	public void RestartStory(){
		SetAmbience(locations[0]);
		currentPage = 0;
		firstStoryOnPage = 0;
		storyNum = 0;
		segmentNum = 0;
		audioSource.Stop();
		pause = false;
		NextClip();
		GeneratePageContent();
		UpdatePage(pageSegments[0]);
	}
}
