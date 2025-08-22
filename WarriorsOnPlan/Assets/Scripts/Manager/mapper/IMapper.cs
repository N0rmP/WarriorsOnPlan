using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapper {
    public void prepareMap();
    public void doWhenCombatVictory();
    public void doWhenCombatDefeated();
    // load might be included in prepareMap
    public void save();
}
