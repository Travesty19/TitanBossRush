using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public Transform m_target;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_target == null)
            return;

        Vector3 towards = m_target.position - transform.position;
        towards.Normalize();

        float turnSpeed = 45 * Time.deltaTime;
        Quaternion target = Quaternion.LookRotation(towards, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, turnSpeed);
    }
}
