using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Control {
    public class Status : MonoBehaviour {
        public Transform attackProjectile;
        Dictionary<BuffId, Buff> buffs = new Dictionary<BuffId, Buff>();

        public bool isPlayer = true;

        public float hpMax = 100;
        float _hpCur;
        public float hpCur {
            get {
                return _hpCur;
            }
            set {
                _hpCur = Mathf.Max(0, value);
                if(isPlayer) GUIManager.SetHPFill(_hpCur / hpMax);
            }
        }
        public int _ammo = 3;
        public int ammo {
            get {
                return _ammo;
            }
            set {
                _ammo = value;
                if (isPlayer) GUIManager.SetSprayCharge(_ammo);
            }
        }


        float baseMoveSpeed;
        public float moveSpeed = 6;
        float baseRunMultiplier;
        public float runMultiplier = 1.5f;
        float baseJumpMultiplier;
        public float jumpMultiplier = 1;
        float baseAttackRange;
        public float attackRange = 2;
        

        public void Start() {
            SaveAsBase();
            hpCur = hpMax;
        }

        public void SaveAsBase() {
            baseMoveSpeed = moveSpeed;
            baseJumpMultiplier = jumpMultiplier;
            baseRunMultiplier = runMultiplier;
            baseAttackRange = attackRange;
        }

        public void Reset() {
            moveSpeed = baseMoveSpeed;
            jumpMultiplier = baseJumpMultiplier;
            runMultiplier = baseRunMultiplier;
            attackRange = baseAttackRange;
        }
        

        public void AddBuff(Buff buff) {
            buff.SetStatus(this);
            if (buffs.ContainsKey(buff.id)) {
                buffs[buff.id].Stack(buff);
            } else {
                buffs[buff.id] = buff;
                buff.Start();
            }
        }
        

        public void RemoveBuff(BuffId buffId) {
            if (!buffs.ContainsKey(buffId)) {
                return;
            }
            Buff buff = GetBuff(buffId);
            if (!buff.End()) {
                return;
            }
            buffs.Remove(buffId);
        }

        public void ClearBuff() {
            foreach (Buff buff in buffs.Values) {
                buff.End();
            }
            buffs.Clear();
        }

        public void UpdateBuffs(float dt) {
            List<Buff> toRemove = new List<Buff>();
            foreach (Buff buff in buffs.Values) {
                buff.Update(dt);
                if (buff.shouldBeRemoved) {
                    toRemove.Add(buff);
                }
            }
            foreach (Buff buff in toRemove) {
                RemoveBuff(buff.id);
            }
        }

        public bool HasBuff(BuffId buffId) {
            return buffs.ContainsKey(buffId);
        }

        private Buff GetBuff(BuffId buffId) {
            if (buffs.ContainsKey(buffId)) {
                return buffs[buffId];
            } else {
                return null;
            }
        }
        public int CheckStack(BuffId buffId, int arg=0) {
            if (!HasBuff(buffId)) return 0;
            return GetBuff(buffId).CheckStack(arg);
        }
        public bool TakeStack(BuffId buffId, int stack) {
            if (!HasBuff(buffId)) return false;
            return GetBuff(buffId).TakeStack(stack);
        }

        public void Damage(float damage) {
            hpCur -= damage;
        }
    }
}