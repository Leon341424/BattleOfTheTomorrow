using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform target1;
    private Transform target2;
    public float smoothSpeed;
    public Vector3 offset; 
    public float minZoomZ;
    public float maxZoomZ;
    public float zoomLimiter;

    void Start()
    {
        Invoke("FindTargets", 0.1f); 
    }

    void FindTargets()
    {
        GameObject personaje1 = GameObject.FindWithTag("Player");
        GameObject personaje2 = GameObject.FindWithTag("Enemy");

        target1 = personaje1.transform;
        target2 = personaje2.transform;
    }
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
