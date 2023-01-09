using System;

namespace battleTest
{
    enum StatusConditionType
    {
        독 = 0b0001,
        화상 = 0b0010,
        마비 = 0b0100,
        얼음 = 0b1000
    }

    internal class StatusCondition
    {
        string name;
        StatusConditionType type;

        public virtual void EffectBeforeSkill()
        {

        }

        public virtual void EffectEndOfTheTurn()
        {

        }
    }
}