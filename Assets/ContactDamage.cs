using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public Collider collider;
    public string[] targetTag;
    private HashSet<string> targetTagHashSet;

    public string resourceName;
    public float damageValue;
    public bool percentage;
    public bool percentageMax;

    void Start(){
        targetTagHashSet = new HashSet<string>(targetTag);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts){
            var other = contact.otherCollider.gameObject;
            Debug.Log(other);
            if(targetTagHashSet.Contains(other.tag)){
                foreach(Resource resource in other.GetComponents<Resource>()){
                    if(resource.name == resourceName){
                        if(percentageMax){
                            resource.UpdateResourcePercentageMax(damageValue);
                        }else if(percentage){
                            resource.UpdateResourcePercentage(damageValue);
                        }else{
                            resource.UpdateResource(damageValue);
                        }
                    }
                }
            }
        }
    }
}
