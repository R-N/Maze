using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Maze {
    public abstract class InputSource {
        public abstract float GetPreviousAxis(string name);
        public abstract float GetAxis(string name);
        public abstract bool GetButton(string name);
        public abstract bool GetButtonDown(string name);
        public abstract bool GetButtonUp(string name);
        public abstract bool GetButtonUpRaw(string name);
        public abstract bool GetButtonCancel(string name);

        public abstract void SetAxis(string name, float value);
        public abstract void CancelButton(string name);
        public abstract void SetButtonUp(string name);
        public abstract void SetButtonDown(string name);
    }
}