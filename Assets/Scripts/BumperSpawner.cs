﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BumperSpawner : MonoBehaviour
{
    public GameObject bumper;
    public float distance;
    public float fadeDuration;
    public float fadeStartValue; // Has to be minimum 0.5f and maximum 8.0f, since this is defined as a range in the Hologramshader itself. The rimpower/color intensity is brightest at 0.5f
    public float fadeEndValue; // Has to be minimum 0.5f and maximum 8.0f, since this is defined as a range in the Hologramshader itself.
    public float fadeSpeed;
    public int sceneIndex; 
    // store new GameObject instance
    private GameObject activeBumper;
    private Scene activeScene;
    
    // Called once per frame
    void Update()
    {
        activeScene = SceneManager.GetActiveScene();
        sceneIndex = activeScene.buildIndex;
        Debug.Log($"Active Scene Index {activeScene.buildIndex}");
        // Check Scene index returns an integer value
        if (activeScene.buildIndex != 0)
        {
            // Activate Controller in Minigames
            ActivateController(true);
        }
        else
        {
            // Deactivate Controller in Workshop
            ActivateController(false);
        }
    }
  
    /// <summary>
    /// On Collision Enter a new bumper object will be 
    /// instantiated and attached to the player.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collission detected");
        Vector3 contact = col.contacts[0].point;
        // Check the scene index
        if (activeScene.buildIndex != 0)
        {
            if (activeBumper == null)
            {
                activeBumper = Instantiate(bumper, new Vector3(contact.x, Camera.main.transform.position.y - 0.7f, contact.z) + 
                               Camera.main.transform.forward * distance, Quaternion.identity);
                Debug.Log("New bumper instance " + activeBumper);
            }
        }
    }

    /// <summary>
    /// OnCollisionExit is calledf when collision is ended
    /// </summary>
    /// <param name="collisionInfo"></param>
    void OnCollisionExit(Collision collisionInfo)
    {
        // Use reflection via Invoke 
        Invoke("DestroyBumpers", 1.2f);
    }

    /// <summary>
    /// Coroutines allow to use procedural animations or sequences of events over time
    /// Otherwise functions need happen only within a single frame.
    /// The function FadeBumper allows to fade the bumper object out over certain time.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>

    IEnumerator FadeBumper()
    {
        // Check if bumper instance is empty
        if(activeBumper != null)
        {
            for(float i = 0; i < fadeDuration; i += Time.deltaTime * fadeSpeed)
            {
                Debug.Log("Coroutine called ");
                float rimPowerShader = Mathf.Lerp(fadeStartValue, fadeEndValue, i / fadeDuration); //Lerping the value of the rimpower between a given start- and endvalue
                activeBumper.GetComponent<Renderer>().material.SetFloat("_RimPower", rimPowerShader); // Set the RimPower of the Bumpers attached shader
                yield return null; // needs to be placed where execution will be paused and resumed on the following frame
            }
            // After while loop has faded bumper out call Destroy(Bumper)
            Destroy(activeBumper);
            Debug.Log("Bumper is destroied");
        }
        else
        {
            Debug.Log("No bumper instance");
        }        
    }

    /// <summary>
    /// Fade Bumper out by calling Coroutine 
    /// and destroying the instance.
    /// </summary>
    void DestroyBumpers()
    {
        StartCoroutine("FadeBumper"); // Can be called by StartCoroutine from everywhere to start the IENumerator 
    }
    
    void ActivateController(bool enable)
    {
        // Set all Colliders inactive on the gameobjects
        GetComponent<BoxCollider>().enabled = enable;
        Debug.Log($"Set Collider {enable}");        
    }
}