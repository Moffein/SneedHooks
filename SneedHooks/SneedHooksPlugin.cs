using BepInEx;
using System.Security.Permissions;
using System.Security;
using UnityEngine;
using System;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace SneedHooks
{
    [BepInPlugin("com.RiskyLives.SneedHooks", "SneedHooks", "1.0.0")]
    public class SneedHooksPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            On.RoR2.GlobalEventManager.ProcessHitEnemy += ProcessHitEnemy.GlobalEventManager_ProcessHitEnemy;
            IL.RoR2.HealthComponent.TakeDamageProcess += ModifyFinalDamage.HealthComponent_TakeDamageProcess;
        }
    }
}

namespace R2API.Utils
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ManualNetworkRegistrationAttribute : Attribute
    {
    }
}