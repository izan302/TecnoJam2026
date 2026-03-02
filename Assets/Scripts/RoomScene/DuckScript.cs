using System.Collections;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(AudioSource))]
public class DuckScript : MonoBehaviour
{
    public RectTransform m_Duck;
    public float growSize = 0.2f;
    public float duration = 0.1f;

    [SerializeField] private EventReference m_DuckSound;



    private Vector3 m_InitialScaleValue; 

    private void Start()
    {
        m_InitialScaleValue = m_Duck.localScale;
    }

    public void OnMouseDown()
    {
        StopAllCoroutines();
        StartCoroutine(QuackPulse());
    }

    IEnumerator QuackPulse()
    {
        RuntimeManager.PlayOneShot(m_DuckSound, transform.position);

        m_Duck.localScale = m_InitialScaleValue + new Vector3(growSize, growSize, 0f);
        
        yield return new WaitForSeconds(duration);
        
        m_Duck.localScale = m_InitialScaleValue;
    }
}