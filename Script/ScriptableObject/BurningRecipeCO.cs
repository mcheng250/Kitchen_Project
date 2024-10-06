using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeCO : ScriptableObject
{
    public KitchenObjectCO input;
    public KitchenObjectCO output;
    public float burningTimerMax;
}
