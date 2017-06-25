using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Utils : MonoBehaviour {
	public static GameObject[] getChildrenOfGameObject(GameObject obj) {
		return Enumerable.Range (0, obj.transform.childCount)
			.Select (i => obj.transform.GetChild (i).gameObject)
			.ToArray ();
	}
}
