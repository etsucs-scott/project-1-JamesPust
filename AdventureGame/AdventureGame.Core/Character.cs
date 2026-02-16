using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame.Core
{
    public interface ICharacter
    {
        string Name { get; }
        int AttackPower { get; }
        int Health { get; }
        bool IsAlive { get; }
        void Attack(ICharacter target);
        void TakeDamage(int amount);
    }

    public class Player : ICharacter
    {
        public string Name { get; set; }
        public int AttackPower { get; set; }
        public int Health { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }


        private List<Weapon> InventoryWeapons = new List<Weapon>();

        public bool IsAlive => Health > 0; //this is added to make sure the player is alive

        public int TrueAttack => AttackPower + (InventoryWeapons.Count > 0 ? InventoryWeapons.Max(weapon => weapon.AttackMulitplier) : 0);

        public Player(string name, int row, int col, int health = 100, int attackpower = 15)
        {
            Name = name;
            Row = row;
            Col = col;
            Health = health;
            AttackPower = attackpower;
            Health = health;
        }

        public void AddWeapon(Weapon weapon)
        {
            InventoryWeapons.Add(weapon);
        }

        public void Heal(int amount)
        {
            Health += amount;
            if (Health > 100)
            {
                Health = 200;
            }
        }

        public void Attack(ICharacter target)
        {
            int damage = TrueAttack;
            target.TakeDamage(damage);
        }
        public void TakeDamage(int amount)
        {
            Health = amount;
        }

        public Weapon GetWeapon(Weapon weapon)
        {
            return weapon;
        }

        public string InvSummary()
        {
            if (InventoryWeapons.Count == 0) return "Empty";
            return string.Join(", ", InventoryWeapons.Select(static weapon => $"{weapon.Name}(+{weapon.AttackMulitplier})"));
        }
    }

    public class Monster : ICharacter
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set;}
        public bool IsAlive => Health > 0; 

        public Monster(string name, int health, int attack)
        {
            Name = name;
            Health = health;
            AttackPower = attack;
        }

        public void Attack(ICharacter target)
        {
            target.TakeDamage(AttackPower);
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
        }
    }
}
