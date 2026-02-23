
using UnityEngine;

[ExecuteAlways]
public class BillBoarding_Behavior : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private bool reverseFacing;

    void LateUpdate()
    {
        if (!targetCamera) targetCamera = Camera.main;
        if (!targetCamera) return;

        Vector3 camForward = targetCamera.transform.forward;
        camForward.y = 0f; // lock to Y axis
        if (camForward.sqrMagnitude < 0.0001f) return;

        camForward.Normalize();

        if (reverseFacing)
            transform.rotation = Quaternion.LookRotation(-camForward, Vector3.up);
        else
            transform.rotation = Quaternion.LookRotation(camForward, Vector3.up);
    }
}

