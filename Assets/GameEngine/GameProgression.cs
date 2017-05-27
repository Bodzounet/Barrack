using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class GameProgression : MonoBehaviour
    {
        public event Action<float> OnCoveredAreaIsModified;

        public int Score
        {
            get;
            set;
        }

        private float _coveredArea = 0;
        public float CoveredArea
        {
            get { return _coveredArea / GameManager.Instance.RootCell.Area(); }
            private set
            {
                _coveredArea = value;
                if (OnCoveredAreaIsModified != null)
                    OnCoveredAreaIsModified(_coveredArea / GameManager.Instance.RootCell.Area());
            }
        }

        private List<Cell> newlyFilledCells = new List<Cell>(); 

        public void RegisterNewFilledCell(Cell cell)
        {
            newlyFilledCells.Add(cell);
            StopAllCoroutines();
            StartCoroutine(_CO_ComputeNewCells());
        }

        private IEnumerator _CO_ComputeNewCells()
        {
            yield return new WaitForEndOfFrame();
            float newArea = 0;

            foreach (Cell cell in newlyFilledCells)
            {
                newArea += cell.Area();
            }
            CoveredArea = _coveredArea + newArea;
            newlyFilledCells.Clear();
        }
    }
}