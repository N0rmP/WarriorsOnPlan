using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.TextCore.Text;
using TMPro;
using UnityEngine.Localization.Settings;

public enum enumFontTableKey {
    MainFont,
    BoldFont,
    LightFont
}

public class localizerFont : MonoBehaviour {
    private static Dictionary<enumFontTableKey, TMP_FontAsset> dictFont = null;
    public enumFontTableKey thisFontTableKey = enumFontTableKey.MainFont;

    public void Start() {
        if (dictFont == null) {
            dictFont = new Dictionary<enumFontTableKey, TMP_FontAsset>();
            LocalizedAsset<TMP_FontAsset> tempLA = new LocalizedAsset<TMP_FontAsset>();
            tempLA.TableReference = "Font Table";
            for (int i = 0; i < 3; i++) {                
                tempLA.TableEntryReference = ((enumFontTableKey)i).ToString();
                dictFont.Add((enumFontTableKey)i, tempLA.LoadAsset());
            }
        }

        GetComponent<TextMeshProUGUI>().font = dictFont[thisFontTableKey];
    }
}
