using System;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseShoes : BaseClothing
	{
		public BaseShoes( int itemID ) : this( itemID, 0 )
		{
		}

		public BaseShoes( int itemID, int hue ) : base( itemID, Layer.Shoes, hue )
		{
		}

		public BaseShoes( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2: break; // empty, resource removed
				case 1:
				{
					m_Resource = (CraftResource)reader.ReadInt();
					break;
				}
				case 0:
				{
					m_Resource = DefaultResource;
					break;
				}
			}
		}
	}

	[Flipable( 0x2307, 0x2308 )]
	public class FurBoots : BaseShoes
	{
		[Constructable]
		public FurBoots() : this( 0 )
		{
		}

		[Constructable]
		public FurBoots( int hue ) : base( 0x2307, hue )
		{
			Weight = 3.0;
		}

		public FurBoots( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[FlipableAttribute( 0x170b, 0x170c )]
	public class Boots : BaseShoes
	{
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		[Constructable]
		public Boots() : this( 0 )
		{
		}

		[Constructable]
		public Boots( int hue ) : base( 0x170B, hue )
		{
			Weight = 3.0;
		}

		public Boots( Serial serial ) : base( serial )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
            if (this.Name != null)
            {
                from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", this.Name));
            }
            else
            {
                if (Effect != ClothEffect.None)
                {
                    if (IsInIDList(from) || from.AccessLevel >= AccessLevel.GameMaster)
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", String.Format("boots {0} ({1} charges)", GetName(), Charges)));
                    else
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "magic boots"));
                }
                else
                {
                    if (Crafter != null)
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", String.Format("boots sewn by {0}", Crafter.Name)));
                    else
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "boots"));
                }
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable]
	public class ThighBoots : BaseShoes, IArcaneEquip
	{
		#region Arcane Impl
		private int m_MaxArcaneCharges, m_CurArcaneCharges;

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxArcaneCharges
		{
			get{ return m_MaxArcaneCharges; }
			set{ m_MaxArcaneCharges = value; InvalidateProperties(); Update(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int CurArcaneCharges
		{
			get{ return m_CurArcaneCharges; }
			set{ m_CurArcaneCharges = value; InvalidateProperties(); Update(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsArcane
		{
			get{ return ( m_MaxArcaneCharges > 0 && m_CurArcaneCharges >= 0 ); }
		}

		/*public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			if ( IsArcane )
				LabelTo( from, 1061837, String.Format( "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges ) );
		}*/

		public void Update()
		{
			if ( IsArcane )
				ItemID = 0x26AF;
			else if ( ItemID == 0x26AF )
				ItemID = 0x1711;

			if ( IsArcane && CurArcaneCharges == 0 )
				Hue = 0;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( IsArcane )
				list.Add( 1061837, "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges ); // arcane charges: ~1_val~ / ~2_val~
		}

		public void Flip()
		{
			if ( ItemID == 0x1711 )
				ItemID = 0x1712;
			else if ( ItemID == 0x1712 )
				ItemID = 0x1711;
		}
		#endregion

		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		[Constructable]
		public ThighBoots() : this( 0 )
		{
		}

		[Constructable]
		public ThighBoots( int hue ) : base( 0x1711, hue )
		{
			Weight = 4.0;
		}

		public ThighBoots( Serial serial ) : base( serial )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
            if (this.Name != null)
            {
                from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", this.Name));
            }
            else
            {
                if (Effect != ClothEffect.None)
                {
                    if (IsInIDList(from) || from.AccessLevel >= AccessLevel.GameMaster)
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", String.Format("thigh boots {0} ({1} charges)", GetName(), Charges)));
                    else
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "magic thigh boots"));
                }
                else
                {
                    if (Crafter != null)
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", String.Format("thigh boots sewn by {0}", Crafter.Name)));
                    else
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "thigh boots"));
                }
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			if ( IsArcane )
			{
				writer.Write( true );
				writer.Write( (int) m_CurArcaneCharges );
				writer.Write( (int) m_MaxArcaneCharges );
			}
			else
			{
				writer.Write( false );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					if ( reader.ReadBool() )
					{
						m_CurArcaneCharges = reader.ReadInt();
						m_MaxArcaneCharges = reader.ReadInt();

						if ( Hue == 2118 )
							Hue = ArcaneGem.DefaultArcaneHue;
					}

					break;
				}
			}
		}
	}

	[FlipableAttribute( 0x170f, 0x1710 )]
	public class Shoes : BaseShoes
	{
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		[Constructable]
		public Shoes() : this( 0 )
		{
		}

		[Constructable]
		public Shoes( int hue ) : base( 0x170F, hue )
		{
			Weight = 2.0;
		}

		public Shoes( Serial serial ) : base( serial )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
            if (this.Name != null)
            {
                from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", this.Name));
            }
            else
            {
                if (Effect != ClothEffect.None)
                {
                    if (IsInIDList(from) || from.AccessLevel >= AccessLevel.GameMaster)
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", String.Format("shoes {0} ({1} charges)", GetName(), Charges)));
                    else
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "magic shoes"));
                }
                else
                {
                    if (Crafter != null)
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", String.Format("shoes sewn by {0}", Crafter.Name)));
                    else
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "shoes"));
                }
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[FlipableAttribute( 0x170d, 0x170e )]
	public class Sandals : BaseShoes
	{
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		[Constructable]
		public Sandals() : this( 0 )
		{
		}

		[Constructable]
		public Sandals( int hue ) : base( 0x170D, hue )
		{
			Weight = 1.0;
		}

		public Sandals( Serial serial ) : base( serial )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
            if (this.Name != null)
            {
                from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", this.Name));
            }
            else
            {
                if (Effect != ClothEffect.None)
                {
                    if (IsInIDList(from) || from.AccessLevel >= AccessLevel.GameMaster)
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", String.Format("sandals {0} ({1} charges)", GetName(), Charges)));
                    else
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "magic sandals"));
                }
                else
                {
                    if (Crafter != null)
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", String.Format("sandals sewn by {0}", Crafter.Name)));
                    else
                        from.Send(new AsciiMessage(Serial, ItemID, MessageType.Label, 0, 3, "", "sandals"));
                }
            }
        }

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable( 0x2797, 0x27E2 )]
	public class NinjaTabi : BaseShoes
	{
		[Constructable]
		public NinjaTabi() : this( 0 )
		{
		}

		[Constructable]
		public NinjaTabi( int hue ) : base( 0x2797, hue )
		{
			Weight = 2.0;
		}

		public NinjaTabi( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable( 0x2796, 0x27E1 )]
	public class SamuraiTabi : BaseShoes
	{
		[Constructable]
		public SamuraiTabi() : this( 0 )
		{
		}

		[Constructable]
		public SamuraiTabi( int hue ) : base( 0x2796, hue )
		{
			Weight = 2.0;
		}

		public SamuraiTabi( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable( 0x2796, 0x27E1 )]
	public class Waraji : BaseShoes
	{
		[Constructable]
		public Waraji() : this( 0 )
		{
		}

		[Constructable]
		public Waraji( int hue ) : base( 0x2796, hue )
		{
			Weight = 2.0;
		}

		public Waraji( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[FlipableAttribute( 0x2FC4, 0x317A )]
	public class ElvenBoots : BaseShoes
	{
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public ElvenBoots() : this( 0 )
		{
		}

		[Constructable]
		public ElvenBoots( int hue ) : base( 0x2FC4, hue )
		{
			Weight = 2.0;
		}

		public ElvenBoots( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
