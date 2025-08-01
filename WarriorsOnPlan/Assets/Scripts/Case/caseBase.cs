using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cases {

    #region enum
    public enum enumCaseType {
        none        = 0,
        circuit     = 0b00001,
        skill       = 0b00010,
        tool        = 0b00100,
        effect      = 0b01000,
        upgrade     = 0b10000,
        others      = -99
    }

    public enum enumCaseTag { 
        deal,
        heal,
        move,
        targetFriendly,
        targetHostile
    }
    #endregion enum

    public abstract class caseBase : codableObject, IInfo {
        public readonly enumCaseType caseType;            

        public bool isVisible { get; protected set;  }

        public Sprite caseImage { get; protected set; }
        protected string pathCategory {
            get {
                return "Case/" +
                    caseType switch {
                        enumCaseType.effect => "Effect/",
                        enumCaseType.tool => "Tool/",
                        enumCaseType.skill => "Skill/",
                        enumCaseType.upgrade => "Upgrade/",
                        _ => ""
                    };
            }
        }

        #region InfoImplementation
        public string infoName { get; protected set; }
        private string infoDescription_;
        public string infoDescription {
            get {
                return string.Format(infoDescription_, getDescriptionArgument());
            }
        }
        public virtual object[] getDescriptionArgument() {
            return new object[0];
        }
        #endregion InfoImplementation

        public caseBase(string parImagePath = "Image/Case/Tool/Image_weaponBareFist", enumCaseType parCaseType = enumCaseType.effect, bool parIsVisible = false) {
            caseType = parCaseType;
            isVisible = parIsVisible;

            if (isVisible) {
                prepareImage(parImagePath);
                prepareInfo();
            }
        }

        protected void prepareImage(string parPath) {
            caseImage = Resources.Load<Sprite>(parPath);
        }

        protected void prepareInfo() {
            dataArbitraryStringArray tempASA = gameManager.GM.FC.importResourcesJson<dataArbitraryStringArray>(pathCategory + GetType().Name);
            infoName = tempASA.SwissArmyStringArray[0];
            infoDescription_ = tempASA.SwissArmyStringArray[1];
        }
    }
}