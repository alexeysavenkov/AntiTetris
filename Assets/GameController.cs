using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController current;


	public Transform itemStartPosition, itemNextPosition;
	private Vector3 itemStartVec, itemNextVec;
	private int level = 1;
	private int score = 50;

	public Transform targetTick;

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

		bool isGameEnd = false;

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

			if (Utils.getChildrenOfGameObject (obj).Any (o => o.transform.position.y + o.transform.lossyScale.y * .25f > targetTick.position.y)) {
				isGameEnd = true;
			}

			if (isToRemove) {
				score--;
				Destroy(obj);
			} else {
				objectsByTurnsAfterUnactive.Add (obj, turnsAfterUnactive + 1);
			}
		}

		nextItem = itemGenerator.instantiateRandomItem (itemNextVec);
		nextItem.GetComponent<Rigidbody2D> ().gravityScale = 0;


		if (isGameEnd) {
			foreach (GameObject obj in objectsByTurnsAfterUnactive.Keys) {
				Destroy (obj);
			}
			objectsByTurnsAfterUnactive.Clear ();
			score += 30;
			level++;
		} else {
			score--;
		}
	}

	public UILabel labelScore, labelLevel;

	private void updateLabels() {
		labelScore.text = "Score: " + score;
		labelLevel.text = "Level: " + level;
	}

	// Use this for initialization
	void Start () {
		current = this;

		cleanupTransforms ();
		itemGenerator = this.GetComponent<ItemGenerator> ();

		generateNextItem ();
		generateNextItem ();

		score = 50;
	}
	
	// Update is called once per frame
	void Update () {
		float hval = Input.GetAxis ("Horizontal");
		float vval = Input.GetAxis ("Vertical");

		if (currentItem != null && !nextItemPending) {
			var rigidBody = currentItem.GetComponent<Rigidbody2D> ();
			var newXValue = Mathf.Abs(hval) > 0.001 ? Mathf.Min (8f, Mathf.Max (-8f, rigidBody.velocity.x + hval * 2f)) : rigidBody.velocity.x * 0.7f;
			rigidBody.velocity = new Vector2 (newXValue, rigidBody.velocity.y);

			rigidBody.angularVelocity = rigidBody.angularVelocity + vval * 5f;
		}

		updateLabels ();

		if (score < 0) {
			SceneManager.LoadScene ("GameOverScene");
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

	private void onLevelEnd() {

	}

	private IEnumerator generateNextItemCoroutine() {
		yield return new WaitForSeconds(1.25f / level);
		generateNextItem ();
		nextItemPending = false;
	}
}
