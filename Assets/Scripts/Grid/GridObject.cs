using UnityEngine;

public class GridObject
    {
        private Grid<GridObject> m_Grid;
        private int m_X;
        private int m_Y;
        private PieceObject m_PieceObject;
        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.m_Grid = grid;
            this.m_X = x;
            this.m_Y = y;
        }
        private void OnGridValueChange(object sender, Grid<GridObject>.OnGridValueChangedEventArgs e)
        {
            Debug.Log("OnGridValueChange");
        }
        public bool CanBuild() 
        {
            return m_PieceObject == null;
        }
        public void SetPieceObject(PieceObject _Transform)
        {
            this.m_PieceObject = _Transform;
            this.m_Grid.TriggerGridObjectChanged(m_X, m_Y);
        }
        public PieceObject GetPieceObject()
        {
           return this.m_PieceObject; 
        }
        public void ClearPiece()
        {
            this.m_PieceObject = null;
        }

        public override string ToString()
        {
            return m_X + ", " + m_Y + "\n" + m_PieceObject;
        }
    }
