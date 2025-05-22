using UnityEngine;
using UnityEngine.UI;

using Cases;

public class imgRoundRectangle : MonoBehaviour {
    // imgRoundRectangle usually shows caseBase, and is able to save what caseBase this is showing, you can use it for comparing or checking etc.
    public caseBase drawingRoom { get; private set; }

    public void setImg(Sprite parSprite) {
        if (parSprite == null) {
            transform.GetChild(1).gameObject.SetActive(false);
            return;
        }

        transform.GetChild(1).GetComponent<Image>().sprite = parSprite;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void setCase(caseBase parCase) {
        if (parCase == null) {
            return;
        }

        drawingRoom = parCase;
        setImg(parCase.caseImage);
    }
}
