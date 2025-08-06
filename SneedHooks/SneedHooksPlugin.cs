using BepInEx;
using System.Security.Permissions;
using System.Security;
using R2API.Utils;
using R2API;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace SneedHooks
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(R2API.RecalculateStatsAPI.PluginGUID)]
    [BepInPlugin("com.RiskyLives.SneedHooks", "SneedHooks", "1.0.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class SneedHooksPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            On.RoR2.GlobalEventManager.ProcessHitEnemy += ProcessHitEnemy.GlobalEventManager_ProcessHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats += RecalculateStats.CharacterBody_RecalculateStats;
            RecalculateStatsAPI.GetStatCoefficients += GetStatCoefficients.RecalculateStatsAPI_GetStatCoefficients;
            IL.RoR2.HealthComponent.TakeDamageProcess += ModifyFinalDamage.HealthComponent_TakeDamageProcess;
        }
    }
}