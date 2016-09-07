using UnityEngine;
using System.Collections;

public class IceBoltBehavior : MonoBehaviour
{
    public float m_speed = 20.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float step = m_speed * Time.deltaTime;

        RaycastHit[] hit = new RaycastHit[1];
        if (Physics.RaycastNonAlloc(transform.position, transform.forward, hit, step, Layers.WORLD | Layers.ENEMY) > 0)
        {
            GameObject.Destroy(this.gameObject);

            VolcanoBossBehavior boss = hit[0].collider.transform.parent.GetComponent<VolcanoBossBehavior>();
            if (boss != null)
                boss.InflictDamage(1, transform.forward);
        }
        else
        {
            transform.position += transform.forward * step;
        }
    }
}
