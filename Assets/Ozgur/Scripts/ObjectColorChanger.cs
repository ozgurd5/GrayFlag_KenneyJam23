using System.Collections.Generic;
using UnityEngine;

public class ObjectColorChanger : MonoBehaviour
{
    [Header("Red")]
    [SerializeField] private Material redMaterial;
    [SerializeField] private List<int> redMaterialIndexes;
    
    [Header("Green")]
    [SerializeField] private Material greenMaterial;
    [SerializeField] private List<int> greenMaterialIndexes;
    
    [Header("Blue")]
    [SerializeField] private Material blueMaterial;
    [SerializeField] private List<int> blueMaterialIndexes;

    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();

        PlayerColorEnabler.OnRedColorEnabled += ActiveRedColor;
        PlayerColorEnabler.OnGreenColorEnabled += ActiveGreenColor;
        PlayerColorEnabler.OnBlueColorEnabled += ActiveBlueColor;
    }

    //Updating mesh renderer materials in Unity is ultra protected for several long reasons
    //Long story short: We can not change a single element of the mesh renderer's materials array
    //We can only change the materials array by assign an array to it, not a single element
    //So we must make our changes in a temporary copy array and assign it to mesh renderer materials array
    
    private void ActiveRedColor()
    {
        Material[] temporaryMaterials = mr.materials;

        foreach (int index in redMaterialIndexes)
        {
            temporaryMaterials[index] = redMaterial;
        }
        
        mr.materials = temporaryMaterials;
    }
    
    private void ActiveGreenColor()
    {
        Material[] temporaryMaterials = mr.materials;

        foreach (int index in greenMaterialIndexes)
        {
            temporaryMaterials[index] = greenMaterial;
        }
        
        mr.materials = temporaryMaterials;
    }
    
    private void ActiveBlueColor()
    {
        Material[] temporaryMaterials = mr.materials;

        foreach (int index in blueMaterialIndexes)
        {
            temporaryMaterials[index] = blueMaterial;
        }
        
        mr.materials = temporaryMaterials;
    }
}
