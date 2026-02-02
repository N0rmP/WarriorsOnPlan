using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class organAnimation {
    private HashSet<enumAttackAnimation> setAttackTriggerName;
    private Animator thisAnimator;

    public organAnimation(Animator parAnimator) {
        setAttackTriggerName = new HashSet<enumAttackAnimation>();
        thisAnimator = parAnimator;
    }

    private void doBeforeAnimate() {
        setAnimationSpeed();
        thisAnimator.ResetTrigger("trigDamaged");
        thisAnimator.SetBool("isControlled", false);
        thisAnimator.SetBool("isFocussing", false);
    }

    private void setAnimationSpeed() {
        thisAnimator.SetFloat("multiplierTotal", combatManager.CM.combatSpeed);
        thisAnimator.SetFloat("multiplierAttack", Math.Max(1, combatManager.CM.combatSpeed * setAttackTriggerName.Count));
    }

    // reset all parameters, and play the idle animation state
    public void resetAnimator() {
        foreach (AnimatorControllerParameter ACP in thisAnimator.parameters) {
            switch (ACP.type) {
                case AnimatorControllerParameterType.Int:
                    thisAnimator.SetInteger(ACP.name, 0);
                    break;
                case AnimatorControllerParameterType.Float:
                    thisAnimator.SetFloat(ACP.name, (ACP.name.Substring(0, 10) == "multiplier") ? 1f : 0f);
                    break;
                case AnimatorControllerParameterType.Bool:
                    thisAnimator.SetBool(ACP.name, false);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    thisAnimator.ResetTrigger(ACP.name);
                    break;
            }
        }
        thisAnimator.Play("Idle", 0);
    }

    #region animate
    public void animateMove() {
        doBeforeAnimate();
        thisAnimator.SetBool("isRun", true);
    }

    public void animateAttack(bool parIsProjectile = true) {
        doBeforeAnimate();
        foreach (enumAttackAnimation enumAA in setAttackTriggerName) {
            thisAnimator.SetTrigger(enumAA.ToString());
        }
        thisAnimator.SetTrigger("trigAttackStart");
    }

    public void animateUseSkill() {
        doBeforeAnimate();
        thisAnimator.SetTrigger("trigUseSkill");
    }

    public void animateDamaged() {
        // damaged animation has lowest priority, skip damaged animation when warrior is not in idle animation
        if (!thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            return;
        }

        doBeforeAnimate();
        thisAnimator.SetTrigger("trigDamaged");
    }

    public void animateDead() {
        doBeforeAnimate();
        thisAnimator.SetTrigger("trigDead");
    }

    public void animateFocuss() {
        doBeforeAnimate();
        thisAnimator.SetBool("isFocussing", true);
    }

    public void animateControlled() {
        doBeforeAnimate();
        thisAnimator.SetBool("isControlled", true);
    }
    #endregion animate    

    #region attack_animation
    public void clearAttackAnimation() {
        setAttackTriggerName.Clear();
    }

    public void addAttackAnimation(enumAttackAnimation parEnumAttackAnimation) {
        setAttackTriggerName.Add(parEnumAttackAnimation);
    }

    public void addAttackAnimation(IEnumerable parEnumAttackAnimation) {
        foreach (enumAttackAnimation eaa in parEnumAttackAnimation) {
            addAttackAnimation(eaa);
        }
    }

    public int getAttackAnimationCount() {
        return setAttackTriggerName.Count;
    }
    #endregion attack_animation
}
