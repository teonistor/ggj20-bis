using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // TODO Make these variables in the game
    private const float energyGain = 0.8f; //.2f;
    private const float energyLoss = 0.1f;
    private const float energyDrift = 0.1f;
    private const float keyPresentTime = 1.25f;
    private const float keyAbsentTime = 0.25f;

    private Dictionary<KeyDirection,KeyCode> keys;
    internal Faction faction { get; private set; }
    
    private float _energy;
    private float timeUntilKeyFlip = float.PositiveInfinity;

    internal float Energy { get => _energy; private set => _energy = Mathf.Clamp01(value); }

    internal Spot CurrentlyAttacking { get; private set; }
    internal KeyDirection CurrentKeyDirection { get; private set; }
    internal KeyCode CurrentKeyCode { get; private set; }
    internal Spot AlternateAttacking { get; private set; }
    internal KeyDirection AlternateKeyDirection { get; private set; }
    internal KeyCode AlternateKeyCode { get; private set; }

    internal void Init (KeyCode[] keys, Spot startingSpot, Faction faction) {
        this.keys = new Dictionary<KeyDirection, KeyCode>();
        this.keys[KeyDirection.Left] = keys[0];
        this.keys[KeyDirection.Up] = keys[1];
        this.keys[KeyDirection.Right] = keys[2];
        this.keys[KeyDirection.Down] = keys[3];
        this.faction = faction;
        timeUntilKeyFlip = 0.5f;

        faction.Init(this, startingSpot);
        RandomiseKey();
    }

    void ConquerIfNeeded () {
        if (Energy == 1f) {
            Energy = 0f;
            faction.Conquer(CurrentlyAttacking);
            CurrentlyAttacking = null;
        }
    }

    void RandomiseKey () {
        if (CurrentlyAttacking == null) {
            Faction a, b;
            HashSet<Faction> found = faction.FindNeighbouringFactionsExceptSelf();
            found.UpToTwoRandomElements(out a, out b);
            printIf("Factions of " + faction.gameName +": " + a?.gameName + ", " + b?.gameName + " out of " + found.Count);
            if (a != null) {
                CurrentlyAttacking = faction.FindNeighbouringSpotsOf(a).RandomElement();
                if (b != null) {
                    AlternateAttacking = faction.FindNeighbouringSpotsOf(b).RandomElement();
                }
            }
        }

        if (CurrentlyAttacking != null) {
            CurrentKeyDirection = keys.RandomKey();
            CurrentKeyCode = keys[CurrentKeyDirection];
            printIf(faction.gameName + ": Randomised key to " + CurrentKeyDirection + " | " + CurrentKeyCode + " attacking " + CurrentlyAttacking);

            if (AlternateAttacking != null) {
                do {
                    AlternateKeyDirection = keys.RandomKey();
                } while (CurrentKeyDirection == AlternateKeyDirection);  // TODO FInd a better way
                AlternateKeyCode = keys[AlternateKeyDirection];
                printIf(faction.gameName + ": Also alternate key " + AlternateKeyDirection + " | " + AlternateKeyCode + " attacking " + AlternateAttacking);
            }
        }

        timeUntilKeyFlip = keyPresentTime;
    }

    void AnullKey () {
        CurrentKeyCode = 0;
        AlternateKeyCode = 0;
        timeUntilKeyFlip = keyAbsentTime;
    }

    void ProcessInput() {
        int whatNext = 0;
        foreach (KeyCode key in keys.Values) {
            if (Input.GetKeyDown(key)) {
                if (key == CurrentKeyCode) {
                    whatNext = 1;
                    AlternateKeyCode = 0;
                    AlternateAttacking = null;
                }
                else {
                    if (key == AlternateKeyCode) {
                        whatNext = 1;
                        CurrentKeyCode = AlternateKeyCode;
                        CurrentlyAttacking = AlternateAttacking;
                        CurrentKeyDirection = AlternateKeyDirection;
                        AlternateKeyCode = 0;
                        AlternateAttacking = null;
                    } else {

                        whatNext = -1;
                        break;
                    }
                }
            }
        }
        //printIf(faction.gameName + " what next " + whatNext);

        switch (whatNext) {
            case 1:
                Energy += energyGain;
                AnullKey();
                break;
            case -1:
                Energy -= energyLoss;
                AnullKey();
                break;
        }
    }

    void Update () {
        ConquerIfNeeded();

        timeUntilKeyFlip -= Time.deltaTime;
        
        if (timeUntilKeyFlip <= 0f) {
            if (CurrentKeyCode == 0) {
                //printIf(faction.gameName + " randomising keys");
                RandomiseKey();
            } else {
                //printIf(faction.gameName + " anulling keys");
                AnullKey();
            }
        }

        Energy -= energyDrift * Time.deltaTime;
        ProcessInput();
    }

    internal void NotifyConquer() {
        CurrentlyAttacking = null;
        AnullKey();
    }

    void printIf(string s) {
        if (faction.gameName.Equals("Covid")) {
            print(s);
        }
    }
}
