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
    #endregion

    /**
        Negates or activates endless bool value
        of the specified animator
    */
    public static void boolAnimatorToggler(
        Animator animator, bool isNegated, params int[] anim)
    {
        for (int i = 0; i < anim.Length; i++)
            animator.SetBool(anim[i], isNegated);
    }
}