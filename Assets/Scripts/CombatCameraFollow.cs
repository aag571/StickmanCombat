using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class CombatCameraFollow : MonoBehaviour
{
    [Tooltip("相机跟随的目标，剧情聚焦时可移动此目标。")]
    public Transform target;

    public Vector2 targetOffset = new Vector2(0f, 2.5f);
    public Vector2 OverallOffset;
    public bool centerOnStart = true;
    public bool followHorizontal = true;
    public bool followVertical = true;
    [Min(0f)] public float horizontalSmoothness = 0.15f;
    [Min(0f)] public float verticalSmoothness = 0.15f;

    private Vector2 followPosition;
    private Vector2 followVelocity;
    private Vector3 appliedShake;
    private Quaternion baseRotation;
    private CombatCameraShake cameraShake;

    private void OnEnable()
    {
        followPosition = transform.position;
        followVelocity = Vector2.zero;
        baseRotation = transform.rotation;
        cameraShake = GetComponent<CombatCameraShake>();
    }

    private void Start()
    {
        if (centerOnStart && target != null)
        {
            Vector2 desiredPosition = (Vector2)target.position + targetOffset + OverallOffset;
            if (followHorizontal) followPosition.x = desiredPosition.x;
            if (followVertical) followPosition.y = desiredPosition.y;
            transform.position = new Vector3(followPosition.x, followPosition.y, transform.position.z);
        }
    }

    private void LateUpdate()
    {
        if (target != null && Time.deltaTime > 0f)
        {
            Vector2 desiredPosition = (Vector2)target.position + targetOffset + OverallOffset;
            if (followHorizontal)
                followPosition.x = Mathf.SmoothDamp(followPosition.x, desiredPosition.x, ref followVelocity.x, horizontalSmoothness, Mathf.Infinity, Time.deltaTime);
            if (followVertical)
                followPosition.y = Mathf.SmoothDamp(followPosition.y, desiredPosition.y, ref followVelocity.y, verticalSmoothness, Mathf.Infinity, Time.deltaTime);
        }

        if (cameraShake != null && cameraShake.isActiveAndEnabled)
            cameraShake.Advance();

        appliedShake = cameraShake != null && cameraShake.isActiveAndEnabled ? cameraShake.PositionOffset : Vector3.zero;
        transform.position = new Vector3(followPosition.x + appliedShake.x, followPosition.y + appliedShake.y, transform.position.z);
        transform.rotation = baseRotation * (cameraShake != null && cameraShake.isActiveAndEnabled ? cameraShake.RotationOffset : Quaternion.identity);
    }

    private void OnDisable()
    {
        transform.position -= appliedShake;
        transform.rotation = baseRotation;
        appliedShake = Vector3.zero;
    }
}
