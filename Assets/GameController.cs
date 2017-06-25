using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController current;


	public Transform itemStartPosition, itemNextPosition;
	private Vector3 itemStartVec, itemNextVec;

	private ItemGenerator itemGenerator;
	private GameObject currentItem, nextItem;

	private Dictionary<GameObject, int> objectsByTurnsAfterUnactive = new Dictionary<GameObject, int>();

	private void cleanupTransforms() {
		itemStartVec = itemStartPosition.localPosition;
		itemNextVec = itemNextPosition.localPosition;
		Destroy (itemStartPosition.gameObject);
		Destroy (itemNextPosition.gameObject);
	}

	private void generateNextItem() {
		if (currentItem != null) {
			objectsByTurnsAfterUnactive.Add (currentItem, 0);
		}

		if (nextItem != null) {

			currentItem = nextItem;
			Debug.Log (nextItem.transform.localPosition);
			currentItem.transform.localPosition = itemStartVec;

			currentItem.GetComponent<Rigidbody2D> ().gravityScale = 1;
		}

		var objects = new List<GameObject> (objectsByTurnsAfterUnactive.Keys);
		//Debug.Log ("cnt " + objects.Count);
		foreach (GameObject obj in objects) {
			int turnsAfterUnactive = objectsByTurnsAfterUnactive [obj];
			objectsByTurnsAfterUnactive.Remove (obj);

			Debug.Log (obj.transform.eulerAngles);
			bool isToRemove = false;
			if (turnsAfterUnactive >= 3) {
				float rotation = obj.transform.eulerAngles.z;
				//Debug.Log (rotation.z);
				if (rotation % 90f < 5f || rotation % 90f > 85f) {
					isToRemove = true;
				}
			}

			if (isToRemove) {
				Destroy(obj);
			} else {
				objectsByTurnsAfterUnactive.Add (obj, turnsAfterUnactive + 1);
			}
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
		float value = Input.GetAxis ("Horizontal");

		if (currentItem != null && !nextItemPending) {
			var rigidBody = currentItem.GetComponent<Rigidbody2D> ();
			var newXValue = Mathf.Abs(value) > 0.001 ? Mathf.Min (8f, Mathf.Max (-8f, rigidBody.velocity.x + value * 2f)) : rigidBody.velocity.x * 0.7f;
			rigidBody.velocity = new Vector2 (newXValue, rigidBody.velocity.y);
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
