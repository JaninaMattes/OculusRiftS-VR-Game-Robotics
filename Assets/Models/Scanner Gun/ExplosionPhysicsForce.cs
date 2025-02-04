using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Effects
{
    public class ExplosionPhysicsForce : MonoBehaviour
    {
        public float explosionForce = 4;


        private IEnumerator Start()
        {
            if (SceneManager.GetActiveScene().buildIndex != 3)
            {
                // wait one frame because some explosions instantiate debris which should then
                // be pushed by physics force
                yield return null;

                float multiplier = GetComponent<ParticleSystemMultiplier>().multiplier;

                float r = 5 * multiplier;
                var cols = Physics.OverlapSphere(transform.position, r);
                var rigidbodies = new List<Rigidbody>();
                foreach (var col in cols)
                {
                    if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
                    {
                        rigidbodies.Add(col.attachedRigidbody);
                    }
                }
                foreach (var rb in rigidbodies)
                {
                    rb.AddExplosionForce(explosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);
                    if (rb.gameObject.tag == "Zuendbar")
                    {
                        if (rb.gameObject.GetComponent<JuicerRocket>())
                        {
                            rb.gameObject.GetComponent<JuicerRocket>().Ignite();
                        }
                    }
                }
            }
        }
    }
}
