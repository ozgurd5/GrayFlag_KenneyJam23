using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NiggerEasterEggScript : MonoBehaviour
{
    AudioSource audioSource;
    public Canvas canvas;

    public Image nigger;

    float niggerAlpha = 1f;
    public float niggerFadeTime = 0.3f;

    bool nPressed;
    bool iPressed;
    bool gPressed;
    bool ePressed;
    bool rPressed;

    bool alreadyPlayedSound = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Niggers()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            nPressed = true;
            Debug.Log("N");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            iPressed = true;
            Debug.Log("I");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gPressed = true;
            Debug.Log("G");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ePressed = true;
            Debug.Log("E");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            rPressed = true;
            Debug.Log("R");
        }
        if (nPressed && iPressed && gPressed && ePressed && rPressed)
        {
            if(!alreadyPlayedSound)
            audioSource.PlayOneShot(audioSource.clip);
            alreadyPlayedSound = true;
            canvas.gameObject.SetActive(true);
            
            niggerAlpha -= niggerFadeTime * Time.deltaTime;
            nigger.color = new Color(nigger.color.r, nigger.color.g, nigger.color.b, niggerAlpha);
            if(niggerAlpha <= 0.01f) canvas.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        Niggers();
    }

}

