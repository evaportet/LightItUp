using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCandles : MonoBehaviour
{
    public bool isLit;
    CharacterController charC;
    [SerializeField] GameObject fire;

    private void Start()
    {
        if (isLit)
            fire.SetActive(true);
        else 
            fire.SetActive(false);
    }
    public void LightUp()
    {
        isLit = true;
        fire.SetActive(true);
    }
}
