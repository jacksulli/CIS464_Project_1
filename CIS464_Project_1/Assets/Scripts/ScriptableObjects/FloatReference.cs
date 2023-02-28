using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatReference
{
    public bool useConstant;
    public float constantValue;
    public FloatVariable variable;

    public float Value
    {
        get { return useConstant ? constantValue : variable.value; }
    }
}
