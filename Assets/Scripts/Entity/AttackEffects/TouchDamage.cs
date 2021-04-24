﻿using UnityEngine;
using System.Collections;

namespace Entities.AttackEffects {

    [CreateAssetMenu(fileName = "TouchDamage", menuName = "AttackEffects/TouchDamage")]
    public class TouchDamage : Effect {

        protected Collider2D collider;

        protected Collider2D[] results;

        [SerializeField]
        protected ContactFilter2D filter;

        [SerializeField]
        protected int damage;

        protected void Awake() {
            results = new Collider2D[3];
        }

        public override bool PerTick(Effectable eff) {
            Debug.Log(eff.gameObject.name);
            if(collider == null) {
                collider = eff.GetComponent<Collider2D>();
            }

            if(collider == null) {
                return false;
            }

            var num = collider.OverlapCollider(filter, results);
            bool hasHit = false;

            for(int i = 0; i < num; i++) {
                if(results[i].gameObject == eff.gameObject) {
                    continue;
                }

                var res = results[i].GetComponent<Resources>();
                
                if(res != null) {
                    res.Damage(damage);
                    hasHit = true;
                    OnHitEffects(eff, res);
                }
            }

            return hasHit;
        }

        private void OnHitEffects(Effectable eff, Resources res) {
            foreach(Effect e in eff.effects) {
                if(e == this) {
                    continue;
                }

                e.OnHit(eff, res);
            }
        }

        public override Effect GenerateCopy() {
            return Instantiate(this);
        }
    }

}