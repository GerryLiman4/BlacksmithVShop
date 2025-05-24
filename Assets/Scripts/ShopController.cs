using System.Collections;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private float minPrepareOrderTime = 1.0f;
    [SerializeField] private float maxPrepareOrderTime = 3.0f;

    [SerializeField] private TextMeshProUGUI goldText;

    public AIController currentCustomer = null;

    private void Start()
    {
        int updatedGold = PlayerPrefs.GetInt("currency", 0);

        UpdateGoldText("GOLD : " + updatedGold);
    }

    public void GetOrder(int itemId, AIController customer)
    {
        currentCustomer = customer;
        print("Shop Get Order");
        StartCoroutine(PrepareOrder(itemId, customer));
    }
    
    private IEnumerator PrepareOrder(int itemId, AIController customer)
    {
        print("Shop PrepareOrder 1");
        AIController designatedCustomer = customer;
        int designatedItemId = itemId;
        yield return new WaitForSeconds(Random.Range(minPrepareOrderTime,maxPrepareOrderTime));
        print("Shop PrepareOrder 2");
        ServeOrder(designatedItemId, designatedCustomer);
    }

    public void ServeOrder(int itemId, AIController customer)
    {
        customer.GetOrder(itemId);
        currentCustomer = null;

        int updatedGold = PlayerPrefs.GetInt("currency", 0) + Random.Range(10,31);

        UpdateGoldText("GOLD : " + updatedGold);
        print("Updated Gold : " + updatedGold);
        SignalManager.UpdateGold(updatedGold);
    }

    public void UpdateGoldText(string text) 
    {
        goldText.text = text;
    }
}
