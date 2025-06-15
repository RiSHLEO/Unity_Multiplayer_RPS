using UnityEngine;

public class BladeDashAbility : IFormAbility
{
    public bool ShouldActivate()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public void UseFormAbility(Player player)
    {
        Debug.LogError(player.name + "DashingActivated");
    }
}
