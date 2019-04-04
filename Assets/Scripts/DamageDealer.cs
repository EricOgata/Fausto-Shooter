using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {

    [SerializeField] int damage = 1;
	
    public int GetDamage() {
        return this.damage;
    }

    public void SetDamage(int damage) {
        this.damage = damage;
    }

    public void Hit() {
        Destroy(gameObject);
    }
}
