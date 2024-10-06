using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeCO : ScriptableObject
{
    public KitchenObjectCO input;
    public KitchenObjectCO output;
    public int cuttingProgressMax;
}
