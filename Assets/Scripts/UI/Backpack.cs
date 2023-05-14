using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Backpack : MonoBehaviour
{
    public Image backpack;
    // Start is called before the first frame update
    void Start()
    {
        backpack.enabled = false;
    }

    // Update is called once per frame
    void Update()   
    {
        if (Input.GetKeyDown(KeyCode.B)){
            backpack.enabled = !backpack.enabled;
        }
    }
}
