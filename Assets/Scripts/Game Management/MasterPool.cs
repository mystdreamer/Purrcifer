using ItemPool;
using UnityEngine;

public class MasterPool : MonoBehaviour
{
    private static MasterPool _instance;

    private ItemBBST _startRoomTree; 
    private ItemBBST _normalRoomTree; 
    private ItemBBST _bossRoomTree;
    private ItemBBST _bossPrefabTree; 
    private ItemBBST _treasureRoomTree;
    private MultiTree _itemTree; 

    [SerializeField] private ItemTreeSO startRoomTree;
    [SerializeField] private ItemTreeSO normalRoomTree;
    [SerializeField] private ItemTreeSO treasureRoomTree;
    [SerializeField] private ItemMultiTreeSO itemPrefabTree;
    [SerializeField] private ItemTreeSO bossRoomTree;
    [SerializeField] private ItemTreeSO bossPrefabTree;

    public static MasterPool Instance => _instance;

    public ItemBBST StartRoomTree => _startRoomTree;
    
    public ItemBBST NormalRoomTree => _normalRoomTree;
    
    public ItemBBST BossRoomTree => _bossRoomTree;

    public ItemBBST BossPrefabTree => _bossPrefabTree;
    
    public ItemBBST TreasureRoomTree => _treasureRoomTree;
    
    public MultiTree ItemTree => _itemTree;

    private void Awake()
    {
        //Generate singleton. 

        //Initialise all the trees. 
        _startRoomTree = new ItemBBST(startRoomTree);
        _normalRoomTree = new ItemBBST(normalRoomTree);
        _bossRoomTree = new ItemBBST(bossRoomTree);
        _bossPrefabTree = new ItemBBST(bossPrefabTree);
        _treasureRoomTree = new ItemBBST(treasureRoomTree);
        _itemTree = new MultiTree(itemPrefabTree);
    }
}
