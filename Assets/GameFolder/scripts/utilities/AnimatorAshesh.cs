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
    #endregion
}