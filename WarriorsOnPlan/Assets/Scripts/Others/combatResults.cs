using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIterateVisibleTextResult {
    public IEnumerable<string> iterateVisibleTextResult();
}

public record combatResult : IIterateVisibleTextResult {
    public bool isPlayerWin;
    public int actionElapsed;
    // both damage below is player side's
    public int totalDamageDealt;
    public int totalDamageTaken;

    public combatResult(bool parIsPlayerWin, int parActionExecuted, int parTotalDamageDealt, int parTotalDamageTaken) {
        isPlayerWin = parIsPlayerWin;
        actionElapsed = parActionExecuted;
        totalDamageDealt = parTotalDamageDealt;
        totalDamageTaken = parTotalDamageTaken;
    }

    public IEnumerable<string> iterateVisibleTextResult() {
        dataBookCombatResult tempDBCR = gameManager.GM.DHouC.bookCombatResult;
        yield return tempDBCR.strActionElapsed + " : " + actionElapsed;
        yield return tempDBCR.strTotalDamageDealt + " : " + totalDamageDealt;
        yield return tempDBCR.strTotalDamageTaken + " : " + totalDamageTaken;
    }
}