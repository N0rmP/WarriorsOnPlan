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
    public IReadOnlyList<AudioClip> arrClipFire { get { return listClipFire; } }
    public IReadOnlyList<AudioClip> arrClipHeavyMetal { get { return listClipHeavyMetal; } }
    public IReadOnlyList<AudioClip> arrClipIce { get { return listClipIce; } }
    public IReadOnlyList<AudioClip> arrClipMagicBasic { get { return listClipMagicBasic; } }
    public IReadOnlyList<AudioClip> arrClipPunch { get { return listClipPunch; } }
    public IReadOnlyList<AudioClip> arrClipSwing { get { return listClipSwing; } }
    public IReadOnlyList<AudioClip> arrClipSword { get { return listClipSword; } }
    public IReadOnlyList<AudioClip> arrClipThunder { get { return listClipThunder; } }
    public IReadOnlyList<AudioClip> arrClipToolEquip { get { return listClipToolEquip; } }
    public IReadOnlyList<AudioClip> arrClipWater { get { return listClipWater; } }
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

    public AudioClip selectAttackSound(enumAttackAnimation parEaa) {
        switch (parEaa) {
            case enumAttackAnimation.trigAttackBow:
            case enumAttackAnimation.trigAttackCrossbow:
                return arrClipSwing.selectRandom();
            case enumAttackAnimation.trigAttackBrandish:
            case enumAttackAnimation.trigAttackStab:
                return arrClipSword.selectRandom();
            case enumAttackAnimation.trigAttackCast:
                return arrClipMagicBasic.selectRandom();            
            case enumAttackAnimation.trigAttackPunch:
            default:
                return arrClipPunch.selectRandom();
        }
    }
}