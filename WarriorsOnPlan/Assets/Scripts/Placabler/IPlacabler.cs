using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placablers {
    // reidiculous name but i believe you understand what this interface does
    public interface IPlacabler : IParametable {
        public bool checkPlacable(node parNode);
    }
}