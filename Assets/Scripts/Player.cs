using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // TODO Make these variables in the game
    private const float energyGain = 0.2f;
    private const float energyLoss = 0.1f;
    private const float energyDrift = 0.1f;
    private const float keyPresentTime = 1.25f;
    private const float keyAbsentTime = 0.25f;

    private KeyCode[] keys;
    private Faction faction;
    
    private float _energy;
    private float timeUntilKeyFlip = float.PositiveInfinity;

    internal float Energy { get => _energy; private set => _energy = Mathf.Clamp01(value); }
    internal KeyCode CurrentKey { get; private set; }

    internal void Init (KeyCode[] keys, Faction faction) {
        this.keys = keys;
        this.faction = faction;
        timeUntilKeyFlip = 0.5f;

        RandomiseKey();
        
    }

    void ConquerIfNeeded () {
        if (Energy == 1f) {
            Energy = 0f;
            faction.ConquerAny();
        }
    }

    void RandomiseKey () {
        CurrentKey = keys[Random.Range(0, keys.Length)];
        print("Current key " + CurrentKey + " energy " + _energy);
        timeUntilKeyFlip = keyPresentTime;
    }

    void AnullKey () {
        CurrentKey = 0;
        timeUntilKeyFlip = keyAbsentTime;
    }

    void ProcessInput() {
        int whatNext = 0;
        foreach (KeyCode key in keys) {
            if (Input.GetKeyDown(key)) {
                if (key == CurrentKey) {
                    whatNext = 1;
                }
                else {
                    whatNext = -1;
                    break;
                }
            }
        }

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
            if (CurrentKey == 0) {
                RandomiseKey();
            } else {
                AnullKey();
            }
        }

        Energy -= energyDrift * Time.deltaTime;
        ProcessInput();
    }
}
