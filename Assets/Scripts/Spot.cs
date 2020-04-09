using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

    // Automated by custom editor button
    [SerializeField] internal List<Spot> neighbours;

    private Faction owner;
    private SpriteRenderer spriteRenderer;
    private Dictionary<Faction,List<Spot>> neighboursByOwner;

    // private Dictionary<Faction,Spot>

    void Awake () {
        neighboursByOwner = new Dictionary<Faction,List<Spot>>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator Start() {
        // On the first frame human assigns itself to all spots, only after that can we recalculate neighbours dictionary
        yield return new WaitForEndOfFrame();
        RecalculateNeighboursByOwner();
    }

    internal void InitialAssignHuman(Faction human) {
        owner = human;
        spriteRenderer.color = human.color;
    }

    internal void Conquer(Faction conqueror) {
        owner.Disown(this);
        owner = conqueror;
        spriteRenderer.color = conqueror.color;
        foreach (Spot spot in neighbours) {
            // Technically I could just say "recalculate me" but doing it from the ground up is safer
            spot.RecalculateNeighboursByOwner();
        }
    }

    internal void RecalculateNeighboursByOwner () {
        neighboursByOwner.Clear();
        foreach (Spot spot in neighbours) {
            if (!neighboursByOwner.ContainsKey(spot.owner)) {
                neighboursByOwner[spot.owner] = new List<Spot>();
            }
            neighboursByOwner[spot.owner].Add(spot);
        }
    }

    internal void ForEachNeighbouringFactionExcept(Faction excepted, System.Func<Faction,bool> consume) {
        foreach (Faction faction in neighboursByOwner.Keys) {
            if (faction != excepted) {
                consume(faction);
            }
        }
    }

    internal void ForEachNeighbouringSpotOf (Faction faction, System.Action<Spot> consume) {
        if (!neighboursByOwner.ContainsKey(faction)) {
            Debug.LogWarning("Expecting to contain " + faction.gameName + " but wasn't there");
            return;
        }
        foreach (Spot spot in neighboursByOwner[faction]) {
            consume(spot);
        }
    }
    
    // To be called by custom editor button
    internal void RecalculateNeighbours () {
        neighbours = new List<Spot>();
        foreach (Collider c in Physics.OverlapSphere(transform.position, 1f)) {
            Spot s = c.GetComponent<Spot>();
            if (s != null && s != this) {
                neighbours.Add(s);
            }
        }

        if (neighbours.Count > 0 && neighbours.Count < 5) {
            Debug.Log("Recalculated neighbours of " + gameObject.name + ", found " + neighbours.Count);
        } else {
            Debug.LogWarning("Recalculated neighbours of " + gameObject.name + ", found " + neighbours.Count);
        }
    }

}
