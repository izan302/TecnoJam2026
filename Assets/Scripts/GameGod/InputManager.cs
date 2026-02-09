using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;    

    [Header("Keyboard and Mouse Controls")]
    [SerializeField] private KeyCode m_JumpKey = KeyCode.Space;
    [SerializeField] private KeyCode m_AttackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode m_MoveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode m_MoveRightKey = KeyCode.D;
    [SerializeField] private KeyCode m_PauseKey = KeyCode.Escape;
    [SerializeField] private KeyCode m_UpKey = KeyCode.W;
    [SerializeField] private KeyCode m_DownKey = KeyCode.S;
    [SerializeField] private KeyCode m_RunKey = KeyCode.LeftShift;
    //private KeyCode m_InteractKey = KeyCode.E;
    
    [Header("Keyboard and Mouse Controls")]
    [SerializeField] private KeyCode m_JoystickJumpKey = KeyCode.JoystickButton1;
    [SerializeField] private KeyCode m_JoystickAttackKey = KeyCode.JoystickButton5;
    [SerializeField] private KeyCode m_JoystickPauseKey = KeyCode.JoystickButton9;
    [SerializeField] private KeyCode m_JoystickRunKey = KeyCode.JoystickButton11;

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


    public bool GetJump()
    {
        if (Input.GetKey(m_JumpKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            return true;
        }
        else if (Input.GetKey(m_JoystickJumpKey))
        {
            m_CurrentInputSource = InputSource.Joystick;
            return true;
        }

        return false;
    }
    public bool GetJumpDown()
    {
        bool inputDetected = false;
        if (Input.GetKeyDown(m_JumpKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else if (Input.GetKeyDown(m_JoystickJumpKey))
        {
            m_CurrentInputSource = InputSource.Joystick;
            inputDetected = true;
        }
        return inputDetected;
    }

    public bool GetJumpUp()
    {
        bool inputDetected = false;
        if (Input.GetKeyUp(m_JumpKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else if (Input.GetKeyUp(m_JoystickJumpKey))
        {
            m_CurrentInputSource = InputSource.Joystick;
            inputDetected = true;
        }
        return inputDetected;
    }

    public bool GetAttack()
    {
        bool inputDetected = false;
        if (Input.GetKeyDown(m_AttackKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else if (Input.GetKeyDown(m_JoystickAttackKey))
        {
            m_CurrentInputSource = InputSource.Joystick;
            inputDetected = true;
        }
        return inputDetected;
    }

    public bool GetMoveLeft()
    {
        bool inputDetected = false;

        if (Input.GetKey(m_MoveLeftKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else
        {
            float horizontalAxis = GetHorizontalAxis();
            if (horizontalAxis < -0.1f)
            {
                m_CurrentInputSource = InputSource.Joystick;
                inputDetected = true;
            }
        }

        return inputDetected;
    }

    public bool GetMoveRight()
    {
        bool inputDetected = false;

        if (Input.GetKey(m_MoveRightKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else
        {
            float horizontalAxis = GetHorizontalAxis();
            if (horizontalAxis > 0.1f)
            {
                m_CurrentInputSource = InputSource.Joystick;
                inputDetected = true;
            }
        }

        return inputDetected;
    }

    public bool GetPause()
    {
        bool inputDetected = false;
        if (Input.GetKeyDown(m_PauseKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else if (Input.GetKeyDown(m_JoystickPauseKey))
        {
            m_CurrentInputSource = InputSource.Joystick;
            inputDetected = true;
        }
        return inputDetected;
    }
    public bool GetUp()
    {
        bool inputDetected = false;

        if (Input.GetKey(m_UpKey))
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

    public bool GetDown()
    {
        bool inputDetected = false;

        if (Input.GetKey(m_DownKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else
        {
            float verticalAxis = Input.GetAxisRaw("Vertical");
            if (verticalAxis < 0.5f)
            {
                m_CurrentInputSource = InputSource.Joystick;
                inputDetected = true;
            }
        }

        return inputDetected;
    }
    public float GetHorizontalAxis()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public bool GetRun()
    {
        bool inputDetected = false;
        if (Input.GetKey(m_RunKey))
        {
            m_CurrentInputSource = InputSource.Keyboard;
            inputDetected = true;
        }
        else if (Input.GetKey(m_JoystickRunKey))
        {
            m_CurrentInputSource = InputSource.Joystick;
            inputDetected = true;
        }
        return inputDetected;
    }

}
