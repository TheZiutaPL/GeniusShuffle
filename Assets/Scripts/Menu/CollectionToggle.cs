using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionToggle : MonoBehaviour
{
    [SerializeField] CardCollection collection;
    [SerializeField] CollectionManager collectionManager;

    [SerializeField] TMP_Text pairsText;

    private void Start()
    {
        pairsText.text = collection.GetCardMatches().Count + " pairs";
        SelectCollection(true);
    }

    public void SelectCollection(bool select)
    {
        if (select)
            collectionManager.SelectCollection(collection);
        else
            collectionManager.UnselectCollection(collection);

        collectionManager.ApplyCollections();
    }
}
