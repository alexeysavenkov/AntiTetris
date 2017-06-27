using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelPrevLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<UILabel> ().text = "You have reached level " + PlayerPrefs.GetInt ("PrevMaxLevel");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
