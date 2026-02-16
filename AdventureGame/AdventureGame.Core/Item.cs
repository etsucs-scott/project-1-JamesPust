using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame.Core
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public Item(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public abstract void ApplyTo(Player player);
    }

    public class Weapon : Item
    {
        public int AttackMulitplier { get; set; }

        public Weapon(string name, int attackMulitplier) : base(name, $"Picked up {name} (+{attackMulitplier} attack)")
        {
            AttackMulitplier = attackMulitplier;
        }

        public override void ApplyTo(Player player)
        {
            player.AddWeapon(this);
        }
    }

    public class Potion : Item
    {
        public int HealthBoost { get; set; }
        public Potion(string name, int healthboost) : base(name, $"You drank {name} and gain {healthboost} health back!")
        {
            HealthBoost = healthboost;
        }

        public override void ApplyTo(Player player)
        {
            player.Heal(HealthBoost);
        }
    }
}
