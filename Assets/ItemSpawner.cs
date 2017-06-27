using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

	public Transform whereToSpawn;
	public float intervalSeconds;

	public ItemGenerator itemGenerator;

	// Use this for initialization
	void Start () {
		//itemGenerator.
		StartCoroutine (coroutine());
	}

	IEnumerator coroutine() {
		yield return new WaitForSeconds(intervalSeconds / 10);
		while (true) {
			GameObject nextItem = itemGenerator.instantiateRandomItem (whereToSpawn.position);
			yield return new WaitForSeconds (intervalSeconds);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
