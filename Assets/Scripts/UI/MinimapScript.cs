using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform player;
    float  storedShadowDistance;
  
    void OnPreRender() {
        storedShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0;
    }
  
    void OnPostRender() {
        QualitySettings.shadowDistance = storedShadowDistance;
    }
    void LateUpdate(){

        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
    
}
