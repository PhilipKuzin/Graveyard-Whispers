using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamagable
{
    void Attack();
    void ApplyDamage ();
    void ApplyFatalDamage();
}
