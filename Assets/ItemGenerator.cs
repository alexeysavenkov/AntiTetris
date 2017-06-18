using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGenerator : MonoBehaviour {

	public Color[] availableColours;
	public GameObject itemPrototypesContainer;

	private Dictionary<GameObject, Color[]> colorsByPrototype;
	private GameObject[] itemPrototypes;

	private void initItemPrototypes() {
		this.itemPrototypes = getChildrenOfGameObject (itemPrototypesContainer);
	}

	private static GameObject[] getChildrenOfGameObject(GameObject obj) {
		return Enumerable.Range (0, obj.transform.childCount)
			.Select (i => obj.transform.GetChild (i).gameObject)
			.ToArray ();
	}

	private void initColorsByPrototype() {
		this.colorsByPrototype = 
			availableColours.OrderBy (x => Random.Range(0f, 1f)).Select (
			(colour, i) => new { shape = itemPrototypes [i % itemPrototypes.Length], colour = colour }
			).GroupBy (x => x.shape).ToDictionary (g => g.Key, g => g.Select(x => x.colour).ToArray());
	}

	public GameObject instantiateRandomItem(Transform position) {
		GameObject prototype = itemPrototypes [Random.Range (0, itemPrototypes.Length)];
		Color[] availableColors = colorsByPrototype [prototype];
		Color color = availableColors [Random.Range (0, availableColors.Length)];

		var newInstance = GameObject.Instantiate (prototype);
		newInstance.transform.localPosition = position.localPosition;
		GetComponentsInChildren<SpriteRenderer> ().ToList ().ForEach (spriteRenderer => spriteRenderer.color = color);

		return newInstance;
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
