using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ShopController shop;
    [SerializeField] private Transform[] boundaryList;
    [SerializeField] private Vector2 npcMaxMinSpawnTime = new Vector2(5.0f , 20f);

    [SerializeField] public NpcController npcPrefab;
    [Range(1,10)]
    [SerializeField] public int npcIdRangeMax = 1;

    public float spawnTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SignalManager.OnGoldUpdated += OnGoldUpdated;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void FixedUpdate()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0f) {
            // Spawn NPC
            SpawnRandomNPC();
            SetRandomNPCSpawnTime();
        }

    }

    private void SetRandomNPCSpawnTime()
    {
        spawnTime = UnityEngine.Random.Range(npcMaxMinSpawnTime.x, npcMaxMinSpawnTime.y);
    }

    public void SpawnRandomNPC()
    {
        if (boundaryList.Length <= 0) return;

        int spawnPositionIndex = UnityEngine.Random.Range(0,2);

        NpcController instantiatedObject = Instantiate<NpcController>(npcPrefab,boundaryList[spawnPositionIndex].position, Quaternion.identity);
        instantiatedObject.Initialize(UnityEngine.Random.Range(1, npcIdRangeMax + 1));
        AIController aiController = instantiatedObject.GetComponent<AIController>();

        // check first if shop has customer
        if(shop.currentCustomer == null) {
            aiController.SwitchState(AIStateId.Order);
            return;
            
        }

        aiController.SwitchState(AIStateId.Stroll);
        if (spawnPositionIndex == 1) instantiatedObject.FlipFacingDirection(false);
    }

    private void OnGoldUpdated(int updatedGold)
    {
        PlayerPrefs.SetInt("currency", updatedGold);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
