using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParametable {
    // most parameter-methods are used for initiation and memento patter, but getParameters is also used for description showing
    // so ["concrete"] index of return value of getParameters should have all values programmer wants to show to player, even if some values are already in another index
    public Dictionary<string, int[]> getParameters();

    public List<object> getReference();

    // restore may be implemented in the first-root parent class with other methods here, and child classes may use only the implemented method
    public void restore(mementoIParametable parmementoIParametable);

    public void restoreParameters(Dictionary<string, int[]> parParameters);

    // overloaded restoreParameters is used for initiation when the object is created, because json file uses array instead of dictionary
    public void restoreParameters(IEnumerator<int> parParameters);    
}
