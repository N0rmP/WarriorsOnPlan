using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Processes {
    public class processSystemCombatEnd : processAbst {
        public processSystemCombatEnd(bool parIsSHOW = true) : base(parIsSHOW) { }

        protected override void actualDO() {
            
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            TextMeshProUGUI tempTMPro = GameObject.Find("TEMP_TEXT_RESULT").GetComponent<TextMeshProUGUI>();
            tempTMPro.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
            tempTMPro.text = (combatManager.CM.checkIsPlayerWin() ? "Player Win" : "Player Defeated");

        }
    }
}