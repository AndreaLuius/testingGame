using UnityEngine;

namespace Art_Intellifence
{
    public interface NormalEnemy
    {
        void patrolling();
        void attack(Collider other);
        void pursuing(Collider other);
        void die();
    }
}