﻿using System.Collections;
using System.Collections.Generic;
using Blaze.Property;
using UnityEngine;

public class TapCountGame : MonoBehaviour
{
    public IntProperty tapCount;

    private void Start() {
        tapCount.Value = tapCount;
    }

    public void Tap(int count)
    {
        tapCount.Value += count;
    }
}
