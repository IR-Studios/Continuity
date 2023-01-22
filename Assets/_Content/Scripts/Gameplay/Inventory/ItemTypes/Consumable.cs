using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum type { Health, Food, Drink, Potion }
[CreateAssetMenu (fileName = "new Consumable Item", menuName = "Items/Consumable")]
public class Consumable : IR_Item
{
    public type type;
    [Tooltip("If a healing item, this is the amount of health restored to the player.")]
    public int heal = 0;
    [Tooltip("If a food item, this is the amount of hunger restored to the player.")]
    public int hunger = 0;
    [Tooltip("If a drink item, this is the amount of thirst restored to the player.")]
    public int thirst = 0;

    public override void Use()
    {
        //Implement use function here. 
        if (type == type.Health) 
        {
            Player.instance.Heal(heal);
        } else if (type == type.Food) 
        {
            Player.instance.Eat(hunger);
        } else if (type == type.Drink) 
        {
            Player.instance.Drink(thirst);
        } else if (type == type.Potion) 
        {
            
        }

    }
}
