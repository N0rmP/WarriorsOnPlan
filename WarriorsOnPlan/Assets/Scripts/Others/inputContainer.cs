using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class inputContainer {
    public Action delOnAnyKeyDown;
    private Action delOnScrollUp;
    private Action delOnScrollDown;
    private Dictionary<KeyCode, Action> dictKeyAction;

    public inputContainer() {
        delOnAnyKeyDown = null;
        delOnScrollUp = () => { };
        delOnScrollDown = () => { };
        dictKeyAction = new Dictionary<KeyCode, Action>();
    }

    public void updateInput() {
        // Key
        if (Input.anyKeyDown) {
            if (delOnAnyKeyDown != null) {
                delOnAnyKeyDown();
            }

            foreach (KeyCode kc in dictKeyAction.Keys.ToArray()) {
                if (Input.GetKeyDown(kc)) {
                    if (dictKeyAction[kc] is not null) {
                        dictKeyAction[kc]();
                    }
                    break;
                }
            }
        }

        // Scroll
        switch (Input.mouseScrollDelta.y) {
            case > 0:
                if (delOnScrollUp != null) {
                    delOnScrollUp();
                }
                break;
            case < 0:
                if (delOnScrollDown != null) {
                    delOnScrollDown();
                }
                break;
            default:
                break;
        }
    }

    public void clearTotal() {
        delOnAnyKeyDown = null;
        delOnScrollUp = null;
        delOnScrollDown = null;
        dictKeyAction.Clear();
    }

    #region add_remove
    public void addKeyActionPair(KeyCode parKeyCode, Action parAction, bool parIsReplace = false) {
        // ensure the parKeyCode exists in dictKeyAction
        if (!dictKeyAction.ContainsKey(parKeyCode)) {
            dictKeyAction[parKeyCode] = null;
        }

        if (parIsReplace) {
            // replace
            dictKeyAction[parKeyCode] = parAction;
        } else {
            // add delegate
            dictKeyAction[parKeyCode] += parAction;
        }
    }

    // remove total delegate connected to certain keycode
    public void removeKey(KeyCode parKeyCode) {
        if (!dictKeyAction.ContainsKey(parKeyCode)) {
            return;
        }

        dictKeyAction.Remove(parKeyCode);
    }

    public void removeKeyAction(KeyCode parKeyCode, Action parAction) {
        if (!dictKeyAction.ContainsKey(parKeyCode)) {
            return;
        }

        dictKeyAction[parKeyCode] -= parAction;
    }

    public void addAnyKeyAction(Action parAction, bool parIsReplace = false) {
        if (parIsReplace) {
            delOnAnyKeyDown = parAction;
        } else {
            delOnAnyKeyDown += parAction;
        }
    }

    public void addScrollUp(Action parAction, bool parIsReplace = false) {
        if (parIsReplace) {
            delOnScrollUp = parAction;
        } else {
            delOnScrollUp += parAction;
        }
    }

    public void addScrollDown(Action parAction, bool parIsReplace = false) {
        if (parIsReplace) {
            delOnScrollDown = parAction;
        } else {
            delOnScrollDown += parAction;
        }
    }
    #endregion add_remove

    #region test
    public void testInputContainer() {
        StringBuilder tempSB = new StringBuilder("- testInputContainer -\ndelOnAnyKeyDown : ");
        tempSB.Append(delOnAnyKeyDown);
        tempSB.Append("\ndelOnScrollUp : ");
        tempSB.Append(delOnScrollUp);
        tempSB.Append("\ndelOnScrollDown : ");
        tempSB.Append(delOnScrollDown);
        tempSB.Append("\ndictKeyAction : ");
        foreach (KeyCode kc in dictKeyAction.Keys) {
            tempSB.Append(kc);
            tempSB.Append("-");
            tempSB.Append(dictKeyAction[kc]);
            tempSB.Append(" , ");
        }

        Debug.Log(tempSB.ToString());
    }

    public void testCallDelScroll() {
        Debug.Log("- testCallDelScroll -");
        Debug.Log("delOnScrollUp : ");
        delOnScrollUp();
        Debug.Log("delOnScrollDown : ");
        delOnScrollDown();
    }
    #endregion test
}
