using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress {

    public event EventHandler <IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned, 
    }

    [SerializeField] private FryingRecipeCO[] fryingRecipeCOArray;
    [SerializeField] private BurningRecipeCO[] burningRecipeCOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeCO fryingRecipeCO;
    private float burningTimer;
    private BurningRecipeCO burningRecipeCO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeCO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeCO.fryingTimerMax) {
                        //Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeCO.output, this);


                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeCO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectCO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = fryingTimer / fryingRecipeCO.fryingTimerMax
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = burningTimer / burningRecipeCO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeCO.burningTimerMax) {
                        //Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeCO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }


    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no KitchenObject here
            if (player.HasKitchenObject()) {
                //Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectCO())) {
                    //Player Carrying something that can be Fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeCO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectCO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeCO.fryingTimerMax
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

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                }
            } else {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectCO inputKitchenObjectCO) {
        FryingRecipeCO fryingRecipeCO = GetFryingRecipeSOWithInput(inputKitchenObjectCO);
        return fryingRecipeCO != null;
    }

    private KitchenObjectCO GetOutputForInput(KitchenObjectCO inputKitchenObjectCO) {
        FryingRecipeCO fryingRecipeCO = GetFryingRecipeSOWithInput(inputKitchenObjectCO);
        if (fryingRecipeCO != null) {
            return fryingRecipeCO.output;
        } else {
            return null;
        }
    }

    private FryingRecipeCO GetFryingRecipeSOWithInput(KitchenObjectCO inputKitchenObjectCO) {
        foreach (FryingRecipeCO fryingRecipeCO in fryingRecipeCOArray) {
            if (fryingRecipeCO.input == inputKitchenObjectCO) {
                return fryingRecipeCO;
            }
        }
        return null;
    }

    private BurningRecipeCO GetBurningRecipeSOWithInput(KitchenObjectCO inputKitchenObjectCO) {
        foreach (BurningRecipeCO burningRecipeCO in burningRecipeCOArray) {
            if (burningRecipeCO.input == inputKitchenObjectCO) {
                return burningRecipeCO;
            }
        }
        return null;
    }
}
