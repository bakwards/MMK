using UnityEngine;
using System.Collections;

public class ElementSelector : MonoBehaviour {

	private SpriteRenderer elementOrigin;
	private Sprite elementNew;
	public StoryConstruct storyConstruct;
	private Sprite[] characterSprites;
	private Sprite[] locationSprites;
	private Sprite[] objectSprites;
	private SpriteRenderer[] selectorButtons;

	void Start(){
		characterSprites = Resources.LoadAll<Sprite>("Sprites/Characters");
		locationSprites = Resources.LoadAll<Sprite>("Sprites/Locations");
		objectSprites = Resources.LoadAll<Sprite>("Sprites/Things");
		selectorButtons = transform.GetComponentsInChildren<SpriteRenderer>();
		gameObject.SetActive(false);
	}

	public void SetOrigin(SpriteRenderer s){
		Sprite[] spriteSet = new Sprite[0];
		switch(s.gameObject.transform.parent.name){
		case "Characters":
			spriteSet = characterSprites;
			break;
		case "Locations":
			spriteSet = locationSprites;
			break;
		case "Things":
			spriteSet = objectSprites;
			break;
		default:
			break;
		}
		spriteSet = RandomizeArray(spriteSet);
		elementOrigin = s;
		int i = 0;
		int adjustForCopy = 1;
		foreach(SpriteRenderer button in selectorButtons){
			button.gameObject.SetActive(false);
			if(spriteSet.Length-adjustForCopy > i){
				button.gameObject.SetActive(true);
				if(spriteSet[i].name == elementOrigin.sprite.name){
					i++;
					adjustForCopy = 0;
				}
				button.sprite = spriteSet[i];
				if(i < selectorButtons.Length){
					i++;
				}
			}
		}
		storyConstruct.Pause();
	}

	public void SetNew(Sprite n){
		storyConstruct.ChangeElement(elementOrigin.gameObject.GetComponent<ClickStoryElement>().identifier, n.name, elementOrigin.gameObject.transform.parent.name);
		elementNew = n;
		elementOrigin.sprite = n;
		storyConstruct.Play ();
	}

	Sprite[] RandomizeArray(Sprite[] arr){
		for (var i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0,i);
			Sprite tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
		return arr;
	}
}
