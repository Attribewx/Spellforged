using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage 
{
    void TakeDamage(float damage, float knockX, float knockY, ElementType elementDamage);
}
