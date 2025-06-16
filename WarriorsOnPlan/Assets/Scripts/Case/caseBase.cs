using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cases {

    public enum enumCaseType {
        none    = 0,
        circuit = 0b0001,
        skill   = 0b0010,
        tool    = 0b0100,
        effect  = 0b1000,
        others  = 99
    }

    public abstract class caseBase : codableObject, IInfo {
        public readonly enumCaseType caseType;            

        public bool isVisible { protected set; get; }

        public Sprite caseImage { get; protected set; }
        protected string pathCategory {
            get {
                return "Case/" +
                    caseType switch {
                        enumCaseType.effect => "Effect/",
                        enumCaseType.tool => "Tool/",
                        enumCaseType.skill => "Skill/",
                        _ => ""
                    };
            }
        }

        #region InfoImplementation
        public string infoName { get; protected set; }
        private string infoDescription_ = "E";
        public string infoDescription {
            get {
                if (infoDescription_ == "E") {
                    prepareInfo();
                }
                return string.Format(infoDescription_, getDescriptionArgument());
            }
            protected set {
                infoDescription_ = value;
            }
        }
        public virtual object[] getDescriptionArgument() {
            return new object[0];
        }
        #endregion InfoImplementation

        public caseBase(int[] parArrParameter, enumCaseType parCaseType = enumCaseType.effect, bool parIsVisible = false) : base(parArrParameter) {
            caseType = parCaseType;
            isVisible = parIsVisible;

            if (isVisible) {
                prepareImage();
                prepareInfo();
            }
        }

        protected void prepareImage() {
            caseImage = Resources.Load<Sprite>("Image/" + pathCategory + "image_" + GetType().Name);
        }

        protected void prepareInfo() {
            dataArbitraryStringArray tempASA = gameManager.GM.JC.getJson<dataArbitraryStringArray>(pathCategory + GetType().Name);
            infoName = tempASA.SwissArmyStringArray[0];
            infoDescription_ = tempASA.SwissArmyStringArray[1];
        }
    }
}