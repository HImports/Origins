using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBChainmailArmor: SBInfo
	{
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBChainmailArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( ChainCoif ), 132, 9, 0x13BB, 0 ) );
				Add( new GenericBuyInfo( typeof( ChainChest ), 205, 9, 0x13BF, 0 ) );
				Add( new GenericBuyInfo( typeof( ChainLegs ), 168, 9, 0x13BE, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( ChainCoif ), 66 );
				Add( typeof( ChainChest ), 103 );
				Add( typeof( ChainLegs ), 84 );
			}
		}
	}
}
