using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

    public ParticleSystem fogSystem;

    public ParticleSystem leafSystem;

    public MenuToggle weatherToggler;
    // 初始化组件
    void Start () {
		
	}
	
	// 每帧更新
	void Update () {
        if (!this.weatherToggler.isOn)
        {
            if (this.fogSystem.isPlaying)
            {
                this.fogSystem.Stop();
            }
            if (this.leafSystem.isPlaying)
            {
                this.leafSystem.Stop();
            }

        }
        if (this.weatherToggler.isOn)
        {
            if (this.fogSystem.isStopped)
            {
                this.fogSystem.Play();
            }
            if (this.leafSystem.isStopped)
            {
                this.leafSystem.Play();
            }

        }
    }
}
