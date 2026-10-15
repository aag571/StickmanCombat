using System;
using UnityEngine;

[Serializable]
public struct CameraShakeSettings
{
    public string Name;
    public Vector3 Strength;
    public float Duration;
    public int Vibrato;
    public float Randomness;
    public float Smoothness;
    public bool UseRandomInitialAngle;
    public float InitialAngle;
    public Vector3 Rotation;
    public bool IgnoreTimeScale;
}

[DisallowMultipleComponent]
[RequireComponent(typeof(CombatCameraFollow))]
public class CombatCameraShake : MonoBehaviour
{
    [Tooltip("默认震屏参数；强度单位为世界坐标单位。")]
    public CameraShakeSettings defaultSettings = new CameraShakeSettings
    {
        Strength = new Vector3(4f, 4f, 0f),
        Duration = 1f,
        Vibrato = 8,
        Randomness = 0.4f,
        Smoothness = 0.1f,
        UseRandomInitialAngle = true,
        IgnoreTimeScale = true
    };

    public Vector3 PositionOffset { get; private set; }
    public Quaternion RotationOffset { get; private set; } = Quaternion.identity;

    private CameraShakeSettings activeSettings;
    private Vector3 offsetVelocity;
    private float elapsed;
    private float initialAngle;
    private bool shaking;

    public void Shake()
    {
        Shake(defaultSettings);
    }

    public void Shake(CameraShakeSettings settings)
    {
        if (!isActiveAndEnabled || settings.Duration <= 0f)
            return;

        activeSettings = settings;
        initialAngle = (settings.UseRandomInitialAngle ? UnityEngine.Random.Range(0f, 360f) : settings.InitialAngle) * Mathf.Deg2Rad;
        elapsed = 0f;
        offsetVelocity = Vector3.zero;
        shaking = true;
    }

    public void Advance()
    {
        if (!shaking)
            return;

        float deltaTime = activeSettings.IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
        if (deltaTime <= 0f)
            return;

        elapsed += deltaTime;
        if (elapsed >= activeSettings.Duration)
        {
            ResetShake();
            return;
        }

        float progress = elapsed / activeSettings.Duration;
        float envelope = (1f - progress) * (1f - progress);
        float phase = progress * Mathf.Max(1, activeSettings.Vibrato) * Mathf.PI * 2f;
        float angle = initialAngle + Mathf.Sin(phase * 0.73f) * Mathf.Clamp01(activeSettings.Randomness) * Mathf.PI;
        float wave = Mathf.Sin(phase) * envelope;
        Vector3 desiredOffset = new Vector3(Mathf.Cos(angle) * activeSettings.Strength.x, Mathf.Sin(angle) * activeSettings.Strength.y, 0f) * wave;
        PositionOffset = Vector3.SmoothDamp(PositionOffset, desiredOffset, ref offsetVelocity, Mathf.Max(0f, activeSettings.Smoothness), Mathf.Infinity, deltaTime);
        RotationOffset = Quaternion.Euler(activeSettings.Rotation * wave);
    }

    private void OnDisable()
    {
        ResetShake();
    }

    private void ResetShake()
    {
        shaking = false;
        PositionOffset = Vector3.zero;
        RotationOffset = Quaternion.identity;
        offsetVelocity = Vector3.zero;
    }
}
