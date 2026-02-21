using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float m_Duration = 0.15f;
    [SerializeField] private float m_Magnitude = 0.3f;

    private Vector3 m_ShakeOffset;
    
    // Configuración del enfado
    private int m_ShakeCount = 0; 
    private float m_LastShakeTime;
    [SerializeField] private int m_ShakesToAngry = 5;
    [SerializeField] private float m_ResetThreshold = 2f; 

    public void PlayCameraShake()
    {
        if (Time.time - m_LastShakeTime > m_ResetThreshold)
        {
            m_ShakeCount = 0;
        }

        m_ShakeCount++;
        m_LastShakeTime = Time.time;

        if (m_ShakeCount >= m_ShakesToAngry)
        {
            FindAnyObjectByType<TutorialStarter>()?.OpenDialogue("<size=120%><color=red>¡PARA YA!</color></size>\n¿NO VES QUE NO PUEDES PONERLO?");
            m_ShakeCount = 0;
        }

        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float l_Elapsed = 0.0f;

        while (l_Elapsed < m_Duration)
        {
            float damp = 1.0f - (l_Elapsed / m_Duration);
            float x = Random.Range(-1f, 1f) * m_Magnitude * damp;
            float y = Random.Range(-1f, 1f) * m_Magnitude * damp;

            Vector3 l_LastOffset = m_ShakeOffset;
            m_ShakeOffset = new Vector3(x, y, 0);

            transform.position += (m_ShakeOffset - l_LastOffset);

            l_Elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position -= m_ShakeOffset;
        m_ShakeOffset = Vector3.zero;
    }
}