using UnityEngine;
using System.Collections;

public class ElementSelector : MonoBehaviour {

	private SpriteRenderer elementOrigin;
	private Sprite elementNew;
	public StoryConstruct storyConstruct;
	private Sprite[] characterSprites;
	private SpriteRenderer[] selectorButtons;

	void Start(){
		characterSprites = Resources.LoadAll<Sprite>("Sprites/Characters");
		selectorButtons = transform.GetComponentsInChildren<SpriteRenderer>();
		gameObject.SetActive(false);
	}

	public void SetOrigin(SpriteRenderer s){
		characterSprites = RandomizeArray(characterSprites);
		elementOrigin = s;
		int i = 0;
		foreach(SpriteRenderer button in selectorButtons){
			if(characterSprites[i].name == elementOrigin.sprite.name){
				i++;
			}
			button.sprite = characterSprites[i];
			if(i < selectorButtons.Length){
				i++;
			}
		}
		storyConstruct.Pause();
	}

	public void SetNew(Sprite n){
		storyConstruct.ChangeElement(elementOrigin.gameObject.GetComponent<ClickStoryElement>().identifier, n.name);
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
