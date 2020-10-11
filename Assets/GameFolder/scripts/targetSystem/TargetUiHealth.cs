using UnityEngine;

namespace TargetSystem
{ 
    public class TargetUiHealth : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerStay(Collider other)
        {
            var isTargetLocked = animator.GetBool(AnimatorAshesh.isTargetLocked);

            if (other.tag.Equals("TargetEnemy") && isTargetLocked)
                other.transform.GetChild(0).gameObject.SetActive(true);
            else if(other.tag.Equals("TargetEnemy") && !isTargetLocked)
                other.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
