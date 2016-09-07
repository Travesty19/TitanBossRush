using UnityEngine;
using System.Collections;
using System.Linq;

public class CastShadow : MonoBehaviour
{
    public Transform m_target = null;

    // Use this for initialization
    void Start()
    {
        if (m_target == null)
            m_target = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 elevatedSource = m_target.position + Vector3.up * 0.0001f;
        RaycastHit[] hits = Physics.RaycastAll(elevatedSource, Vector3.down, float.PositiveInfinity, Layers.WORLD);

        if (hits.Length > 0)
        {
            RaycastHit nearest = hits.OrderBy(x => x.distance).First();

            transform.position = elevatedSource + Vector3.down * nearest.distance;
            transform.rotation = Quaternion.identity;

            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = true;
        }
        else
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = false;
        }
    }
}
