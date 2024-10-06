using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    public event EventHandler <IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeCO[] cuttingRecipeCOArray;


    private int cuttingProgress;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no KitchenObject here
            if (player.HasKitchenObject()) {
                //Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectCO())) {
                    //Player Carrying something that can be Cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeCO cuttingRecipeCO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectCO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeCO.cuttingProgressMax
                    });
                }
            } else {
                //Player not carrying anything
            }
        } else {
            // There is a KitchenObject here
            if (player.HasKitchenObject()) {
                //Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectCO())) {
                        GetKitchenObject().DestroySelf();
                    }
                }
            } else {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectCO())) {
            //There is a kitchenObject here and it can be cut
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeCO cuttingRecipeCO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectCO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeCO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeCO.cuttingProgressMax) {
                KitchenObjectCO outputKitchenObjectCO = GetOutputForInput(GetKitchenObject().GetKitchenObjectCO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectCO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectCO inputKitchenObjectCO) {
        CuttingRecipeCO cuttingRecipeCO = GetCuttingRecipeSOWithInput(inputKitchenObjectCO);
        return cuttingRecipeCO != null;
    }

    private KitchenObjectCO GetOutputForInput(KitchenObjectCO inputKitchenObjectCO) {
        CuttingRecipeCO cuttingRecipeCO = GetCuttingRecipeSOWithInput(inputKitchenObjectCO);
        if (cuttingRecipeCO != null) {
            return cuttingRecipeCO.output;
        } else {
            return null;
        }
    }

    private CuttingRecipeCO GetCuttingRecipeSOWithInput(KitchenObjectCO inputKitchenObjectCO) {
        foreach (CuttingRecipeCO cuttingRecipeCO in cuttingRecipeCOArray) {
            if (cuttingRecipeCO.input == inputKitchenObjectCO) {
                return cuttingRecipeCO;
            }
        }
        return null;
    }
}
