using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyClicker : Graphic {
    protected override void OnPopulateMesh(Mesh m) {
        m.Clear();
    }
}
