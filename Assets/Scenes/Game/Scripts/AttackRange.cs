using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public Action OnContact;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnContact?.Invoke();
    }
}
