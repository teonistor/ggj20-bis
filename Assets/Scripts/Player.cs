using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private const float energyGain = 0.2f;
    private const float energyLoss = 0.1f;
    private const float energyDrift = 0.1f;

    private KeyCode[] keys;
    private Faction faction;

    private KeyCode currentKey;
    private float _energy;

    internal float Energy { get => _energy; private set => _energy = Mathf.Clamp01(value); }

    internal void Init (KeyCode[] keys, Faction faction) {
        this.keys = keys;
        this.faction = faction;

        RandomiseKey();
    }

    void ConquerIfNeeded () {
        if (Energy == 1f) {
            Energy = 0f;
            faction.ConquerAny();
        }
    }

    void RandomiseKey () {
        currentKey = keys[Random.Range(0, keys.Length)];
        print("Current key " + currentKey + " energy " + _energy);
    }

    void ProcessInput() {
        int whatNext = 0;
        foreach (KeyCode key in keys) {
            if (Input.GetKeyDown(key)) {
                if (key == currentKey) {
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
                RandomiseKey();
                break;
            case -1:
                Energy -= energyLoss;
                RandomiseKey();
                break;
        }
    }

    void Update () {
        ConquerIfNeeded();
        Energy -= energyDrift * Time.deltaTime;
        ProcessInput();
    }
}
