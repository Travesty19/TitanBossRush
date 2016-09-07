using UnityEngine;
using System.Collections;

public class ResourceModule : MonoBehaviour
{
    public static ResourceModule Instance = null;

    public Effects m_effects;
    public Prefabs m_prefabs;

    // Use this for initialization
    void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    public class Effects
    {
        public GameObject m_prefTeleportStart = null;
        public GameObject m_prefBossExplosion = null;
    }

    [System.Serializable]
    public class Prefabs
    {
        public GameObject m_prefFireball = null;
        public GameObject m_prefExplosionWave = null;
        public GameObject m_prefMagmaLaser = null;
        public GameObject m_prefMagmaBeamer = null;
        public GameObject m_prefFlamePillar = null;
        public GameObject m_prefIceBolt = null;
    }
}
