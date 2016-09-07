using UnityEngine;
using System.Collections;

public class XForm : MonoBehaviour
{
    public Vector3 m_translation = Vector3.zero;
    public Vector3 m_rotation = Vector3.zero;
    public Vector3 m_scale = Vector3.zero;

    public Vector2 m_uvs = Vector2.zero;

    private Vector3 m_startTranslation = Vector3.zero;
    private Quaternion m_startRotation = Quaternion.identity;

    public bool m_OverrideStartScale = false;
    public Vector3 m_startScale = Vector3.one;

    // Use this for initialization
    void Start()
    {
        m_startTranslation = transform.localPosition;
        m_startRotation = transform.localRotation;

        if (!m_OverrideStartScale)
        {
            m_startScale = transform.localScale;
        }
        
        ResetTransform();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += m_translation;
        transform.rotation *= Quaternion.Euler(m_rotation);
        transform.localScale += m_scale;

        foreach (Renderer r in transform.GetComponentsInChildren<Renderer>())
        {
            r.material.mainTextureOffset += m_uvs;
        }
    }

    public void ResetTransform()
    {
        transform.localPosition = m_startTranslation;
        transform.localRotation = m_startRotation;
        transform.localScale = m_startScale;
    }
}
