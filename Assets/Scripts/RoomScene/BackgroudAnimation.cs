using System.Collections;
using UnityEngine;

public class BackgroudAnimation : MonoBehaviour
{
    [SerializeField] private GameObject m_AllOnLights;
    [SerializeField] private GameObject m_OneOffLight;
    [SerializeField] private GameObject m_AllOffLights;
    private bool m_IsPlaying;
    void Start()
    {
        m_AllOffLights.SetActive(false);
        m_OneOffLight.SetActive(false);
        m_AllOnLights.SetActive(true);
        m_IsPlaying = false;
    }
    void Update()
    {
        if (!m_IsPlaying)
        {
            StartCoroutine(BackgroundRoutine());
        }
    }

    IEnumerator BackgroundRoutine()
    {
        m_IsPlaying = true;
        m_AllOnLights.SetActive(true);
        m_AllOffLights.SetActive(false);
        m_OneOffLight.SetActive(false);
        yield return new WaitForSeconds(3f);
        m_OneOffLight.SetActive(true);
        m_AllOnLights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        m_AllOnLights.SetActive(true);
        m_OneOffLight.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        m_OneOffLight.SetActive(true);
        m_AllOnLights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        m_AllOnLights.SetActive(true);
        m_OneOffLight.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        m_AllOffLights.SetActive(true);
        m_AllOnLights.SetActive(false);
        m_OneOffLight.SetActive(false);
        yield return new WaitForSeconds(1f);
        m_AllOnLights.SetActive(true);
        m_AllOffLights.SetActive(false);
        m_OneOffLight.SetActive(false);
        m_IsPlaying = false;
    }
}
