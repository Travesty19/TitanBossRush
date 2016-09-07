using UnityEngine;
using System.Collections;

public class MagmaBeamerBehavior : MonoBehaviour
{
    public float m_gravity = 1.0f;
    public float m_airDrag = 1.0f;
    public Vector3 m_velocity = Vector3.zero;

    public float m_windUpTime = 1.0f;
    public float m_beamTime = 1.0f;
    public float m_windDownTime = 1.0f;
    public float m_laserSpeed = 1.0f;



    private int m_bounces = 0;

    private int m_state = 0;
    private float m_time = 0;
    private Vector3 m_targetPosition = Vector3.zero;

    private Transform m_visual = null;
    private Transform m_laserBeam = null;
    private Transform m_laserHandle = null;



    // Use this for initialization
    void Start()
    {
		
        m_visual = transform.FindChild("visual");
        m_laserBeam = transform.FindChild("laser");
        m_laserHandle = m_laserBeam.FindChild("handle");
        m_laserBeam.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsMain();
        StateMain();


    }

    private void PhysicsMain()
    {
        if (m_state != 0)
            return;

        Vector3 lateralMotion = Vector3.Scale(m_velocity, new Vector3(1.0f, 0.0f, 1.0f));
        Vector3 lateralDirection = Vector3.Normalize(lateralMotion);

        float lateralSpeed = Vector3.Dot(m_velocity, lateralDirection);
        float dampening = Mathf.Min(m_airDrag * Time.deltaTime, lateralSpeed);

        m_velocity -= Vector3.up * m_gravity * Time.deltaTime;
        m_velocity -= lateralDirection * dampening;

        Vector3 lastPosition = transform.position;
        transform.position += m_velocity * Time.deltaTime;

        Ray ray = new Ray(lastPosition, Vector3.Normalize(transform.position - lastPosition));
        RaycastHit[] hits = OptimizedCast.SphereCastAll(ray, 0.5f, Vector3.Distance(transform.position, lastPosition), Layers.WORLD);
        if (hits.Length > 0)
        {
            float reboundVelocity = -m_velocity.y * 0.6f;

            if (reboundVelocity > 0.2f && m_bounces < 2)
            {
                transform.position = hits[0].point;
                m_velocity.y = reboundVelocity;
                m_bounces++;
            }
            else
            {
                transform.position = hits[0].point;
                m_velocity = Vector3.zero;
                ChangeState(1);
            }
        }
    }

    private void StateMain()
    {
        if (m_state == 0) // Physics
        {

        }
        else if (m_state == 1) // Wind-up
        {
            m_time += Time.deltaTime;

            float t = m_time / m_windUpTime;
            m_visual.GetComponent<XForm>().m_rotation.y = 15 * Mathf.Min(t, 1.0f);

            if (m_time > m_windUpTime)
            {
                m_laserBeam.gameObject.SetActive(true);
                m_laserBeam.GetComponent<MagmaLaserBehavior>().m_thickness = 1.0f;

                ChangeState(2);
            }
        }
        else if (m_state == 2) // Laser Start
        {
            m_time += Time.deltaTime;

            Vector3 playerPosition = PlayerController.Instance.transform.position;
            Vector3 playerOffset = playerPosition - transform.position;
            Vector3 direction = Vector3.Normalize(Vector3.Scale(playerOffset, new Vector3(1.0f, 0.0f, 1.0f)));

            Vector3 targetPoint = transform.position;
            targetPoint += Vector3.down * 0.1f;
            targetPoint += direction * 2;

            float t = m_time / 0.2f;

            m_laserHandle.position = Vector3.Lerp(transform.position, targetPoint, t);
            if (m_time > 0.2f)
            {
                m_targetPosition = playerPosition;
                m_targetPosition.y -= 0.1f;

                ChangeState(3);
            }
        }
        else if (m_state == 3) // Laser
        {
			
            m_time += Time.deltaTime;

            Vector3 playerPosition = PlayerController.Instance.transform.position;
            Vector3 targetOffset = playerPosition - transform.position;
            targetOffset.y = 0.0f;

            Vector3 direction = Vector3.Normalize(targetOffset);

            m_laserHandle.position += direction * m_laserSpeed * Time.deltaTime;

            if (m_time > m_beamTime)
                ChangeState(4);
        }
        else if (m_state == 4) // Wind-down
        {
            m_time += Time.deltaTime;

            float t = m_time / m_windDownTime;
            m_visual.GetComponent<XForm>().m_rotation.y = 15 * (1.0f - Mathf.Min(t, 1.0f));

            MagmaLaserBehavior laserBehavior = m_laserBeam.GetComponent<MagmaLaserBehavior>();
            laserBehavior.m_thickness = (1.0f - Mathf.Min(t, 1.0f));

            if (m_time > m_windUpTime)
            {
                m_laserHandle.position = m_laserBeam.transform.position;

                ChangeState(1);
            }
        }
    }


    private void ChangeState(int state)
    {
        m_state = state;
        m_time = 0;
    }
}
