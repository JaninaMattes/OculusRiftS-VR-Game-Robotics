﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EnemyHit : MonoBehaviour
{
    public GameObject particleSystem;

    ScoringSystem scoringSystem;

    public GameObject score10Text;

    public GameObject score25Text;

    public GameObject score50Text;

    public GameObject scorePlusText;

    public GameObject scoreMinusText;

    public Material scoreMatBlau;

    public Material scoreMatRot;

   

    GameObject activeCameraRig;


    public enum Relation
    {
        Friend = 0,
        Enemy = 1,
    }

    public Relation relation;

    public enum ScoreValue
    {
        _10 = 10,
        _25 = 25,
        _50 = 50,
    }

    public ScoreValue scoreValue;



    //public int scoreValue = 10;

    

    private void Start()
    {
        scoringSystem = FindObjectOfType<ScoringSystem>();

        Invoke("GetCameraRig", 0.05f);       
        
    }

    void GetCameraRig()
    {
        activeCameraRig = GameObject.Find("CenterEyeAnchor");
    }


    public void Hit(bool useScoring)
    {
        ParticleSystem ps = Instantiate(particleSystem, gameObject.transform.position, gameObject.transform.rotation).GetComponent<ParticleSystem>();
        var psMain = ps.main;
        var psRenderer = ps.gameObject.GetComponent<ParticleSystemRenderer>();
        if (relation == Relation.Enemy)
        {
            psMain.startColor = new Color(0.5f,0,0,1);
            psRenderer.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");

            if (useScoring)
            {
                scoringSystem.SubtractLocalScore((sbyte)scoreValue);

                GameObject scoreText = null;

                switch (scoreValue)
                {
                    case ScoreValue._10:
                        //Spawn Text 10_Cube
                        scoreText = Instantiate(score10Text, gameObject.transform.position, gameObject.transform.rotation);
                        break;
                    case ScoreValue._25:
                        //Spawn Text 25_Cube
                        scoreText = Instantiate(score25Text, gameObject.transform.position, gameObject.transform.rotation);
                        break;
                    case ScoreValue._50:
                        //Spawn Text 50_Cube
                        scoreText = Instantiate(score50Text, gameObject.transform.position, gameObject.transform.rotation);
                        break;
                }

                GameObject vorzeichen = Instantiate(scoreMinusText, gameObject.transform.position + new Vector3(1.5f, 0, 0), gameObject.transform.rotation);

                vorzeichen.transform.parent = scoreText.transform;

                //Vector3 dir = scoreText.transform.position - activeCameraRig.transform.position;
                scoreText.transform.LookAt(activeCameraRig.transform);

                Transform[] children = scoreText.GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.GetComponent<MeshRenderer>() != null)
                    {
                        child.GetComponent<MeshRenderer>().material = scoreMatRot;
                    }

                }


                //for (int i = 0; i < scoreText.transform.childCount; i++)
                //{
                //    if (scoreText.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                //    {
                //        scoreText.transform.GetChild(i).GetComponent<MeshRenderer>().material = scoreMatRot;
                //    }
                //}
            }
        }
        else
        {
            

            if (useScoring)
            {
                scoringSystem.AddLocalScore((sbyte)scoreValue);
                GameObject scoreText = null;

                switch (scoreValue)
                {
                    case ScoreValue._10:
                        //Spawn Text 10_Cube
                        scoreText = Instantiate(score10Text, gameObject.transform.position, gameObject.transform.rotation);
                        break;
                    case ScoreValue._25:
                        //Spawn Text 25_Cube
                        scoreText = Instantiate(score25Text, gameObject.transform.position, gameObject.transform.rotation);
                        break;
                    case ScoreValue._50:
                        //Spawn Text 50_Cube
                        scoreText = Instantiate(score50Text, gameObject.transform.position, gameObject.transform.rotation);
                        break;
                }

                GameObject vorzeichen = Instantiate(scorePlusText, gameObject.transform.position + new Vector3(1.5f, 0, 0), gameObject.transform.rotation);

                vorzeichen.transform.parent = scoreText.transform;

                scoreText.transform.LookAt(activeCameraRig.transform);
            }
        }

        Destroy(gameObject);
    }

}
