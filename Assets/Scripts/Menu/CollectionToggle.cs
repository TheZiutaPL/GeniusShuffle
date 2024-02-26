using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionToggle : MonoBehaviour
{
    [SerializeField] CardCollection collection;

    [SerializeField] TMP_Text pairsText;

    private void Start()
    {
        pairsText.text = collection.GetPairCount() + " pairs";
        SelectCollection(true);
    }

    public void SelectCollection(bool select)
    {
        if (select)
            CollectionManager.SelectCollection(collection);
        else
            CollectionManager.UnselectCollection(collection);

        CollectionManager.ApplyCollections();
    }
}
