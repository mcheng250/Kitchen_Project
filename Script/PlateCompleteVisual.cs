using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{

    [Serializable]
    public struct KitchenObjectCO_GameObject {

        public KitchenObjectCO kitchenObjectCO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectCO_GameObject> kitchenObjectCOGameObjectList;

    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectCO_GameObject kitchenObjectCOGameObject in kitchenObjectCOGameObjectList) {
                kitchenObjectCOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        foreach (KitchenObjectCO_GameObject kitchenObjectCOGameObject in kitchenObjectCOGameObjectList) {
            if (kitchenObjectCOGameObject.kitchenObjectCO == e.kitchenObjectCO) {
                kitchenObjectCOGameObject.gameObject.SetActive(true);
            }
        }
    }


}
