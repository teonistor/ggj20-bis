using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUI : MonoBehaviour
{

    [SerializeField] private List<PlayerUI> playerUis;
    private static IEnumerator<PlayerUI> nextPlayerUI;

    void Awake() {
        nextPlayerUI = playerUis.GetEnumerator();
    }

    internal static void AddPlayerUI(Player player, Faction faction) {
        nextPlayerUI.MoveNext();
        PlayerUI p = nextPlayerUI.Current;
        p.gameObject.SetActive(true);
        p.Init(player, faction);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
