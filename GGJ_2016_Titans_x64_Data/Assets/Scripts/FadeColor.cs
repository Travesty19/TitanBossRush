using UnityEngine;
using System.Collections;

public class FadeColor : MonoBehaviour
{
    public Color m_fromColor = Color.white;
    public Color m_toColor = Color.white;

    public AnimationCurve m_controlCurve;
    public float m_totalTime = 0;
    private float m_time = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_totalTime == 0)
            return;

        m_time += Time.fixedDeltaTime;

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            float t = m_controlCurve.Evaluate(Mathf.Clamp(m_time / m_totalTime, 0, 1));
            Color c = Color.Lerp(m_fromColor, m_toColor, t);

            if (r.material.HasProperty("_Color"))
                r.material.color = c;

            if (r.material.HasProperty("_TintColor"))
                r.material.SetColor("_TintColor", c);
        }
    }
}
