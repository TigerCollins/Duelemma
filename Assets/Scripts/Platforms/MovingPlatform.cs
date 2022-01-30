using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public AnimationCurve travelCurve = AnimationCurve.EaseInOut (0, 0, 1, 1);
	public Vector3 pos1;
	public Vector3 pos2;
	public float timeOffset = 0;
	public float travelTime = 2f;
	public bool positionRelative;
	Transform trans;
	[HideInInspector]
	public Vector2 velocity;


	void Start () {
		if (positionRelative) {
			Vector3 offset = transform.position;
			pos1 += offset;
			pos2 += offset;
			positionRelative = false;
		}
		trans = transform;
	}

	void FixedUpdate () {
		Vector3 prevPos = trans.position;
		trans.position = (Vector3.Lerp (pos1, pos2, travelCurve.Evaluate (Mathf.PingPong ((Time.time + timeOffset) / travelTime, 1))));
		velocity = (-prevPos + trans.position) / Time.fixedDeltaTime;
	}

	private void OnDrawGizmos () {
		if (positionRelative) {
			Gizmos.DrawSphere ((Vector3)pos1 + transform.position, 0.2f);
			Gizmos.DrawSphere ((Vector3)pos2 + transform.position, 0.2f);
			Gizmos.DrawLine ((Vector3)pos1 + transform.position, (Vector3)pos2 + transform.position);
		} else {
			Gizmos.DrawSphere (pos1, 0.2f);
			Gizmos.DrawSphere (pos2, 0.2f);
			Gizmos.DrawLine (pos1, pos2);
		}
	}
}