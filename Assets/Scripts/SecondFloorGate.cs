﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloorGate : EnemySpawnerComponent
{
    public override void OnEnemyDie()
    {
        Destroy(gameObject);
    }
}
