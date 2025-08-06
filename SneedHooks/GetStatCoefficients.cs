using R2API;
using RoR2;

namespace SneedHooks
{
    //Almost every item ends up having an inventory check in GetStatCoefficients, so this exists to just skip that step.
    public static class GetStatCoefficients
    {
        public delegate void GetStatInventory(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args, Inventory inventory);
        public static GetStatInventory GetStatInventoryActions;

        internal static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.inventory) GetStatInventoryActions?.Invoke(sender, args, sender.inventory);
        }
    }
}
