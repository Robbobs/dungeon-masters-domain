using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResourceProgressBar : MonoBehaviour
{
    public Image progress;
    public float maxResource = 100f;
    public float currentResource;
    public float regenResource = 5f;

    // Start is called before the first frame update
    void Start()
    {
        currentResource = maxResource;
    }

    public float UpdateResource(float value)
    {
        currentResource += value;
        currentResource = System.Math.Min(currentResource, maxResource);
        currentResource = System.Math.Max(currentResource, 0);
        return currentResource;
    }

    public float UpdateResourcePercentage(float value)
    {
        return UpdateResource(currentResource * value);
    }

    public float UpdateResourcePercentageMax(float value)
    {
        return UpdateResource(maxResource * value);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")){
            currentResource += -10;
        }
        currentResource = UpdateResource(regenResource * Time.deltaTime);
        progress.fillAmount = currentResource / maxResource;
    }
}