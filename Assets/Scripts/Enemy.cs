using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Things all enemies have in common go here. This makes it easier to keep track of all enemies regardless of type later
/// All enemies will need to inherit from this class
/// </summary>
public abstract class Enemy : MonoBehaviour {

    public GameObject[] enemy_Starting_Points; //An Array to Hold the Enemy Starting Points

    int health;
    float speed;

    public virtual void TakeDamage(int damage) {
        health -= damage;
    }

    public abstract void Die();
    public abstract void Attack();
}
