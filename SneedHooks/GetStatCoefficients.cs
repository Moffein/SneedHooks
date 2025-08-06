using R2API;
using RoR2;

namespace SneedHooks
{
    //Almost every item ends up having an inventory check in GetStatCoefficients, so this exists to just skip that step.
    public static class GetStatCoefficients
    {
        public delegate void HandleStatsInventory(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args, Inventory inventory);
        public static HandleStatsInventory HandleStatsInventoryActions;

        internal static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.inventory) HandleStatsInventoryActions?.Invoke(sender, args, sender.inventory);
        }
    }
}
