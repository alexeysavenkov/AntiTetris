using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public Transform itemStartPosition, itemNextPosition;

	private ItemGenerator itemGenerator;
	private GameObject currentItem, nextItem;

	private void cleanupTransforms() {
		Destroy (itemStartPosition.gameObject);
		Destroy (itemNextPosition.gameObject);
	}

	private void generateNextItem() {
		nextItem = itemGenerator.instantiateRandomItem (itemNextPosition);
		nextItem.GetComponent<Rigidbody2D> ().gravityScale = 0;
	}

	// Use this for initialization
	void Start () {
		cleanupTransforms ();
		itemGenerator = this.GetComponent<ItemGenerator> ();

		generateNextItem ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
