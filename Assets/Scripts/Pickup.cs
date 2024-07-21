using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] public CharacterBehaviour pickUpBy;
        [SerializeField] private float speed;
        [SerializeField] protected float accel;
        protected void Update()
        {
            if (!pickUpBy) return;
            var displacement = pickUpBy.transform.position - transform.position;
            speed += accel * Time.deltaTime;
            transform.position += Mathf.Min(speed*Time.deltaTime,displacement.magnitude) * displacement.normalized;
            if (Vector3.Distance(pickUpBy.transform.position, transform.position) < 0.1f)
            {
                GrantPickupTo(pickUpBy);
            }
        }

        protected virtual void GrantPickupTo(CharacterBehaviour chara)
        {
            
        }
    }

}
