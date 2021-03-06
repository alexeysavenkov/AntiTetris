﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGenerator : MonoBehaviour {

	public Color[] availableColours;
	public GameObject itemPrototypesContainer;

	private Dictionary<GameObject, Color[]> colorsByPrototype;
	private GameObject[] itemPrototypes;

	private void initItemPrototypes() {
		this.itemPrototypes = Utils.getChildrenOfGameObject (itemPrototypesContainer);
	}



	private void initColorsByPrototype() {
		this.colorsByPrototype = 
			availableColours.OrderBy (x => Random.Range(0f, 1f)).Select (
			(colour, i) => new { shape = itemPrototypes [i % itemPrototypes.Length], colour = colour }
			).GroupBy (x => x.shape).ToDictionary (g => g.Key, g => g.Select(x => x.colour).ToArray());
	}

	public GameObject instantiateRandomItem(Vector3 position) {
		GameObject prototype = itemPrototypes [Random.Range (0, itemPrototypes.Length)];
		Color[] availableColors = colorsByPrototype [prototype];
		Color color = availableColors [Random.Range (0, availableColors.Length)];

		var newInstance = GameObject.Instantiate (prototype);
		newInstance.transform.localPosition = position;
		var spriteRenderers = newInstance.GetComponentsInChildren<SpriteRenderer> ().ToList ();
		Debug.Log (spriteRenderers.Count);
		spriteRenderers.ForEach ( spriteRenderer => {
			//Debug.Log(spriteRenderer.gameObject.name);
			spriteRenderer.color = color;
		});

		randomRotate (newInstance);

		return newInstance;
	}

	private void randomRotate(GameObject obj) {
		obj.transform.Rotate (0, 0, 90 * (int)(Random.value * 4));
	}



	// Use this for initialization
	void Start () {
		this.initItemPrototypes ();
		this.initColorsByPrototype ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
