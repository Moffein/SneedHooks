using MonoMod.Cil;
using System;
using RoR2;
using UnityEngine;
using Mono.Cecil.Cil;
using R2API;
using static SneedHooks.ModifyFinalDamage;

namespace SneedHooks
{
    public static class ModifyFinalDamage
    {
        public class DamageModifierArgs
        {
            /// <summary>Additive damage multiplier. Only modify this with addition. Full Formula: (1 + damageMultAdd) * damageMultFinal / (1f + damageReductionFactorAdd)</summary>
            public float damageMultAdd = 0f;

            /// <summary>Multiplicative damage multiplier. Only modify this with multiplication and division. Full Formula: (1 + damageMultAdd) * damageMultFinal / (1f + damageReductionFactorAdd)</summary>
            public float damageMultFinal = 1f;

            /// <summary>Damage reduction multiplier. Only modify this with addition. Full Formula: (1 + damageMultAdd) * damageMultFinal / (1f + damageReductionFactorAdd)</summary>
            public float damageReductionFactorAdd = 0f;

            public float flatDamageAdd = 0f;
        }

        public delegate void ModifyFinalDamageDelegate(DamageModifierArgs damageModifierArgs, DamageInfo damageInfo,
            HealthComponent victim, CharacterBody victimBody);
        public static ModifyFinalDamageDelegate ModifyFinalDamageActions;

        public delegate void ModifyFinalDamageAttackerDelegate(DamageModifierArgs damageModifierArgs, DamageInfo damageInfo,
            HealthComponent victim, CharacterBody victimBody, CharacterBody attackerBody);
        public static ModifyFinalDamageAttackerDelegate ModifyFinalDamageAttackerActions;

        internal static void HealthComponent_TakeDamageProcess(MonoMod.Cil.ILContext il)
        {
            bool matched = false;
            ILCursor c = new ILCursor(il);
            var targetIndex = 0;
            int ignoreIndex = 0;
            if (c.TryGotoNext(
                x => x.MatchLdloc(out targetIndex),
                x => x.MatchLdloc(out ignoreIndex),
                x => x.MatchLdfld<TeamDef>("friendlyFireScaling")
            ))
            {
                c = new ILCursor(il);
                if (c.TryGotoNext(
                     x => x.MatchStloc(targetIndex)
                    ))
                {
                    c.Emit(OpCodes.Ldarg_0);    //self
                    c.Emit(OpCodes.Ldarg_1);    //damageInfo
                    c.EmitDelegate<Func<float, HealthComponent, DamageInfo, float>>((origDamage, victimHealth, damageInfo) =>
                    {

                        float newDamage = origDamage;
                        CharacterBody victimBody = victimHealth.body;
                        if (victimBody)
                        {
                            DamageModifierArgs damageModifierArgs = new DamageModifierArgs();
                            ModifyFinalDamageActions?.Invoke(damageModifierArgs, damageInfo, victimHealth, victimBody);

                            if (damageInfo.attacker)
                            {
                                CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                                if (attackerBody)
                                {
                                    ModifyFinalDamageAttackerActions?.Invoke(damageModifierArgs, damageInfo, victimHealth, victimBody, attackerBody);
                                }
                            }

                            newDamage *= (1f + damageModifierArgs.damageMultAdd) * damageModifierArgs.damageMultFinal / (1f + damageModifierArgs.damageReductionFactorAdd);
                            newDamage += damageModifierArgs.flatDamageAdd;
                        }
                        return newDamage;
                    });
                    matched = true;
                }
            }

            
            if (!matched)
            {
                UnityEngine.Debug.LogError("SneedHooks: ModifyFinalDamage IL Hook failed. This will break a lot of things.");
            }
        }
    }
}
