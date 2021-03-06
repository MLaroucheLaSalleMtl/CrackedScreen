﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpParticleEffectSpawner : MonoBehaviour
{
    [SerializeField] private Transform placeToSpawn;
    [SerializeField] private GameObject jumpParticleEffect;

    public void SpawnJumpParticleEffect()
    {
        var jumpParticleEffectSpawned = Instantiate(jumpParticleEffect, placeToSpawn.position, Quaternion.identity);
        jumpParticleEffectSpawned.transform.parent = null;
    }
}
