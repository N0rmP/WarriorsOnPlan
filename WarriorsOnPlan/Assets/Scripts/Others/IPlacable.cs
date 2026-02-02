using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacable {
    public node curPosition { get; set; }

    public void setPosition(Vector3 parVector);
}

public interface IPlacableOccupier : IPlacable { }

public interface IPlacableSharer : IPlacable { }
