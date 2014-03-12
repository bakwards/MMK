using UnityEngine;
using System.Collections;

public class ElementSelector : MonoBehaviour {

	private string elementOrigin;
	private string elementNew;

	public void SetOrigin(string o){
		elementOrigin = o;
	}

	public void SetNew(string n){
		elementNew = n;
	}
}
