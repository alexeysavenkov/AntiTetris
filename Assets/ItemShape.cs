using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShape : MonoBehaviour {
	public Vector2[] points;

	public ItemShape rotateClockwise90deg() {
		return new ItemShape {
			points = (
				from pt in points
				select new Vector2 (pt.y, -pt.x)
			).ToArray()
		};
	}

	public ItemShape rotateClockwise90degTimes(int times) {
		if (times < 0)
			throw new UnityException ("Illegal argument of rotateClockwise90degTimes");

		if (times == 0)
			return this;
		else
			return this.rotateClockwise90deg ().rotateClockwise90degTimes (times - 1);
	}
}
