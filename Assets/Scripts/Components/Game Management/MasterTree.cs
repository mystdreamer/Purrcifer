using ItemPool;
using UnityEngine;

/// <summary>
/// Component script holding references to all item trees, acts as a hub for interfacing. 
/// </summary>
public class MasterTree : MonoBehaviour
{
    /// <summary>
    /// The singleton reference to the MasterPool. 
    /// </summary>
    private static MasterTree _instance;

    /// <summary>
    /// The start room tree. 
    /// </summary>
    private ItemBBST _startRoomTree; 
    
    /// <summary>
    /// The normal room tree. 
    /// </summary>
    private ItemBBST _normalRoomTree; 

    /// <summary>
    /// The boss room prefab tree. 
    /// </summary>
    private ItemBBST _bossRoomTree;

    /// <summary>
    /// The boss prefab tree. 
    /// </summary>
    private ItemBBST _bossPrefabTree; 
    
    /// <summary>
    /// The treasure room prefab tree. 
    /// </summary>
    private ItemBBST _treasureRoomTree;

    /// <summary>
    /// The item prefab tree. 
    /// </summary>
    private MultiTree _itemTree; 

    /// <summary>
    /// Scriptable object instance used to generate the start room tree. 
    /// </summary>
    [SerializeField] private ItemTreeSO startRoomTree;

    /// <summary>
    /// Scriptable object instance used to generate the normal room tree. 
    /// </summary>
    [SerializeField] private ItemTreeSO normalRoomTree;

    /// <summary>
    /// Scriptable object instance used to generate the treasure room tree. 
    /// </summary>
    [SerializeField] private ItemTreeSO treasureRoomTree;

    /// <summary>
    /// Scriptable object instance used to generate the item prefab tree. 
    /// </summary>
    [SerializeField] private ItemMultiTreeSO itemPrefabTree;

    /// <summary>
    /// Scriptable object instance used to generate the boss room tree. 
    /// </summary>
    [SerializeField] private ItemTreeSO bossRoomTree;

    /// <summary>
    /// Scriptable object instance used to generate the boss prefab tree. 
    /// </summary>
    [SerializeField] private ItemTreeSO bossPrefabTree;

    /// <summary>
    /// Returns a reference to the current master tree instance. 
    /// </summary>
    public static MasterTree Instance => _instance;

    /// <summary>
    /// Returns the current start room tree. 
    /// </summary>
    public ItemBBST StartRoomTree => _startRoomTree;
    
    /// <summary>
    /// Returns the current normal room tree. 
    /// </summary>
    public ItemBBST NormalRoomTree => _normalRoomTree;
    
    /// <summary>
    /// Returns the current boss room tree. 
    /// </summary>
    public ItemBBST BossRoomTree => _bossRoomTree;

    /// <summary>
    /// Returns the current boss prefab tree. 
    /// </summary>
    public ItemBBST BossPrefabTree => _bossPrefabTree;
    
    /// <summary>
    /// Returns the current treasure room tree. 
    /// </summary>
    public ItemBBST TreasureRoomTree => _treasureRoomTree;
    
    /// <summary>
    /// Returns the current item tree. 
    /// </summary>
    public MultiTree ItemTree => _itemTree;

    private void Awake()
    {
        //Generate singleton. 
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
            DestroyImmediate(_instance);

        //Initialise all the trees. 
        _startRoomTree = new ItemBBST(startRoomTree);
        _normalRoomTree = new ItemBBST(normalRoomTree);
        _bossRoomTree = new ItemBBST(bossRoomTree);
        _bossPrefabTree = new ItemBBST(bossPrefabTree);
        _treasureRoomTree = new ItemBBST(treasureRoomTree);
        _itemTree = new MultiTree(itemPrefabTree);
    }
}
