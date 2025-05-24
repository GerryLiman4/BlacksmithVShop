using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private AIStateId currentState = AIStateId.None;
    [SerializeField] public NpcController npcController;
    [SerializeField] public SpriteRenderer orderBubble;

    [SerializeField] public ShopController shop;
    [Range(1, 100)]
    [SerializeField] public int itemIdRange = 98;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwitchState(currentState, true);
    }

    public void SwitchState(AIStateId nextState, bool canOverride = false)
    {
        if (currentState == nextState && !canOverride)
        {
            return;
        }

        OnStateExited();
        currentState = nextState;
        OnStateEntered();
    }

    private void OnStateEntered()
    {
        switch (currentState)
        {
            case AIStateId.Stroll:
                npcController.SwitchState(StateId.Walk);
                break;
            case AIStateId.Order:
                shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopController>();
                shop.currentCustomer = this;
                npcController.destination = shop.transform;
                npcController.reachedDestination -= OnReachedStall;
                npcController.reachedDestination += OnReachedStall;
                npcController.SwitchState(StateId.Walk);
                break;
            default:
                break;
        }
    }

    private void OnStateExited()
    {
        switch (currentState)
        {
            case AIStateId.Stroll:
                break;
            case AIStateId.Order:
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case AIStateId.Stroll:
                break;
            case AIStateId.Order:
                break;
            default:
                break;
        }
    }
    private void OnReachedStall()
    {
        npcController.reachedDestination -= OnReachedStall;
        npcController.SwitchState(StateId.Idle);
        DecideOrder();        
    }
    public const string ResourceItemDefaultPath = "Item/";

    public void DecideOrder()
    {
        int itemId = Random.Range(1, itemIdRange);
        orderBubble.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(ResourceItemDefaultPath + "Item" + itemId.ToString("D3"));
        orderBubble.gameObject.SetActive(true);
        shop.GetOrder(itemId, this);
    }

    public async void GetOrder(int itemId)
    {
        orderBubble.gameObject.SetActive(false);
        await Wait(1.25f);
        SwitchState(AIStateId.Stroll);
    }

    private async Task Wait(float waitTime)
    {
        await Task.Delay(((int)(waitTime * 1000)));
    }
}
