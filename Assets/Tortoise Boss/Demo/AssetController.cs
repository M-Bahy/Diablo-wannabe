using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetController : MonoBehaviour
{
	public Material[] materials;
	AnimationClip[] clips;
	Animator anim;

	int m_current_AnimStep;
	int current_AnimStep
	{
		get
		{
			return m_current_AnimStep;
		}

		set
		{
			if (value >= clips.Length)
				value = 0;
			else if (value < 0)
				value = clips.Length - 1;

			m_current_AnimStep = value;
		}
	}


	int m_current_MatStep;
	int current_MatStep
	{
		get
		{
			return m_current_MatStep;
		}

		set
		{
			if (value >= materials.Length)
				value = 0;
			else if (value < 0)
				value = materials.Length - 1;

			GetComponent<Turtle_AnimationEvents>().materialType = value;
			m_current_MatStep = value;
		}
	}

	Renderer rend;
	// Use this for initialization
	void Start ()
	{
		rend = GetComponentInChildren<SkinnedMeshRenderer>();
		anim = GetComponent<Animator>();
		clips = anim.runtimeAnimatorController.animationClips;
	}

	public Text animationText;
	public void AnimationStep(int val)
	{
        current_AnimStep += val;

        anim.Play(clips[current_AnimStep].name);
		animationText.text = clips[current_AnimStep].name;
	}

	public void MaterialStep(int val)
	{
		rend.sharedMaterial = materials[current_MatStep++];
	}

    public void Rotate(float f)
    {
        transform.Rotate(new Vector3(0, f, 0) * 20);
    }
}
