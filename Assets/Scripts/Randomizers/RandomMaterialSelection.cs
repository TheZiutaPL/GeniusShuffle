using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialSelection : MonoBehaviour
{
    [System.Serializable]
    private class RandomizedMaterialUnit
    {
        [SerializeField] private string name;
        [SerializeField] private bool instantiateNew;
        [Space(10)]
        [SerializeField] private MeshRenderer[] meshRenderers = new MeshRenderer[0];
        [SerializeField] private Material[] randomMaterials = new Material[0];

        public void RandomizeMaterial()
        {
            Material randomMaterial = randomMaterials[Random.Range(0, randomMaterials.Length)];

            foreach (MeshRenderer item in meshRenderers)
                item.material = instantiateNew ? Instantiate(randomMaterial) : randomMaterial;
        }
    }

    [SerializeField] private bool randomizeOnAwake = true;
    [SerializeField] private RandomizedMaterialUnit[] randomizedMaterialUnits = new RandomizedMaterialUnit[0];

    private void Awake()
    {
        if (randomizeOnAwake)
            Randomize();
    }

    public void Randomize()
    {
        foreach (RandomizedMaterialUnit item in randomizedMaterialUnits)
            item.RandomizeMaterial();
    }
}
