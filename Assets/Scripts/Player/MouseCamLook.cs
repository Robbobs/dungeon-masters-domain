using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamLook : MonoBehaviour
{
    public GameObject playerHead;

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    
    const float minimumX = -360F;
    const float maximumX = 360F;
    const float minimumY = -60F;
    const float maximumY = 60F;
    
    float rotationX = 0F;
    float rotationY = 0F;
    
    Quaternion originalBodyRotation;
    Quaternion originalHeadRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalBodyRotation = transform.localRotation;
        originalHeadRotation = playerHead.transform.localRotation;
    }

    void Update()
    {
        // Body
         rotationX += Input.GetAxis("Mouse X") * sensitivityX;
         Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);

         transform.localRotation = originalBodyRotation * xQuaternion;


        // Head
         rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
         rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

         Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

         playerHead.transform.localRotation = originalHeadRotation * yQuaternion;
    }
}