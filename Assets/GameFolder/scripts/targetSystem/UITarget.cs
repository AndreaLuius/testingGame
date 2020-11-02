using UnityEngine;
using UnityEngine.UI;

namespace TargetSystem
{ 
    public class UITarget : MonoBehaviour
    {
        public Image img;
        public bool isDead;
        private Animator animator;

        private void Start()
        {
            animator = GetComponentInParent<Animator>();
        }

        public void Update()
        {
            if (animator.GetBool("isDead2")) isDead = true;
        }
    }
}
