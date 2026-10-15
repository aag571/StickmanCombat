// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InitialButtonSpiker : MonoBehaviour
{
	public Button button;

	private void Start()
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute<IPointerEnterHandler>(this.button.gameObject, eventData, ExecuteEvents.pointerEnterHandler);
		ExecuteEvents.Execute<ISubmitHandler>(this.button.gameObject, eventData, ExecuteEvents.submitHandler);
		ExecuteEvents.Execute<IPointerDownHandler>(this.button.gameObject, eventData, ExecuteEvents.pointerDownHandler);
		ExecuteEvents.Execute<IPointerUpHandler>(this.button.gameObject, eventData, ExecuteEvents.pointerUpHandler);
	}
}
