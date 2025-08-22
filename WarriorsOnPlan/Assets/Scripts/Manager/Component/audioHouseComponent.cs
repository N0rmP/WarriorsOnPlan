using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioHouseComponent {
    private List<AudioClip> listClipFire;
    private List<AudioClip> listClipHeavyMetal;
    private List<AudioClip> listClipIce;
    private List<AudioClip> listClipMagicBasic;
    private List<AudioClip> listClipPunch;
    private List<AudioClip> listClipSwing;
    private List<AudioClip> listClipSword;
    private List<AudioClip> listClipThunder;
    private List<AudioClip> listClipToolEquip;
    private List<AudioClip> listClipWater;

    #region property
    public AudioClip[] arrClipFire { get { return listClipFire.ToArray(); } }
    public AudioClip[] arrClipHeavyMetal { get { return listClipHeavyMetal.ToArray(); } }
    public AudioClip[] arrClipIce { get { return listClipIce.ToArray(); } }
    public AudioClip[] arrClipMagicBasic { get { return listClipMagicBasic.ToArray(); } }
    public AudioClip[] arrClipPunch { get { return listClipPunch.ToArray(); } }
    public AudioClip[] arrClipSwing { get { return listClipSwing.ToArray(); } }
    public AudioClip[] arrClipSword { get { return listClipSword.ToArray(); } }
    public AudioClip[] arrClipThunder { get { return listClipThunder.ToArray(); } }
    public AudioClip[] arrClipToolEquip { get { return listClipToolEquip.ToArray(); } }
    public AudioClip[] arrClipWater { get { return listClipWater.ToArray(); } }
    #endregion property

    public audioHouseComponent() {        
        listClipFire = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/Fire"));
        listClipHeavyMetal = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/HeavyMetal"));
        listClipIce = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/Ice"));
        listClipMagicBasic = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/MagicBasic"));
        listClipPunch = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/Punch"));
        listClipSwing = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/Swing"));
        listClipSword = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/Sword"));
        listClipThunder = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/Thunder"));
        listClipToolEquip = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/ToolEquip"));
        listClipWater = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/SE/Water"));
    }
}