using System;
using UnityEngine;

namespace Midterm.Character
{
    public class Spell : MonoBehaviour
    {
        public string internalName;
        public string key;
        public float energyCost;
        public Character user;
        public bool casting;
        public Sprite icon;

        public virtual bool CanCast()
        {
            return user.energy >= energyCost;
        }
        public virtual void BeginUse(Vector2 pos)
        {
            if (!CanCast()) return;
            casting = true;
            Use(pos);
        }
        
        public virtual void Use(Vector2 pos)
        {
            if (!casting) return;
            if (!CanCast() && casting)
            {
                EndUse(pos);
                return;
            }

            user.energy -= energyCost * Time.deltaTime;
        }

        public virtual void EndUse(Vector2 pos)
        {
            if (!casting) return;
            casting = false;
        }
    }
}