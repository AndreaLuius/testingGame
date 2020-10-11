using System.Collections;
using UnityEngine;

public interface EnemyTimeProcessUtility 
{
    IEnumerator alertController(float alertTime);
    IEnumerator attackTimingController(float waitTime, Collider other);
    IEnumerator waitStartAlert(float waitTime);
}



