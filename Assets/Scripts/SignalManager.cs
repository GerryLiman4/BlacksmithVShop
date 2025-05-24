using System;

public static class SignalManager
{
    public static event Action<int> OnGoldUpdated;

    public static void UpdateGold(int updatedAmount)
    {
        OnGoldUpdated?.Invoke(updatedAmount);
    }

}
