using UnityEngine;

public class EnemyPosition
{
    private Transform transform;
    private float enemyAngle;

    public EnemyPosition(Transform transform,float enemyAngle)
    {
        this.transform = transform;
        this.enemyAngle = enemyAngle;
    }

    public Transform Transform { get { return transform; } }
    public float EnemyAngle { get { return enemyAngle; } set { enemyAngle = value; } }
}
