using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance = null;
    public static PlayerController Instance
    {
        get
        {
            return instance;
        }
    }

    private Animator m_animatorController = null;
    public Material m_material = null;
    public Camera _CurrentCamera = null;

    private PlayerState m_currentState = null;

    public float m_height = 3;
    public float m_radius = 2;
    public float m_moveSpeed = 10;
    public float m_groundingAngle = Mathf.Deg2Rad * 180;

    private Vector3 m_lastPosition = Vector3.zero;
    private const int COLLISION_ITERATIONS = 3;


    void Start()
    {
        if (instance != null)
            GameObject.Destroy(instance);

        instance = this;
        m_animatorController = gameObject.GetComponent<Animator>();
        //m_material = GetComponent<MeshRenderer>().material;

        if (_CurrentCamera == null)
            _CurrentCamera = Camera.main;

        m_lastPosition = transform.position;
        
        ChangeState(new PlayerStateGreen());
    }


    void Update()
    {
        InputMain();
        StateMain();
        WallCollisionMain();
        DamageMain();

        m_lastPosition = transform.position;
    }

    void InputMain()
    {
        Vector3 camForward = _CurrentCamera.transform.forward;
        Vector3 camDirection = Vector3.Normalize(Vector3.Scale(camForward, new Vector3(1, 0, 1)));

        float inputMagnitude = InputHelper.GetInputMagnitude();
        float inputAngle = InputHelper.GetInputAngle();

        Quaternion inputQuat = Quaternion.AngleAxis(inputAngle, Vector3.up);
        Quaternion moveQuat = Quaternion.LookRotation(camDirection, Vector3.up) * inputQuat;
        Vector3 moveDirection = moveQuat * Vector3.forward;
        
        float moveMagnitude = inputMagnitude * Time.deltaTime;
        
        if (moveMagnitude > 0)
        {
            transform.position += moveDirection * moveMagnitude * m_moveSpeed;
            transform.rotation = moveQuat;

            m_animatorController.SetBool("Moving", true);
            m_animatorController.SetFloat("Blend", inputMagnitude);
        }
        else
        {
            m_animatorController.SetBool("Moving", false);
        }


        /*
        if (Input.GetButtonDown("ButtonA"))
        {
            ChangeState(new PlayerStateGreen());
        }
        else if (Input.GetButtonDown("ButtonB"))
        {
            ChangeState(new PlayerStateRed());
        }
        else if (Input.GetButtonDown("ButtonX"))
        {
            ChangeState(new PlayerStateBlue());
        }
        else if (Input.GetButtonDown("ButtonY"))
        {
            ChangeState(new PlayerStateYellow());
        }*/
        if (Input.GetButtonDown("TriggerL"))
        {
            if (m_currentState.Defend())
            {
                m_animatorController.SetBool("Defending", true);
            }
        }
        else if (Input.GetButtonDown("TriggerR"))
        {
            if (m_currentState.Attack())
            {
                m_animatorController.SetBool("Attacking", true);
            }
        }
    }

    void StateMain()
    {
        m_currentState.UpdateState();
    }

    void ChangeState(PlayerState nextState)
    {
        if (m_currentState != null)
            m_currentState.Exit();

        m_currentState = nextState;

        if (m_currentState != null)
            m_currentState.Enter();
    }

    void WallCollisionMain()
    {
        RaycastHit lastHit = new RaycastHit() { normal = Vector3.zero };

        for (int i = 0; i < COLLISION_ITERATIONS; i++)
        {
            Vector3 capA = m_lastPosition + Vector3.up * m_radius;
            Vector3 capB = m_lastPosition + Vector3.up * m_height + Vector3.up * m_radius;
            Vector3 rayDir = Vector3.Normalize(transform.position - m_lastPosition);
            float rayDist = Vector3.Distance(transform.position, m_lastPosition);

            RaycastHit[] hits = OptimizedCast.CapsuleCastAll(capA, capB, m_radius, rayDir, rayDist, Layers.WORLD);
            hits = hits.Where(x => Mathf.Abs(x.normal.y) < 0.5f && x.collider != lastHit.collider).ToArray();

            if (hits.Length == 0)
                break;
            
            RaycastHit nearest = hits.OrderBy(x => x.distance).First();
            Vector3 wallNormal = Vector3.Normalize(Vector3.Scale(nearest.normal, new Vector3(1, 0, 1)));
            Vector3 correctedNormal = Vector3.Normalize(wallNormal + lastHit.normal * Vector3.Dot(nearest.normal, lastHit.normal));
            
            Vector3 hitLocation = m_lastPosition + rayDir * nearest.distance;
            float penetrationDistance = Vector3.Dot(correctedNormal, hitLocation - transform.position);

            transform.position += wallNormal * penetrationDistance;
            lastHit = nearest;
        }
    }

    private void DamageMain()
    {
        Collider[] hit = new Collider[1];

        if (Physics.OverlapSphereNonAlloc(transform.position, m_radius, hit, Layers.DAMAGE) > 0)
        {
            KillMe();
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 value)
    {
        transform.position = value;
        m_lastPosition = value;
    }

    public void SetColor(Color color)
    {
        m_material.color = color;
    }

    public void KillMe()
    {
        Application.LoadLevel(0);
    }
}