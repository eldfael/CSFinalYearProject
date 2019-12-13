﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyBehaviour
{
    void OnStart();
    void OnFixed();
    void OnDeath();

    void OnContact();

}