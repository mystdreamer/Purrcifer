using System;
using System.Collections.Generic;
using UnityEngine;

public partial class MultiTree
{
    public TreeData[] trees;

    public MultiTree(ItemMultiTreeSO treeSO)
    {
        List<TreeData> _trees = new List<TreeData>();

        for (int i = 0; i < treeSO.trees.Length; i++)
        {
            _trees.Add((TreeData)treeSO.trees[i]);
        }
        this.trees = _trees.ToArray();
    }

    public GameObject GetRandomPrefab(bool removable)
    {
        int randomTreeSelector = UnityEngine.Random.Range(0, trees.Length -1);
        Console.WriteLine("RandSelector: " + randomTreeSelector);
        TreeData tree = trees[randomTreeSelector];
        return tree.ItemTree.GetRandomPrefab(removable);
    }

    #region Search Utils. 
    public int GetTreeIDByName(string name)
    {
        for (int i = 0; i < trees.Length; i++)
        {
            if (trees[i].Name == name)
                return trees[i].ID;
        }
        throw new SystemException(">> Requested tree with string " + name + " could not be found in data. ");
    }

    public string GetTreeName(int id)
    {
        for (int i = 0; i < trees.Length; i++)
        {
            if (trees[i].ID == id)
                return trees[i].Name;
        }

        return null;
    }
    #endregion

    public static explicit operator MultiTree(ItemMultiTreeSO itemMultiTreeSO)
    {
        return new MultiTree(itemMultiTreeSO); 
    }

    //TODO: 
    // - Implement a method of removing a tree from a given subpool. 
}
