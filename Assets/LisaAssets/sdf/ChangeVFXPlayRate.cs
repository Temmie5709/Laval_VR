using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChangeVFXPlayRate : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;
    [SerializeField] float playRate = 1f;
    // Start is called before the first frame update
    void Start()
    {
        vfx.playRate = playRate;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
