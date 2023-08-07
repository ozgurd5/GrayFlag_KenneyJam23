using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColorChanger : MonoBehaviour
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
    
    [Header("Yellow")]
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private List<int> yellowMaterialIndexes;

    private SkinnedMeshRenderer smr;

    private void Awake()
    {
        smr = GetComponent<SkinnedMeshRenderer>();

        PlayerColorEnabler.OnRedColorEnabled += ActiveRedColor;
        PlayerColorEnabler.OnGreenColorEnabled += ActiveGreenColor;
        PlayerColorEnabler.OnBlueColorEnabled += ActiveBlueColor;
        PlayerColorEnabler.OnYellowColorEnabled += ActiveYellowColor;
    }

    //Updating mesh renderer materials in Unity is ultra protected for several long reasons
    //Long story short: We can not change a single element of the mesh renderer's materials array
    //We can only change the materials array by assign an array to it, not a single element
    //So we must make our changes in a temporary copy array and assign it to mesh renderer materials array
    
    private void ActiveRedColor()
    {
        Material[] temporaryMaterials = smr.materials;

        foreach (int index in redMaterialIndexes)
        {
            temporaryMaterials[index] = redMaterial;
        }
        
        smr.materials = temporaryMaterials;
    }
    
    private void ActiveGreenColor()
    {
        Material[] temporaryMaterials = smr.materials;

        foreach (int index in greenMaterialIndexes)
        {
            temporaryMaterials[index] = greenMaterial;
        }
        
        smr.materials = temporaryMaterials;
    }
    
    private void ActiveBlueColor()
    {
        Material[] temporaryMaterials = smr.materials;

        foreach (int index in blueMaterialIndexes)
        {
            temporaryMaterials[index] = blueMaterial;
        }
        
        smr.materials = temporaryMaterials;
    }
    
    private void ActiveYellowColor()
    {
        Material[] temporaryMaterials = smr.materials;

        foreach (int index in yellowMaterialIndexes)
        {
            temporaryMaterials[index] = yellowMaterial;
        }
        
        smr.materials = temporaryMaterials;
    }

    private void OnDestroy()
    {
        PlayerColorEnabler.OnRedColorEnabled -= ActiveRedColor;
        PlayerColorEnabler.OnGreenColorEnabled -= ActiveGreenColor;
        PlayerColorEnabler.OnBlueColorEnabled -= ActiveBlueColor;
        PlayerColorEnabler.OnYellowColorEnabled -= ActiveYellowColor;
    }
}
