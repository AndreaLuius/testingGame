using UnityEngine;
using Cinemachine;

namespace TargetSystem
{ 
    public class SecondTargetSwitcher : MonoBehaviour
    {
        [SerializeField] ControllingTargets controllingTargets;
        [SerializeField] CinemachineTargetGroup targetGroup;

        void Update()
        {
            switching();
        }
     
        private void switching()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (!targetGroup.IsEmpty) targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
                controllingTargets.closestNumber -= (controllingTargets.closestNumber) > 0 ? 1 : 0;

               if (controllingTargets.closestNumber >= 0)
                    targetGroup.AddMember(controllingTargets.EnemiesList[controllingTargets.closestNumber].Transform, 10, 10);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                if (!targetGroup.IsEmpty) targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];

                    controllingTargets.closestNumber += (controllingTargets.closestNumber <= (controllingTargets.EnemiesList.Count - 2)) ? 1 : 0;

                if (controllingTargets.closestNumber <= (controllingTargets.EnemiesList.Count - 1))
                    targetGroup.AddMember(controllingTargets.EnemiesList[controllingTargets.closestNumber].Transform, 10, 10);
            }
        }
    }
}
