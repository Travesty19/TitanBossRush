using UnityEngine;
using System.Collections;

public class FlamePillarBehavior : MonoBehaviour
{
    public float m_signalTime = 1.0f;
    public float m_burstTime = 1.0f;

	private bool soundOne;
	private float soundTime = 75;

    private Vector3 m_scaleFrom = Vector3.zero;
    private Vector3 m_scaleTo = Vector3.one;

    private float m_time = 0.0f;
    private int m_state = 0;

    private Transform m_outerBurst = null;
	public AudioClip flameBurst;
	private AudioSource source;
    private Transform m_collider = null;

    // Use this for initialization
    void Start()
    {
		source = GetComponent<AudioSource>();
        Vector3 scale = transform.localScale;
        m_scaleTo = scale;
        m_scaleFrom = scale;
        m_scaleFrom.y = 0.01f;

        transform.localScale = m_scaleFrom;

        m_outerBurst = transform.FindChild("FlamePillarBurstOuter");
        m_outerBurst.gameObject.SetActive(false);

        m_collider = transform.FindChild("collider");
        m_collider.GetComponent<CapsuleCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_state == 0) // Signal
        {
            m_time += Time.deltaTime;

            if (m_time > m_signalTime)
            {
                m_outerBurst.gameObject.SetActive(true);
                ChangeState(1);
            }

			if (soundTime <= 0) {
				if (soundOne == false) {
					source.PlayOneShot (flameBurst, 1.0f);
					soundOne = true;
				}
			} else {
				soundTime--;
			}
        }
        else if (m_state == 1) // Burst
        {
            m_collider.GetComponent<CapsuleCollider>().enabled = true;

			m_time += Time.deltaTime;

            float t = Mathf.Min(m_time / 0.1f, 1.0f);
            transform.localScale = Vector3.Lerp(m_scaleFrom, m_scaleTo, t);

            if (m_time > m_burstTime)
                ChangeState(2);
        }
        else if (m_state == 2) // Die out
        {
            m_time += Time.deltaTime;

            Vector3 scaleFrom = m_scaleTo;
            Vector3 scaleTo = Vector3.Scale(scaleFrom, new Vector3(0.0f, 1.0f, 0.0f));

            float t = Mathf.Min(m_time / 0.3f, 1.0f);
            transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, t);
            m_collider.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, t);

            if (m_time > 0.3f)
                GameObject.Destroy(this.gameObject);
        }
    }

    private void ChangeState(int value)
    {
        m_state = value;
        m_time = 0;
    }
}
