using System;
using System.Collections.Generic;
using UnityEngine;

public partial class MultiTree
{
    public TreeInstanceData[] trees;

    public MultiTree(ItemMultiTreeSO treeSO)
    {
        List<TreeInstanceData> _trees = new List<TreeInstanceData>();

        for (int i = 0; i < treeSO.trees.Length; i++)
        {
            _trees.Add((TreeInstanceData)treeSO.trees[i]);
        }
        this.trees = _trees.ToArray();
    }

    public GameObject GetRandomPrefab(bool removable)
    {
        int randomTreeSelector = UnityEngine.Random.Range(0, trees.Length -1);
        TreeInstanceData tree = trees[randomTreeSelector];
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
