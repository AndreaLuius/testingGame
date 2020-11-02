using UnityEngine;

public abstract class AnimatorAshesh : MonoBehaviour
{
    #region HashedProperties
    public static int armingInProcess = Animator.StringToHash("armingInProcess");
    public static int isAttacking = Animator.StringToHash("isAttacking");
    public static int isInAir = Animator.StringToHash("isInAir");
    public static int isTargetLocked = Animator.StringToHash("isTargetLocked");
    public static int arming = Animator.StringToHash("arming");
    public static int isJumping = Animator.StringToHash("isJumping");
    public static int canAttack = Animator.StringToHash("canAttack");
    public static int horizontal = Animator.StringToHash("horizontal");
    public static int vertical = Animator.StringToHash("vertical");
    public static int attack = Animator.StringToHash("attack");
    public static int attackType = Animator.StringToHash("attackType");
    public static int enemySpeed = Animator.StringToHash("speed");
    public static int alertMode = Animator.StringToHash("isAlerted");
    public static int isStrafingLeft = Animator.StringToHash("isStrafingLeft");
    public static int isStrafingRight = Animator.StringToHash("isStrafingRight");
    public static int staminaSucker = Animator.StringToHash("staminaSucker");
    public static int rollType = Animator.StringToHash("rollType");
    public static int isRolling = Animator.StringToHash("isRolling");
    public static int isRollEnabled = Animator.StringToHash("isRollEnabled");
    public static int isCurrentlyRolling = Animator.StringToHash("isCurrentlyRolling");
    public static int isTurningRight = Animator.StringToHash("isTurningRight");
    public static int isTurningLeft = Animator.StringToHash("isTurningLeft");
    public static int isCameraFreed = Animator.StringToHash("isCameraFreed");
    public static int turnRight = Animator.StringToHash("turnRight");
    public static int turnLeft = Animator.StringToHash("turnLeft");
    public static int isTurning = Animator.StringToHash("isTurning");
    public static int isInProcess = Animator.StringToHash("isInProcess");
    public static int jumpTowards = Animator.StringToHash("jumpTowards");
    public static int dodgeBack = Animator.StringToHash("dodgeBack");
    public static int controlTimerHandler = Animator.StringToHash("controlTimeHandler");
    public static int isAttackMovPossible = Animator.StringToHash("isAttackMovPossible");
    public static int attMovAllower = Animator.StringToHash("attMovAllower");
    public static int hitRight = Animator.StringToHash("hitRight");
    public static int bigTurnRight = Animator.StringToHash("bigTurnRight");
    public static int bigTurnLeft = Animator.StringToHash("bigTurnLeft");
    public static int hitLeft = Animator.StringToHash("hitLeft");
    public static int hitBack = Animator.StringToHash("hitBack");
    public static int hitCenter = Animator.StringToHash("hitCenter");


    #endregion

    /*
        Negates or activates endless bool value
        of the specified animator
    */
    public static void boolAnimatorToggler(
        Animator animator, bool isNegated, params int[] anim)
    {
        for (int i = 0; i < anim.Length; i++)
            animator.SetBool(anim[i], isNegated);
    }

    /*
        Toggles the the given animation start/stop
    */
    public static void animSwitcher(Animator animator, int animToStop, int animToStart)
    {
        animator.SetBool(animToStop, false);
        animator.SetBool(animToStart, true);
    }
}