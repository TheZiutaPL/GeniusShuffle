using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGraveyard : MonoBehaviour
{
    private List<CardObject> cardGraveyard = new List<CardObject>();

    public void AddCardToGraveyard(CardObject card) => cardGraveyard.Add(card);
}
