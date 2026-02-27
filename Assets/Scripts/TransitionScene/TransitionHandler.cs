using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] GameObject Letter;
    [SerializeField] GameObject solapaEnvelope;
    [SerializeField] GameObject folio;

    [Header("CartaFinal")]
    [SerializeField] GameObject letterFinal;
    [SerializeField] GameObject paperNotificationRejected;
    [SerializeField] GameObject paperNotificationAproved;
    [SerializeField] TextMeshProUGUI letterText;

    [Header("Animacion Solapa")]
    [SerializeField] AnimationCurve flapRotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] float flapRotationDuration = 1f;
    [SerializeField] Vector3 flapRotationAxis = new Vector3(1, 0, 0);

    [Header("Animacion Folio")]
    [SerializeField] AnimationCurve folioMoveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] float folioMoveDuration = 1f;
    [SerializeField] float folioUpwardDistance = 2f;
    [Header("Animación Carta Final")]
    [SerializeField] AnimationCurve letterMoveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] float letterMoveDuration = 1f;
    [SerializeField] float targetYPosition = 6.4f;

    [SerializeField] string[] m_KeysToPlay;
    [Header("Letter Before Click Animation")]
    private RectTransform m_Letter;
    private Vector3 m_InitialLetterScale;
    [SerializeField] private float m_GrowSize = 0.2f;
    [SerializeField] private float m_Duration = 0.1f;

    TypewriterEffect typewriter;
    bool showAproved = false;
    bool isAnimating = false;
    bool isTextFinished = false;
    bool isEnvelopeOpen = false;

    Coroutine m_LetterPulse;

    private void Start()
    {
        showAproved = GabeNewell.Instance.m_Level() == 6;
        typewriter = GetComponent<TypewriterEffect>();
        typewriter.OnTextFinished += TextFinished;
        Letter.SetActive(true);
        folio.SetActive(true);
        m_Letter = Letter.GetComponent<RectTransform>();
        m_InitialLetterScale = m_Letter.localScale;
    }

    private void Update()
    {
        if (!isEnvelopeOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isTextFinished)
                {
                    typewriter.Skip();
                }
                else
                {
                    StartNextScene();
                }
            }
        }else
        {
            /*
            if (m_LetterPulse != null)
            {
                m_LetterPulse = StartCoroutine(LetterPulse());
            }else
            {
                StopCoroutine(LetterPulse());
            }
            */
        }

    }
    /*
    IEnumerator LetterPulse()
    {
        m_Letter.localScale = m_InitialLetterScale + new Vector3(m_GrowSize, m_GrowSize, 0f);
        yield return new WaitForSeconds(m_Duration);
        m_Letter.localScale = m_InitialLetterScale;
    }
    */
    void TextFinished()
    {
        isTextFinished = true;
    }

    public void OnEnvelopeOpened()
    {
        if (isAnimating) return;
        Letter.GetComponent<Button>().enabled = false;
        StartCoroutine(OpeningEnvelope());
    }
    IEnumerator OpeningEnvelope()
    {
        isAnimating = true;
        float timeElapsed = 0f;

        Quaternion startRot = solapaEnvelope.transform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(flapRotationAxis * 180f);

        while (timeElapsed < flapRotationDuration)
        {
            float t = timeElapsed / flapRotationDuration;
            float curveValue = flapRotationCurve.Evaluate(t);
            solapaEnvelope.transform.localRotation = Quaternion.LerpUnclamped(startRot, endRot, curveValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        solapaEnvelope.transform.localRotation = endRot;

        solapaEnvelope.transform.SetSiblingIndex(0);


        timeElapsed = 0f;
        Vector3 startPos = folio.transform.localPosition;
        Vector3 endPos = startPos + (Vector3.up * folioUpwardDistance);
        while (timeElapsed < folioMoveDuration)
        {
            float t = timeElapsed / folioMoveDuration;
            float curveValue = folioMoveCurve.Evaluate(t);

            folio.transform.localPosition = Vector3.LerpUnclamped(startPos, endPos, curveValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        folio.transform.localPosition = endPos;
        StartCoroutine(ShowLetter(0.5f));
    }
    IEnumerator ShowLetter(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);

        letterFinal.SetActive(true);
        if (showAproved)
        {
            paperNotificationAproved.SetActive(true);
            paperNotificationRejected.SetActive(false);
        }
        else
        {
            paperNotificationAproved.SetActive(false);
            paperNotificationRejected.SetActive(true);
        }

        RectTransform rect = letterFinal.GetComponent<RectTransform>();

        float timeElapsed = 0f;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, 6f);

        while (timeElapsed < letterMoveDuration)
        {
            float t = timeElapsed / letterMoveDuration;
            float curveValue = letterMoveCurve.Evaluate(t);

            rect.anchoredPosition =
                Vector2.LerpUnclamped(startPos, endPos, curveValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = endPos;

        isAnimating = false;

        if (showAproved)
        {
            typewriter.PlayText("gameSent_accepted", letterText);
        }
        else
        {
            typewriter.PlayText("gameSent_rejected", letterText);
        }
    }
    public void StartNextScene()
    {
        GabeNewell.Instance.GoToDesktop();
    }
}
