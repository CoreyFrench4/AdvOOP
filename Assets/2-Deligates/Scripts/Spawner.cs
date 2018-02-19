/*=============================================
-----------------------------------
Copyright (c) 2018 Corey R D French
-----------------------------------
@file: Spawner.cs
@date: 19/02/2018
@author: Corey R D French
@brief: Script for spawning things with delegates
===============================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Delegates
{
    /*
     * 
     * 
     * 
     */
    public class Spawner : MonoBehaviour
    {
        public Transform target;
        public GameObject trollPrefab, orkPrefab;
        public float spawnAmount = 10; // Spawn amount for each prefab
        public float spawnRate = 0.5f;

        private float spawnTimer = 0f;

        public delegate void SpawnDelegate();
        public SpawnDelegate spawnCallback;

        void Start()
        {
            // Subscribe all functions to delegate
            spawnCallback += SpawnOrk;
            spawnCallback += SpawnTroll;
        }
        // Update is called once per frame
        void Update()
        {
            // Count up the timer
            spawnTimer += Time.deltaTime;
            // Has timer reached spawn rate?
            if (spawnTimer >= spawnRate)
            {
                // Loop through and spawn orcs and trolls
                for (int i = 0; i < spawnAmount; i++)
                {
                    spawnCallback.Invoke();
                
                }
                // Reset spawn timer
                spawnTimer = 0f;
            }
        }

         void SpawnOrk()
        {
            GameObject clone = Instantiate(orkPrefab,
                transform.position, transform.rotation);

            // Do orc stuff

            NavMeshAgent agent = clone.GetComponent<NavMeshAgent>();
            agent.SetDestination(target.position);
        }
        void SpawnTroll()
        {
            GameObject clone = Instantiate(trollPrefab,
                transform.position, transform.rotation);

            // Do troll stuff

            NavMeshAgent agent = clone.GetComponent<NavMeshAgent>();
            agent.SetDestination(target.position);
        }
    }
}