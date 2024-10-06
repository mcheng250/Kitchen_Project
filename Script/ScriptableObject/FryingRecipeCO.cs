using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeCO : ScriptableObject
{
    public KitchenObjectCO input;
    public KitchenObjectCO output;
    public float fryingTimerMax;
}
