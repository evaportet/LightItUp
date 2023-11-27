using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static SFXManager;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject respawnCanvas;

    [Header("Melting Settings")]
    [SerializeField] float maxDuration = 10f;
    [SerializeField] float duration;
    [SerializeField] float dropInterval = 5f;
    float nextDrop;
    public bool isAlive { get; private set; } = true;
    public bool isLit = true;
    public float meltModifier = 1f;
    public GameObject waxDrop;
    [SerializeField] GameObject fire;
    GameObject heat;
    float distFromSrc;
    bool inFire;
    GameObject currentWaxTrail;
    float waxBaseHeight = 1.8372e-02f;
    Vector3 prevPos;
    bool waxDropPressed = false;
    [SerializeField] float waxGrowthModifier;

    [Header("PLayer Audio")]
    public AudioClip lightUpClip;
    public AudioClip extinguishClip;


    void Start()
    {
        duration = maxDuration;
        nextDrop = maxDuration - dropInterval;
        Cursor.visible = false;
    }


    void Update()
    {

        #region CANDLE MELT

        if (isLit)
        {
            duration -= Time.deltaTime * meltModifier;
            Vector3 scaleChange = new Vector3(1f, duration / maxDuration, 1f);
            this.gameObject.transform.localScale = scaleChange * 1.75f;
        }
        if (duration <= .0f)
        {
            Die();
        }

        if (heat != null)
        {
            distFromSrc = heat.GetComponent<HeatHazard>().DistFromSource(this.gameObject.transform.position);
            if (distFromSrc < 10f)
            {
                meltModifier = ((4 * 1) / distFromSrc) + 1;
            }
        }
        if (inFire)
        {
            meltModifier = 20f;
        }
        //if(prevPos == this.gameObject.transform.position)
        if (Input.GetButtonDown("WaxDrop"))
            waxDropPressed = true;
        if (Input.GetButtonUp("WaxDrop"))
            waxDropPressed = false;

        if (waxDropPressed && isLit)
        {
            if (currentWaxTrail == null)
            {
                Drop();
            }
            else
            {
                currentWaxTrail.transform.localScale += new Vector3(0f, Time.deltaTime * meltModifier * waxGrowthModifier, 0f);
            }
        }

        #endregion
        prevPos = this.gameObject.transform.position;

    }
    //}

    public void Respawn(Vector3 respawnPos)
    {
        Cursor.visible = false;
        duration = maxDuration;
        isAlive = true;
        isLit = true;
        respawnCanvas.SetActive(false);
        this.GetComponent<ThirdPersonController>().Teleport(respawnPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RespawnPoint"))
        {
            RespawnManager.GetInstance().SetNewRespawnPoint(other);
        }
        else if (other.gameObject.CompareTag("AirHazard"))
        {
            Extinguish();
        }
        else if (other.gameObject.CompareTag("NPCandle"))
        {
            if (!isLit && other.gameObject.GetComponent<NPCandles>().isLit)
            {
                LightUp();
            }
            else if (isLit && !other.gameObject.GetComponent<NPCandles>().isLit)
            {
                other.gameObject.GetComponent<NPCandles>().LightUp();
            }
        }
        else if (other.gameObject.CompareTag("Fire"))
            Die();
        else if (other.gameObject.CompareTag("Heat"))
        {
            heat = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Heat"))
        {
            heat = null;
            meltModifier = 1f;
        }
        else if (other.gameObject.CompareTag("waxTrail"))
        {
            currentWaxTrail = null;
        }
    }

    void Drop()
    {
        waxDrop.transform.localScale = new Vector3(.3f, (Time.deltaTime * meltModifier) / maxDuration, .3f);
        currentWaxTrail = Instantiate(waxDrop, this.gameObject.transform.position, waxDrop.transform.rotation);
    }

    public void LightUp()
    {
        isLit = true;
        fire.SetActive(true);
        SFXManager.GetInstance().PlayAudioClip(this.transform, lightUpClip);
    }

    private void Extinguish()
    {
        isLit = false;
        fire.SetActive(false);
        SFXManager.GetInstance().PlayAudioClip(this.transform, extinguishClip);
    }

    private void Die()
    {
        Cursor.visible = true;
        isAlive = false;
        isLit = false;
        respawnCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
