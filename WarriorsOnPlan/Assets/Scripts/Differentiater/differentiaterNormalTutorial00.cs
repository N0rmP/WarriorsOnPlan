using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Differentiaters {
    public class differentiaterNormalTutorial00 : differentiaterBase {
        private GameObject canvasActionOrderTemp;

        public differentiaterNormalTutorial00() {
            /*
            normal first tutorial list
                1. player's warrior walks in
                2. player should clicks it because there's nothing except it
                3. canvasInformation & canvasToolstorage comes in
                4. player should do something with the only provided tool, same reason as 2
                5. canvasOthers comes in and enemy warrior walks in
                6. start combat
            */
            GameObject tempObj;
            arrDelCheckSequential = new Func<bool>[] {
                () => { return combatManager.CM.CUM.CStatus.thisThing != null; },
                () => { return combatManager.CM.HouC.getArrAlive(enumSide.player)[0].getCaseCount(Cases.enumCaseType.tool) > 0; }
            };
            arrDelDoSequential = new Action[] {
                () => {
                    tempObj = GameObject.Find("canvasInformation");
                    tempObj.AddComponent<uiActivatable>().isOutClickDeactivate = false;
                    tempObj.GetComponent<uiActivatable>().activatePanel(
                        new Vector3((gameManager.GM.canvasMain.GetComponent<RectTransform>().sizeDelta.x - tempObj.GetComponent<RectTransform>().sizeDelta.x) / 2f, 0f, 0f)
                    );

                    tempObj = GameObject.Find("canvasToolStorage");
                    tempObj.AddComponent<uiActivatable>().isOutClickDeactivate = false;
                    tempObj.GetComponent<uiActivatable>().activatePanel(
                        new Vector3(tempObj.GetComponent<RectTransform>().localPosition.x, -150f, 0f)
                    );

                    // set cameras' viewport
                    Camera tempCameraCombat = GameObject.Find("CAMERA_Combat").GetComponent<Camera>();
                    Camera tempCameraUI = GameObject.Find("CAMERA_UI").GetComponent<Camera>();
                    Vector2 tempCoorVelocity = (new Vector2(0f, 0.11f) - tempCameraCombat.rect.position) * 10f;
                    Vector2 tempSizeVelocity = (new Vector2(0.7f, 1f) - tempCameraCombat.rect.size) * 10f;
                    Rect tempSafetyRect = tempCameraCombat.rect;

                    IEnumerator tempCameraRectChange(){
                        while(tempSafetyRect.y < 0.11f || tempSafetyRect.width > 0.7f){
                            tempSafetyRect.position += tempCoorVelocity * Time.deltaTime;
                            tempSafetyRect.size += tempSizeVelocity * Time.deltaTime;
                            tempSafetyRect.y = Mathf.Min(tempSafetyRect.y, 0.11f);
                            tempSafetyRect.width = Mathf.Max(tempSafetyRect.width, 0.7f);

                            tempCameraCombat.rect = tempSafetyRect;
                            tempCameraUI.rect = tempSafetyRect;
                            yield return new WaitForSeconds(0.01f);
                        }
                    }

                    gameManager.GM.TC.addDelegate(
                        () => combatManager.CM.DC.StartCoroutine(tempCameraRectChange()),
                        0.5f
                    );
                },
                () => {
                    Thing tempThing = combatManager.CM.HouC.getArrAlive(enumSide.enemy)[0];
                    tempThing.Look(tempThing.transform.position + new Vector3(0f, 0f, -1f));
                    tempThing.thisOrganAnimation.animateMove();
                    tempThing.moveLinear(tempThing.curPosition.getVector3());

                    gameManager.GM.IC.dismissTemporayInputContinaer();

                    tempObj = GameObject.Find("canvasOthers");
                    tempObj.AddComponent<uiActivatable>().isOutClickDeactivate = false;
                    tempObj.GetComponent<uiActivatable>().activatePanel(
                        new Vector3(tempObj.GetComponent<RectTransform>().localPosition.x, 75f, 0f)
                    );
                }
            };
        }

        protected override void actualInit() {
            GameObject.Find("canvasOthers").GetComponent<RectTransform>().localPosition += new Vector3(0f, gameManager.GM.canvasMain.GetComponent<RectTransform>().sizeDelta.y, 0f);
            GameObject.Find("canvasOthers").GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 150f);
            GameObject.Find("canvasToolStorage").GetComponent<RectTransform>().localPosition += new Vector3(0f, -gameManager.GM.canvasMain.GetComponent<RectTransform>().sizeDelta.y, 0f);
            GameObject.Find("canvasInformation").GetComponent<RectTransform>().localPosition += new Vector3(gameManager.GM.canvasMain.GetComponent<RectTransform>().sizeDelta.x, 0f, 0f);
            GameObject.Find("buttonSetCircuit").SetActive(false);
            canvasActionOrderTemp = GameObject.Find("canvasActionOrder");
            canvasActionOrderTemp.SetActive(false);
            gameManager.GM.IC.inaugurateTemporayInputContinaer(new inputContainer());

            // player's warrior is placed outside the screen and walks in
            Thing tempThing = combatManager.CM.HouC.getArrAlive(enumSide.player)[0];
            tempThing.setPosition(new Vector3(tempThing.transform.position.x, 0f, -4f));
            tempThing.Look(tempThing.transform.position + new Vector3(0f, 0f, 1f));
            tempThing.thisOrganAnimation.animateMove();
            tempThing.moveLinear(tempThing.curPosition.getVector3());

            // player's warrior walks in
            tempThing = combatManager.CM.HouC.getArrAlive(enumSide.enemy)[0];
            tempThing.setPosition(new Vector3(tempThing.transform.position.x, 0f, 16f));

            // change camera viewport
            GameObject.Find("CAMERA_Combat").GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
            GameObject.Find("CAMERA_UI").GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
        }

        public override void restoreWhenLeave() {
            base.restoreWhenLeave();

            // restore canvases
            GameObject tempObj;
            uiActivatable tempUA;
            tempObj = GameObject.Find("canvasInformation");
            tempObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            if (tempObj.TryGetComponent<uiActivatable>(out tempUA)) {
                GameObject.Destroy(tempUA);
            }
            tempObj = GameObject.Find("canvasToolStorage");
            tempObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            if (tempObj.TryGetComponent<uiActivatable>(out tempUA)) {
                GameObject.Destroy(tempUA);
            }
            tempObj = GameObject.Find("canvasOthers");
            tempObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            tempObj.GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 150f);
            if (tempObj.TryGetComponent<uiActivatable>(out tempUA)) {
                GameObject.Destroy(tempUA);
            }
            canvasActionOrderTemp.SetActive(true);

            // restore Cameras
            GameObject.Find("CAMERA_Combat").GetComponent<Camera>().rect = new Rect(0f, 0.22f, 0.7f, 1f);
            GameObject.Find("CAMERA_UI").GetComponent<Camera>().rect = new Rect(0f, 0.22f, 0.7f, 1f);


        }
    }
}