// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class RoomsEventsExample : MonoBehaviour
{
	public void StartedRoomTransition(int currentRoom, int previousRoom)
	{
		UnityEngine.Debug.Log(string.Format("开始切换房间 - 当前房间：{0} - 上一个房间：{1}", currentRoom, previousRoom));
	}

	public void FinishedRoomTransition(int currentRoom, int previousRoom)
	{
		UnityEngine.Debug.Log(string.Format("房间切换完成 - 当前房间：{0} - 上一个房间：{1}", currentRoom, previousRoom));
	}
}
