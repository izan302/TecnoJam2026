using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DuckScript : MonoBehaviour
{
    AudioSource m_AudioSource;
    public RectTransform m_Duck;
    public float growSize = 0.2f;
    public float duration = 0.1f;
    
    private Vector3 m_InitialScaleValue; 

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_InitialScaleValue = m_Duck.localScale;
    }

    public void OnMouseDown()
    {
        StopAllCoroutines();
        StartCoroutine(QuackPulse());
    }

    IEnumerator QuackPulse()
    {
        m_AudioSource.Play();
        
        m_Duck.localScale = m_InitialScaleValue + new Vector3(growSize, growSize, 0f);
        
        yield return new WaitForSeconds(duration);
        
        m_Duck.localScale = m_InitialScaleValue;
    }
}