using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target1;
    public Transform target2;
    public float smoothSpeed;
    public Vector3 offset; 
    public float minZoomZ;
    public float maxZoomZ;
    public float zoomLimiter;
    
    void LateUpdate() {
        Vector3 middlePoint = (target1.position + target2.position) / 2;
        
        float distance = Vector3.Distance(target1.position, target2.position);

        float targetZ = Mathf.Lerp(minZoomZ, maxZoomZ, distance / zoomLimiter);
        
        Vector3 desiredPosition = new Vector3(
            middlePoint.x + offset.x,
            middlePoint.y + offset.y,
            targetZ + offset.z
        );

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
