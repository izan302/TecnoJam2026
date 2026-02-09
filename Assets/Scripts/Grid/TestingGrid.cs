using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    Grid m_Grid;
    void Start()
    {
       m_Grid = new Grid(4, 3, 10f, new Vector3(20, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Grid.SetValue(InputManager.Instance.GetWorldMousePosition(), 56);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(m_Grid.GetValue(InputManager.Instance.GetWorldMousePosition()));
        }
    }
}
