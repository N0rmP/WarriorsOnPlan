using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxMovable : movableObject, IMovableSupplement {
    public Action<GameObject> delReturn { private get; set; }

    public void whenStartMove() { }

    public void whenEndMove() {
        delReturn(gameObject);
    }

    
}
