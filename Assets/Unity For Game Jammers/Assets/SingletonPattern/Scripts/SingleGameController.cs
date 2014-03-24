using UnityEngine;
using System.Collections;

public class SingleGameController : MonoBehaviour {

	void Update () 
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AudioController.Instance.PlaySound();
        }
	}
}
