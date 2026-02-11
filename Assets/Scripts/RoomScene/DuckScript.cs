using UnityEngine;
[RequireComponent (typeof(AudioSource))]
public class DuckScript : MonoBehaviour
{
    AudioSource m_AudioSource;
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    private void OnMouseDown()
    {
        m_AudioSource.Play();
    }
}
