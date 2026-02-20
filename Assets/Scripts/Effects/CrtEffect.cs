using UnityEngine;

public class CrtEffect : MonoBehaviour
{
    [SerializeField] private GameObject m_CrtLinePrefab;
    [SerializeField] private GameObject m_Spawner;
    [SerializeField] private float m_SpawnCooldown = 4f;
    [SerializeField] private float m_CrtSpeed = 150f;
    [SerializeField] private Transform m_DestroyPoint;
    private float m_CooldownTimer;

    void Update()
    {
        if (!GabeNewell.Instance.m_CrtEffect) return;

        m_CooldownTimer += Time.deltaTime;
        if (m_CooldownTimer >= m_SpawnCooldown)
        {
            SpawnLine();
            m_CooldownTimer = 0;
        }
    }

    private void SpawnLine()
    {
        GameObject l_Line = Instantiate(m_CrtLinePrefab, m_Spawner.transform, false);
        
        CrtLine lineScript = l_Line.GetComponent<CrtLine>();
        if (lineScript != null)
        {
            lineScript.Initialize(m_CrtSpeed, m_DestroyPoint);
        }
    }
}