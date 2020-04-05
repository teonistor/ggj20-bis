using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class StickyButton : MonoBehaviour {
    [SerializeField] private int factionIndex;
    [SerializeField] private Sprite pressed;
    private Sprite released;

    private Image image;

    void Start () {
        image = GetComponent<Image>();
        released = image.sprite;
        image.sprite = Game.IsFactionActive(factionIndex) ? pressed : released;
    }
    
    public void Press () {
        image.sprite = Game.PressButtonForFaction(factionIndex) ? pressed : released;
    }
}
