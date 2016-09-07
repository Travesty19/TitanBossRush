using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public List<Transform> m_targets;

    public float m_distance = 18.6f;
    public float m_catchupSpeed = 1.0f;

    private Camera m_camera;
    private RectTransform m_trackingPoint;

    // Use this for initialization
    void Start()
    {
        m_camera = GetComponent<Camera>();

        GameObject debugCanvas = GameObject.Find("DebugCanvas");
        if (debugCanvas != null)
            m_trackingPoint = debugCanvas.transform.FindChild("Tracking Point") as RectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_targets.Count == 0)
            return;

        Vector3 targetsMin = Vector3.one * float.MaxValue;
        Vector3 targetsMax = Vector3.one * float.MinValue;
        Vector3 targetPosition = Vector3.zero;

        foreach (Transform t in m_targets)
        {
            targetsMin = Vector3.Min(targetsMin, t.position);
            targetsMax = Vector3.Max(targetsMax, t.position);
            targetPosition += t.position;
        }

        targetPosition /= m_targets.Count;

        if (m_trackingPoint != null)
            m_trackingPoint.position = m_camera.WorldToScreenPoint(targetPosition);


        Vector3 screenMin = m_camera.WorldToViewportPoint(targetsMin);
        Vector3 screenMax = m_camera.WorldToViewportPoint(targetsMax);
        float xDistance = screenMax.x - screenMin.x;
        float yDistance = screenMax.y - screenMin.y;
        
        float zoomFactor = 0.0f;
        zoomFactor = Mathf.Max(xDistance - 0.5f, zoomFactor);
        zoomFactor = Mathf.Max(yDistance - 0.5f, zoomFactor);
        
        float distance = m_distance + (100.0f * zoomFactor);
        
        Vector3 velocity = Vector3.zero;
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition - transform.forward * distance, ref velocity, 0.05f);
        transform.position = newPosition;
    }
}
