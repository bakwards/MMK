﻿using UnityEngine;
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
		objectSprites = Resources.LoadAll<Sprite>("Sprites/Objects");
		selectorButtons = transform.GetComponentsInChildren<SpriteRenderer>();
		gameObject.SetActive(false);
	}

	public void SetOrigin(SpriteRenderer s){
		Sprite[] spriteSet;
		switch(s.gameObject.transform.parent.name){
		case "Characters":
			spriteSet = characterSprites;
			break;
		case "Locations":
			spriteSet = locationSprites;
			break;
		case "Objects":
			spriteSet = objectSprites;
			break;
		default:
			break;
		}
		spriteSet = RandomizeArray(characterSprites);
		elementOrigin = s;
		int i = 0;
		foreach(SpriteRenderer button in selectorButtons){
			if(spriteSet[i].name == elementOrigin.sprite.name){
				i++;
			}
			button.sprite = spriteSet[i];
			if(i < selectorButtons.Length){
				i++;
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
