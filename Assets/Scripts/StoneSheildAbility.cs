using UnityEngine;

public class StoneSheildAbility : IFormAbility
{
    public bool ShouldActivate()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public void UseFormAbility(Player player)
    {
        Debug.LogError("SheildActivated");
    }
}
