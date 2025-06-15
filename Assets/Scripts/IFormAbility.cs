using UnityEngine;

public interface IFormAbility
{
    bool ShouldActivate();
    void UseFormAbility(Player player);
}
