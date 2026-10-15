// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_ObjectMovement : LevelStep
{
	public GameObject objectToMove;

	public Vector3 targetPosition;

	public float duration;

	private bool deactivateWhenComplete;

	private Vector3 originalPosition;

	private Vector3 step;

	private float elapsedTime;

	public LevelStep_ObjectMovement(GameObject objectToMove, float duration, bool deactivateWhenComplete = false)
	{
		this.objectToMove = objectToMove;
		this.duration = duration;
		this.deactivateWhenComplete = deactivateWhenComplete;
	}

	public override void Start()
	{
		base.Start();
		this.targetPosition = new Vector3(-2f, 0f, 0f) + new Vector3(StoryManager.instance.GetWorldPositionOnPlane(new Vector3((float)Screen.width, 0f, 0f)).x, CharacterManager.instance.transform.position.y, CharacterManager.instance.transform.position.z);
		this.originalPosition = this.objectToMove.transform.position;
		this.step = (this.targetPosition - this.objectToMove.transform.position) / this.duration;
	}

	public override bool Update()
	{
		base.Update();
		this.elapsedTime += Time.deltaTime;
		if (this.elapsedTime > this.duration)
		{
			this.objectToMove.transform.position = this.targetPosition;
			if (this.deactivateWhenComplete)
			{
				this.objectToMove.SetActive(false);
			}
			return true;
		}
		this.objectToMove.transform.position = this.step * this.elapsedTime + this.originalPosition;
		return false;
	}
}
