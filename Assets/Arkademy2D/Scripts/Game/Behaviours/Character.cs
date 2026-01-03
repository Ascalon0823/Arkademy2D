using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Game.Data.Static;
using Arkademy2D.Game.Interaction;
using UnityEngine;
using Attribute = Arkademy2D.Game.Data.Runtime.Attribute;

namespace Arkademy2D.Game.Behaviours
{
    public class Character : MonoBehaviour
    {
        [Header("Data")]
        public Core.Models.Character model;
        public CharacterBase @base;
        [Header("Health")]
        public Attribute maxHp;
        public int faction;
        public int hp;
        [Header("Energy")]
        public Attribute maxEnergy;
        public Attribute energyRegen;
        [SerializeField] private float energyFloat;
        public int energy => Mathf.RoundToInt(energyFloat);
      
        [Header("Movement")] public Attribute moveSpeed;
        public Vector2 moveDir;
        public Vector2 faceDir;
        [Header("Inventory")] public List<ItemData> items;
        [Header("Usables")] public List<Usable> usables;
        [Header("Interaction")] public Interactable interactionCandidate;
        public float interactableDetectionRange;
        [Header("Caster")] public List<string> castKeys;
        public bool showCasting => casting && hp > 0;
        public bool casting;

        [Header("Components")] [SerializeField]
        private Rigidbody2D body;

        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D collision;

        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            if (model != null)
            {
                @base = CharacterBase.Get(model.CharacterBaseId);
            }

            if (@base)
            {
                maxHp.config = @base.attributeConfigs.FirstOrDefault(x => x.@base == maxHp.config.@base);
                maxEnergy.config = @base.attributeConfigs.FirstOrDefault(x => x.@base == maxEnergy.config.@base);
                energyRegen.config = @base.attributeConfigs.FirstOrDefault(x => x.@base == energyRegen.config.@base);
                moveSpeed.config = @base.attributeConfigs.FirstOrDefault(x => x.@base == moveSpeed.config.@base);
            }

            hp = maxHp.Value;
            energyFloat = maxEnergy.Value;

            if (!animator) animator = GetComponent<Animator>();
            if (!collision) collision = GetComponent<Collider2D>();
            if (!body) body = GetComponent<Rigidbody2D>();
        }

        public void Cast(string key)
        {
            if (hp <= 0) return;
            castKeys ??= new List<string>();
            if (castKeys.Contains(key)) return;
            castKeys.Add(key);
        }

        public Dictionary<SpellBase, Usable> SpellUsables = new Dictionary<SpellBase, Usable>();
        public void EndCast()
        {
            if (hp <= 0) return;
            if (castKeys == null || castKeys.Count == 0) return;
            var spellBase = SpellBase.GetSpellByKey(string.Join("", castKeys));
            if (!SpellUsables.TryGetValue(spellBase, out var spellUsable))
            {
                spellUsable = Instantiate(spellBase.spellUsablePrefab, transform);
                SpellUsables[spellBase] = spellUsable;
                spellUsable.user = this;
            }
            if (energyFloat < maxEnergy.Value) return;
            if (!spellBase || !spellUsable || !spellUsable.CanUse()) return;
            Debug.Log($"Use spell: {spellBase.displayName}", spellBase);
            spellUsable.Use();
        }

        public void ConsumeEnergy(int amount)
        {
            energyFloat -= amount;
            energyFloat = Mathf.Clamp(energyFloat, 0, maxEnergy.Value);
        }

        public void TakeDamage(int damage)
        {
            if (hp <= 0) return;
            hp -= damage;
            hp = Mathf.Clamp(hp, 0, maxHp.Value);
            animator.SetTrigger("hit");
        }

        private void Update()
        {
            if (hp <= 0) return;
            energyFloat += energyRegen.Value * Time.deltaTime;
            energyFloat = Mathf.Clamp(energyFloat, 0, maxEnergy.Value);
            if (moveDir.sqrMagnitude > float.Epsilon)
            {
                faceDir = moveDir;
            }
        }

        private void FixedUpdate()
        {
            collision.isTrigger = hp <= 0;
            if (hp <= 0) return;
            interactionCandidate = Physics2D.OverlapCircleAll(transform.position, interactableDetectionRange)?
                .Select(x => x.GetComponent<Interactable>())?
                .Where(x => x)?
                .OrderBy(x => Vector2.Distance(x.transform.position, transform.position))?
                .FirstOrDefault();
            body.MovePosition(body.position + moveDir.normalized * moveSpeed.Value / 100f * Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            animator.SetBool("walking", moveDir.sqrMagnitude > float.Epsilon && hp > 0);
            animator.SetBool("dead", hp <= 0);
        }

        public void SetPosition(Vector2 pos)
        {
            body.position = pos;
        }
    }
}