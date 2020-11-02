using UnityEngine;

public interface EnemyAngleHandlerUtility 
{
    void enemyTurnAngle(Collider other);
    bool angleTurnControl(Collider other, float turnSpeed);
}
