using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Particles
{
	public Transform[] list;
}
public class Turtle_AnimationEvents : MonoBehaviour
{
	public int materialType;

	public Transform[] positions;

	public List<Particles> particles = new List<Particles>();
	// Use this for initialization
	void Start()
	{
		/*
		* ANIMATION EVENTS
		* */
		// MINING ANIMATION
		AddEvent("Walk", 0.7f, 0, 0, true);
		AddEvent("Walk", 0f, 0, 1, true);
		AddEvent("Walk", 1.55f, 0, 2, true);
		AddEvent("Walk", 0.65f, 0, 3, true);
		AddEvent("Attack3", 0.8f, 1, 4, false);
		AddEvent("Attack2", 1.7f, 2, 5, false);
	}

	void AddEvent(string animName, float time, int val, float posid, bool attach)
	{
		AnimationEvent evt = new AnimationEvent();
		evt.stringParameter = val + "," + posid + "," + attach;
		evt.time = time;
		evt.functionName = "AnimEvent";

		foreach (AnimationClip ac in GetComponent<Animator>().runtimeAnimatorController.animationClips)
			if (ac.name == animName) ac.AddEvent(evt);
	}

	void AnimEvent(string val)
	{
		string[] vals = val.Split(',');
		Transform t = Instantiate(particles[materialType].list[int.Parse (vals[0])], positions[int.Parse (vals[1])].position, transform.rotation);

		if (bool.Parse (vals[2])) t.SetParent(positions[int.Parse(vals[1])]);
		Destroy(t.gameObject, 5);
	}
}
