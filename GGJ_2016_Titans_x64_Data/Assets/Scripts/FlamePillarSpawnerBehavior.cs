using UnityEngine;
using System.Collections;

public class FlamePillarSpawnerBehavior : MonoBehaviour
{
    public float m_spawnPeriod = 8.0f;

    private float m_time = 0.0f;


    // Use this for initialization
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;

        if (m_time > m_spawnPeriod)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            GameObject.Instantiate(ResourceModule.Instance.m_prefabs.m_prefFlamePillar, spawnPoint, Quaternion.identity);

            m_time = 0.0f;
        }
    }
}
