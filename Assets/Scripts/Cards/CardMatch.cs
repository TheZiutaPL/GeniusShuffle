using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Match", menuName = "Scriptable Objects/Cards/Card Match")]
public class CardMatch : ScriptableObject
{
    public CardData inventorCard;

    [Space(10)]
    [SerializeField] private CardData[] inventionCards = new CardData[0];
    public CardData GetRandomInventionCard() => inventionCards[Random.Range(0, inventionCards.Length)];
}
