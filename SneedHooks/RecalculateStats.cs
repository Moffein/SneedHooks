using RoR2;

namespace SneedHooks
{
    public static class RecalculateStats
    {
        public delegate void HandleRecalculateStatsInventory(CharacterBody self, Inventory inventory);
        public static HandleRecalculateStatsInventory HandleRecalculateStatsInventoryActions;

        internal static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self.inventory) HandleRecalculateStatsInventoryActions?.Invoke(self, self.inventory);
        }
    }
}
