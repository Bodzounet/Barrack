using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public abstract class Ball : MonoBehaviour
    {
        private Cell _parentCell;
        public virtual Cell ParentCell
        {
            get { return _parentCell; }
            set
            {
                _parentCell = value;
            }
        }
    }
}