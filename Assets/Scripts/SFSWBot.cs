// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class SFSWBot : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (CharacterManager.instance.currentState != CharacterManager.State.Idle)
		{
			return;
		}
		if (!SceneManager.instance.gameStarted)
		{
			return;
		}
		int num = -1;
		if (MonetizationManager.instance.coinSpawned)
		{
			num = MonetizationManager.instance.coinSide;
		}
		LinkedList<Npc> linkedList;
		LinkedList<Npc> linkedList2;
		if (num == -1)
		{
			linkedList = NpcManager.instance.leftNpcs;
			linkedList2 = NpcManager.instance.rightNpcs;
		}
		else
		{
			linkedList = NpcManager.instance.rightNpcs;
			linkedList2 = NpcManager.instance.leftNpcs;
		}
		if (linkedList.First != null && linkedList.First.Value.markedForKill)
		{
			CharacterManager.instance.inputQueue.Enqueue(num);
			return;
		}
		if (linkedList.First == null)
		{
			CharacterManager.instance.inputQueue.Enqueue(num);
			return;
		}
		if ((float)num * (linkedList.First.Value.transform.position.x + linkedList.First.Value.speed * (float)(-(float)num) * 0.4f) > (float)num * (CharacterManager.instance.transform.position.x + CharacterManager.instance.baseRange * (float)num))
		{
			CharacterManager.instance.inputQueue.Enqueue(num);
			return;
		}
		if (linkedList2.First != null && Mathf.Abs(CharacterManager.instance.transform.position.x - linkedList2.First.Value.transform.position.x) < 0.6f)
		{
			CharacterManager.instance.inputQueue.Enqueue(-num);
			return;
		}
	}
}
