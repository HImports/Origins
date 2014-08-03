using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public abstract class MagerySpell : Spell
	{
		public MagerySpell( Mobile caster, Item scroll, SpellInfo info )
			: base( caster, scroll, info )
		{
		}

		public abstract SpellCircle Circle { get; }

		public override bool ConsumeReagents()
		{
			if( base.ConsumeReagents() )
				return true;

			if( ArcaneGem.ConsumeCharges( Caster, (Core.SE ? 1 : 1 + (int)Circle) ) )
				return true;

			return false;
		}

		private const double ChanceOffset = 20.0, ChanceLength = 100.0 / 7.0;

		public override void GetCastSkills( out double min, out double max )
		{
			int circle = (int)Circle;

			if( Scroll != null )
				circle -= 2;

			double avg = ChanceLength * circle;

			min = avg - ChanceOffset;
			max = avg + ChanceOffset;
		}

		private static int[] m_ManaTable = new int[] { 4, 6, 9, 11, 14, 20, 40, 50 };

		public override int GetMana()
		{
			if (Scroll != null && ( Scroll is BaseWand || Scroll is BaseJewel || Scroll is GnarledStaff || Scroll is BaseStationary ))
				return 0;

            if ( Scroll != null )
            {
                return m_ManaTable[(int)Circle] / 2;
            }
            else
            {
                return m_ManaTable[(int)Circle];
            }
		}

		public override double GetResistSkill( Mobile m )
		{
			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if( m.Skills[SkillName.MagicResist].Value < maxSkill )
				m.CheckSkill( SkillName.MagicResist, 0.0, 100.0 );

			return m.Skills[SkillName.MagicResist].Value;
		}

        public virtual bool CheckResisted(Mobile target, double damage)
        {          
            bool canattack = false;
            int noto = Notoriety.Compute( Caster, target );

            if (noto == Notoriety.Enemy)
                canattack = true;
            else if (((Caster.FindItemOnLayer(Layer.TwoHanded) is OrderShield) && (target.FindItemOnLayer(Layer.TwoHanded) is ChaosShield)) && target is PlayerMobile)
                canattack = true;
            else if (((Caster.FindItemOnLayer(Layer.TwoHanded) is ChaosShield) && (target.FindItemOnLayer(Layer.TwoHanded) is OrderShield)) && target is PlayerMobile)
                canattack = true;

            if (SpellHelper.IsTown(target.Location, Caster) && !canattack)
                damage = 1;

            double sk = damage * 2.5;
            if (sk > 124.9)
                sk = 124.9;
            return target.CheckSkill(SkillName.MagicResist, sk - 25.0, sk + 25.0);
        }

		public virtual bool CheckResisted( Mobile target )
		{
			double n = GetResistPercent( target );

			n /= 100.0;

			if( n <= 0.0 )
				return false;

			if( n >= 1.0 )
				return true;

			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if( target.Skills[SkillName.MagicResist].Value < maxSkill )
				target.CheckSkill( SkillName.MagicResist, 0.0, 100.0 );

			return (n >= Utility.RandomDouble());
		}

		public virtual double GetResistPercentForCircle( Mobile target, SpellCircle circle )
		{
			double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
			double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
		}

		public virtual double GetResistPercent( Mobile target )
		{
			return GetResistPercentForCircle( target, Circle );
		}

		public override TimeSpan GetCastDelay()
		{
            if (Scroll is BaseWand || Scroll is BaseJewel || Scroll is GnarledStaff || Scroll is BaseStationary)
				return TimeSpan.Zero;

			if( !Core.AOS )
				return TimeSpan.FromSeconds( 0.5 + (0.25 * (int)Circle) );

			return base.GetCastDelay();
		}

		public override TimeSpan CastDelayBase
		{
			get
			{
				return TimeSpan.FromSeconds( (3 + (int)Circle) * CastDelaySecondsPerTick );
			}
		}
	}
}
