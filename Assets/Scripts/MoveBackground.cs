using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour {

    public float speed = 0;
    float offsetX = 0;
    // 初始化组件
    void Start () {
		
	}
	
	// 每帧更新
	void Update () {
        offsetX = Time.deltaTime * speed;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x - offsetX, this.transform.localPosition.y, this.transform.localPosition.z);
	}
}
