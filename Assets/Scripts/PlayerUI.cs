using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField] private Text text;
    [SerializeField] private Image energyBar, energyContour, boostBar, boostContour;
    [SerializeField] private GameObject keyText;

    private Player player;
    private TextMesh keyTextInstance;
    private RectTransform energyBarRectTransform, boostBarRectTransform;
    private float energyBarOffsetMaxX, energyBarOffsetMaxY, boostBarOffsetMaxX, boostBarOffsetMaxY;

    internal void Init(Player player, Faction faction) {
        this.player = player;
        text.text = faction.gameName;
        text.color = faction.color;
        energyBar.color = faction.color;
        boostBar.color = faction.color;

        energyBarRectTransform = energyBar.GetComponent<RectTransform>();
        boostBarRectTransform = boostBar.GetComponent<RectTransform>();
        energyBarOffsetMaxX = energyBarRectTransform.offsetMax.x;
        energyBarOffsetMaxY = energyBarRectTransform.offsetMax.y;
        boostBarOffsetMaxX = boostBarRectTransform.offsetMax.x;
        boostBarOffsetMaxY = boostBarRectTransform.offsetMax.y;

        boostBar.gameObject.SetActive(false);
        boostContour.gameObject.SetActive(false);

        keyTextInstance = Instantiate(keyText).GetComponent<TextMesh>();
        keyTextInstance.color = faction.color;
    }

    void Start () {

    }

    void Update () {
        if (player) {
            //print("Min " + energyBarRectTransform.offsetMin + " max " + energyBarRectTransform.offsetMax);
            //print("AMin " + energyBarRectTransform.anchorMin + " amax " + energyBarRectTransform.anchorMax);
            energyBarRectTransform.offsetMax = new Vector2(Mathf.Lerp(energyBarRectTransform.offsetMin.x, energyBarOffsetMaxX, player.Energy), energyBarOffsetMaxY);

            //text.text = string.Format(textFormat,
            //    //player.CurrentKey == 0 ? "" : player.CurrentKey.ToString(),
            //    "".PadRight(Mathf.CeilToInt(player.Energy * 15), '|').PadRight(15));


            keyTextInstance.text = NiceKey(player.CurrentKey);
            Spot plausibleSpot = player.faction.FindPlausibleSpotToConquer();
            if (plausibleSpot != null) {
                keyTextInstance.transform.SetParent(plausibleSpot.transform, false);
            }
        }
    }

    string NiceKey(KeyCode keyCode) {
        switch(keyCode) {
            case 0: return "";
        // Arrows are such a bitch! It's hard to find a set that works in Unity. These triangles are nice, but a bit too easily confused...
            case KeyCode.LeftArrow:
            case KeyCode.A: return "◀";
            case KeyCode.RightArrow:
            case KeyCode.D: return "▶"; // "➔";
            case KeyCode.UpArrow:
            case KeyCode.W: return "▲";
            case KeyCode.DownArrow:
            case KeyCode.S: return "▼";
            default: return keyCode.ToString();
        }
    }
}
