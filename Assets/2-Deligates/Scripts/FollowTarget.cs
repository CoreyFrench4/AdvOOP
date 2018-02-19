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
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;

        private NavMeshAgent agent;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            agent.SetDestination(target.position);
        }
    }
}