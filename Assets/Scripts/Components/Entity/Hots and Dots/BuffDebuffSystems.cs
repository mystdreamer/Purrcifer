using NUnit.Framework;
using System.Numerics;
using UnityEngine;

namespace Purrcifer.Entity.HotsDots
{
    public abstract class Buff
    {
        public float timeToBuff;
        public float tickEveryX;
        public float valuePerTick;
        internal float currentTime;
        internal float passedTime;

        public bool Completed => (currentTime <= 0);

        public Buff(float timeToBuff, float tickEveryX, float healPerTick)
        {
            this.timeToBuff = timeToBuff;
            this.tickEveryX = tickEveryX;
            this.valuePerTick = healPerTick;
            currentTime = timeToBuff;
            passedTime = 0;
        }

        private bool UpdateTimes(float dt)
        {
            currentTime -= dt;
            passedTime += dt;
            return (passedTime >= tickEveryX);
        }

        public bool Update(float dt, ref EntityHealth health, out bool complete)
        {
            bool hasTicked = UpdateTimes(dt);
            if (hasTicked)
            {
                passedTime = 0;
                Apply(ref health);
            }
            complete = (currentTime <= 0);
            return hasTicked;
        }

        public bool Update(float dt, ref BossHealth health, out bool complete)
        {
            bool hasTicked = UpdateTimes(dt);
            if (hasTicked)
            {
                passedTime = 0;
                Apply(ref health);
            }
            complete = (currentTime <= 0);
            return hasTicked;
        }

        internal abstract void Apply(ref EntityHealth health);

        internal abstract void Apply(ref BossHealth health);
    }

    public class HealOverTime : Buff
    {

        public HealOverTime(float timeToBuff, float tickEveryX, float healPerTick) : base(timeToBuff, tickEveryX, healPerTick)
        {
        }

        internal override void Apply(ref EntityHealth health)
        {
            health.Health += valuePerTick;
        }

        internal override void Apply(ref BossHealth health)
        {
            health.Health += valuePerTick;
        }
    }

    public class DamageOverTime : Buff
    {

        public DamageOverTime(float timeToBuff, float tickEveryX, float damagePerTick) : base(timeToBuff, tickEveryX, damagePerTick)
        {
        }

        internal override void Apply(ref EntityHealth health)
        {
            health.Health -= valuePerTick;
        }

        internal override void Apply(ref BossHealth health)
        {
            health.Health -= valuePerTick;
        }
    }
}


