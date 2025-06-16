using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInfo {
    public string infoName { get; }
    public string infoDescription { get; }
    public object[] getDescriptionArgument();
}
