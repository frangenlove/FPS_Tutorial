using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieAttackHand attackHand;
    public int zombieDamage;
    private void Start()
    {
        attackHand.damage = zombieDamage;
    }
}
