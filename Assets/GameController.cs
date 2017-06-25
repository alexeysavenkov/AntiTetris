using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController current;


	public Transform itemStartPosition, itemNextPosition;
	private Vector3 itemStartVec, itemNextVec;

	private ItemGenerator itemGenerator;
	private GameObject currentItem, nextItem;

	private void cleanupTransforms() {
		itemStartVec = itemStartPosition.localPosition;
		itemNextVec = itemNextPosition.localPosition;
		Destroy (itemStartPosition.gameObject);
		Destroy (itemNextPosition.gameObject);
	}

	private void generateNextItem() {
		if (nextItem != null) {
			currentItem = nextItem;
			Debug.Log (nextItem.transform.localPosition);
			currentItem.transform.localPosition = itemStartVec;

			currentItem.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}

		nextItem = itemGenerator.instantiateRandomItem (itemNextVec);
		nextItem.GetComponent<Rigidbody2D> ().gravityScale = 0;
	}

	// Use this for initialization
	void Start () {
		current = this;

		cleanupTransforms ();
		itemGenerator = this.GetComponent<ItemGenerator> ();

		generateNextItem ();
		generateNextItem ();
	}
	
	// Update is called once per frame
	void Update () {
		int movX = 0;

		if (!nextItemPending) {
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				movX = 1;
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				movX = -1;
			}
		}

		if (movX != 0) {
			currentItem.transform.localPosition = currentItem.transform.localPosition + new Vector3 (movX, 0, 0);
		}
	}

	private bool nextItemPending = false;
	public void onItemCollide(GameObject item) {
		lock (this) {
			if (item == currentItem) {
				if (!nextItemPending) {
					nextItemPending = true;
					StartCoroutine(generateNextItemCoroutine());
				}
			}
		}
	}

	private IEnumerator generateNextItemCoroutine() {
		yield return new WaitForSeconds(1);
		generateNextItem ();
		nextItemPending = false;
	}
}
