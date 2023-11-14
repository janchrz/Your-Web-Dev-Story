using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum ETransactionType
    {
        Buy,
        Sell
    }

    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Shops + nameof(Shop))]
    public class Shop : ScriptableObject
    {
        public Item[] items = null;
        public float sellingPriceMultiplier = 0.5f;
        public float buyingPriceMultiplier = 1.0f;

        public int GetPrice(Item item, ETransactionType transaction)
        {
            float floatPrice = 0.0f;

            switch (transaction)
            {
                case ETransactionType.Buy:
                    floatPrice = item.price * buyingPriceMultiplier;
                    break;

                case ETransactionType.Sell:
                    floatPrice = item.price * sellingPriceMultiplier;
                    break;
            }

            return (int)math.ceil(floatPrice);
        }
    }
}
