using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Keyboard and Mouse Controls")]
    [SerializeField] private KeyCode k_up = KeyCode.W;
    [SerializeField] private KeyCode k_left = KeyCode.A;
    [SerializeField] private KeyCode k_right = KeyCode.D;
    [SerializeField] private KeyCode k_down = KeyCode.S;
    [SerializeField] private KeyCode k_leftRotation = KeyCode.Q;
    [SerializeField] private KeyCode k_rightRotation = KeyCode.E;
    [SerializeField] private KeyCode k_confirmPiece = KeyCode.Space;
    [SerializeField] private KeyCode k_returnPiece = KeyCode.Mouse1;

    [Header("Keyboard and Mouse Controls")]
    [SerializeField] private KeyCode j_leftRotation = KeyCode.JoystickButton4; 
    [SerializeField] private KeyCode j_rightRotation = KeyCode.JoystickButton5;
    [SerializeField] private KeyCode j_confirmPiece = KeyCode.JoystickButton1;
    [SerializeField] private KeyCode j_returnPiece = KeyCode.JoystickButton2;

    public enum InputSource
    {
        None,
        Keyboard,
        Joystick
    }
    public InputSource m_CurrentInputSource { get; private set; } = InputSource.None;
    private Vector3 m_LastMousePosition;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.mousePosition != m_LastMousePosition)
        {
            m_CurrentInputSource = InputSource.Keyboard;
        }
        else
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            bool isUsingKeyboardKeys = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);

            if ((Mathf.Abs(horizontal) > 0.2f || Mathf.Abs(vertical) > 0.2f) && !isUsingKeyboardKeys)
            {
                m_CurrentInputSource = InputSource.Joystick;
            }
        }

        m_LastMousePosition = Input.mousePosition;
    }

    public bool GetDown()
    {
        bool inputDetected = false;

        if (Input.GetKey(k_down))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else
        {
            float verticalAxis = Input.GetAxisRaw("Vertical");
            if (verticalAxis < -0.5f)
            {
                m_CurrentInputSource = InputSource.Joystick;
                inputDetected = true;
            }
        }

        return inputDetected;
    }
    public bool GetUp()
    {
        bool inputDetected = false;

        if (Input.GetKey(k_up))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else
        {
            float verticalAxis = Input.GetAxisRaw("Vertical");
            if (verticalAxis > 0.5f)
            {
                m_CurrentInputSource = InputSource.Joystick;
                inputDetected = true;
            }
        }

        return inputDetected;
    }

    public bool GetLeft()
    {
        bool inputDetected = false;

        if (Input.GetKeyDown(k_left))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            if (horizontalAxis < -0.5f)
            {
                m_CurrentInputSource = InputSource.Joystick;
                inputDetected = true;
            }
        }

        return inputDetected;
    }

    public bool GetRight()
    {
        bool inputDetected = false;

        if (Input.GetKey(k_right))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            if (horizontalAxis > 0.5f)
            {
                m_CurrentInputSource = InputSource.Joystick;
                inputDetected = true;
            }
        }

        return inputDetected;
    }

    public bool GetConfirm()
    {
        if (Input.GetKeyDown(k_confirmPiece))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            return true;
        }
        else if (Input.GetKey(j_confirmPiece))
        {
            m_CurrentInputSource = InputSource.Joystick;
            return true;
        }

        return false;
    }

    public bool GetLeftRotation()
    {
        if (Input.GetKeyDown(k_leftRotation))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            return true;
        }
        else if (Input.GetKeyDown(j_leftRotation))
        {
            m_CurrentInputSource = InputSource.Joystick;
            return true;
        }

        return false;
    }
    public bool GetRightRotation()
    {
        if (Input.GetKeyDown(k_rightRotation))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            return true;
        }
        else if (Input.GetKeyDown(j_rightRotation))
        {
            m_CurrentInputSource = InputSource.Joystick;
            return true;
        }

        return false;
    }

    public bool GetReturnPiece()
    {
        if (Input.GetKeyDown(k_returnPiece))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            return true;
        }
        else if (Input.GetKeyDown(j_returnPiece))
        {
            m_CurrentInputSource = InputSource.Joystick;
            return true;
        }

        return false;
    }
    public Vector3 GetWorldMousePosition()
    {
        Vector3 l_Vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return l_Vector;
    }
}
