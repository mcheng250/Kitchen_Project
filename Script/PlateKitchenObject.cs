using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectCO kitchenObjectCO;
    }


    [SerializeField] private List<KitchenObjectCO> validKitchenObjectCOList;

    private List<KitchenObjectCO> kitchenObjectCOList;

    private void Awake() {
        kitchenObjectCOList = new List<KitchenObjectCO>();
    }

    public bool TryAddIngredient(KitchenObjectCO kitchenObjectCO) {
        if (!validKitchenObjectCOList.Contains(kitchenObjectCO)) {
            return false;
        }
        if (kitchenObjectCOList.Contains(kitchenObjectCO)) {
            return false;
        } else {
            kitchenObjectCOList.Add(kitchenObjectCO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectCO = kitchenObjectCO
            });
            return true;
        }
    }

    public List<KitchenObjectCO> GetKitchenObjectCOList() {
        return kitchenObjectCOList;
    }
}
