using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Collection", menuName = "Scriptable Objects/Cards/Card Collection")]
public class CardCollection : ScriptableObject
{
    public string collectionName;

    [SerializeField] private CardMatch[] cardMatches = new CardMatch[0];
    public List<CardMatch> GetCardMatches() => new List<CardMatch>(cardMatches);
}
