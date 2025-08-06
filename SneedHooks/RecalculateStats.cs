using RoR2;

namespace SneedHooks
{
    public static class RecalculateStats
    {
        public delegate void RecalculateStatsInventory(CharacterBody self, Inventory inventory);
        public static RecalculateStatsInventory RecalculateStatsInventoryActions;

        internal static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self.inventory) RecalculateStatsInventoryActions?.Invoke(self, self.inventory);
        }
    }
}
