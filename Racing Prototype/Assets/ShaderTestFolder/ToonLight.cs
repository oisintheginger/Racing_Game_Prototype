using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonLight : MonoBehaviour
{
    private Light light = null;

    public GameObject Object;

    private void OnEnable()
    {
        light = this.GetComponent<Light>();
    }

    private void Update()
    {
        Shader.SetGlobalVector("_ToonLightDirection", -this.transform.forward);     //sets a vector between any object with the cartoon unlit shader and uses that to calculate the shadows
    }
}
