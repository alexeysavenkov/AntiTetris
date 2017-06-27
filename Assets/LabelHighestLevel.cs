using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelHighestLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<UILabel> ().text = "The highest level you reached: " + PlayerPrefs.GetInt ("HighScoreMaxLevel", 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
