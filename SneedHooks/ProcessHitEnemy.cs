using RoR2;
using UnityEngine.Networking;

namespace SneedHooks
{
    public static class ProcessHitEnemy
    {
        public delegate void OnHit(DamageInfo damageInfo, CharacterBody victimBody);
        public static OnHit OnHitActions;
        public static OnHit PreOnHitActions;

        public delegate void OnHitAttacker(DamageInfo damageInfo, CharacterBody victimBody, CharacterBody attackerBody);
        public static OnHitAttacker OnHitAttackerActions;
        public static OnHitAttacker PreOnHitAttackerActions;

        public delegate void OnHitAttackerInventory(DamageInfo damageInfo, CharacterBody victimBody, CharacterBody attackerBody, Inventory attackerInventory);
        public static OnHitAttackerInventory OnHitAttackerInventoryActions;
        public static OnHitAttackerInventory PreOnHitAttackerInventoryActions;

        internal static void GlobalEventManager_ProcessHitEnemy(On.RoR2.GlobalEventManager.orig_ProcessHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, UnityEngine.GameObject victim)
        {
            CharacterBody attackerBody = null;
            CharacterBody victimBody = null;
            Inventory attackerInventory = null;

            bool validDamage = NetworkServer.active && victim && damageInfo.procCoefficient > 0f && !damageInfo.rejected;

            if (validDamage)
            {
                victimBody = victim.GetComponent<CharacterBody>();
                if (victimBody)
                {
                    PreOnHitActions?.Invoke(damageInfo, victimBody);

                    if (damageInfo.attacker) attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if (attackerBody)
                    {
                        PreOnHitAttackerActions?.Invoke(damageInfo, victimBody, attackerBody);

                        attackerInventory = attackerBody.inventory;
                        if (attackerInventory) PreOnHitAttackerInventoryActions.Invoke(damageInfo, victimBody, attackerBody, attackerInventory);
                    }
                }
                else
                {
                    validDamage = false;
                }
            }

            orig(self, damageInfo, victim);

            if (validDamage)
            {
                OnHitActions?.Invoke(damageInfo, victimBody);
                if (attackerBody)
                {
                    OnHitAttackerActions?.Invoke(damageInfo, victimBody, attackerBody);
                    if (attackerInventory) OnHitAttackerInventoryActions?.Invoke(damageInfo, victimBody, attackerBody, attackerInventory);
                }
            }
        }
    }
}
