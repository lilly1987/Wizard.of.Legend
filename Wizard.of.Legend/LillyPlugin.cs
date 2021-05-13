using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wizard.of.Legend
{

    [BepInPlugin("Lilly", "Lilly", "1.0.0.0")]
    public class LillyPlugin : BaseUnityPlugin
    {
        void Awake()
        {
            UnityEngine.Debug.Log("Hello, world! Lilly");
            Harmony.CreateAndPatchAll(typeof(LillyPlugin));
        }

        //[HarmonyPatch(typeof(PlatWallet), "Withdraw")]
        //[HarmonyPatch(typeof(GoldWallet), "Withdraw")]
        [HarmonyPatch(typeof(Wallet), "Withdraw")]
        [HarmonyPrefix]
        // public virtual bool Withdraw(int withdrawAmount)
        static void Withdraw(Wallet __instance, int withdrawAmount)
        {
            UnityEngine.Debug.Log("Withdraw maxBalance : " + __instance.maxBalance);
            UnityEngine.Debug.Log("Withdraw balance : " + __instance.balance);
            UnityEngine.Debug.Log("Withdraw withdrawAmount : " + withdrawAmount);
            __instance.balance += withdrawAmount;
        }

        [HarmonyPatch(typeof(Wallet), "Deposit")]
        [HarmonyPrefix]
        // public virtual bool Deposit(int depositAmount)
        static void Deposit(Wallet __instance, int depositAmount)
        {
            UnityEngine.Debug.Log("Withdraw depositAmount : " + depositAmount);
        }
        
        [HarmonyPatch(typeof(Wallet), MethodType.Constructor, new Type[] { typeof(int), typeof(int) })]
        [HarmonyPostfix]
        // public Wallet(int newBalance, int newMaxBalance)
        static void WalletCtor(Wallet __instance)
        {
            UnityEngine.Debug.Log("Withdraw Constructor : " + __instance.balance);
            UnityEngine.Debug.Log("Withdraw Constructor : " + __instance.maxBalance);
            __instance.balance= __instance.maxBalance;
        }

        /// <summary>
        /// 적과 공유함
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="givenID"></param>
        /// <param name="givenMaxTime"></param>
        /// <param name="givenData"></param>
        /// <param name="givenSkill"></param>
        [HarmonyPatch(typeof(Cooldown), MethodType.Constructor,new Type[] { typeof(string), typeof(float) , typeof(StatData) , typeof(Player.SkillState) })]
        [HarmonyPrefix]
        //public Cooldown(string givenID, float givenMaxTime, StatData givenData = null, Player.SkillState givenSkill = null)
        static void CooldownCtor(Cooldown __instance, string givenID,ref float givenMaxTime, StatData givenData = null, Player.SkillState givenSkill = null)
        {
            UnityEngine.Debug.Log("Cooldown CooldownCtor : " + givenID);
            UnityEngine.Debug.Log("Cooldown CooldownCtor : " + givenMaxTime);
            givenMaxTime = 0f;
        }
        
        /// <summary>
        /// 적과 공유함
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        [HarmonyPatch(typeof(Cooldown), "MaxTime",MethodType.Getter)]
        [HarmonyPostfix]
        //public Cooldown(string givenID, float givenMaxTime, StatData givenData = null, Player.SkillState givenSkill = null)
        static void MaxTime(Cooldown __instance,ref float __result)
        {
            UnityEngine.Debug.Log("Cooldown MaxTime "+ __result);
            __result = 0f;
            //return false;
        }
        

        [HarmonyPatch(typeof(Cooldown), "RemainingDelayTime", MethodType.Getter)]
        [HarmonyPostfix]
        //public Cooldown(string givenID, float givenMaxTime, StatData givenData = null, Player.SkillState givenSkill = null)
        static void RemainingDelayTime(Cooldown __instance, float __result)
        {
            UnityEngine.Debug.Log("Cooldown RemainingDelayTime "+ __result);
            __result = 0;
           // return false;
        }
        

        [HarmonyPatch(typeof(Cooldown), "RemainingTime", MethodType.Getter)]
        [HarmonyPostfix]
        //public Cooldown(string givenID, float givenMaxTime, StatData givenData = null, Player.SkillState givenSkill = null)
        static void RemainingTime(Cooldown __instance,ref float __result)
        {
            UnityEngine.Debug.Log("Cooldown RemainingTime " + __result);
            __result = 0;
            //return false;
        }
        
        /// <summary>
        /// 의미 없음. 숫자 확인용
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        //[HarmonyPatch(typeof(Cooldown), "TimePercentage", MethodType.Getter)]
        //[HarmonyPostfix]
        //public Cooldown(string givenID, float givenMaxTime, StatData givenData = null, Player.SkillState givenSkill = null)
        static void TimePercentage(Cooldown __instance, float __result)
        {
            UnityEngine.Debug.Log("Cooldown TimePercentage " + __result);
            __result = 0;
            //return false;
        }
        
        /// <summary>
        /// 오히려 0이면 안될거 같음
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        //[HarmonyPatch(typeof(Cooldown), "TotalChargePercentage", MethodType.Getter)]
        //[HarmonyPostfix]
        //public Cooldown(string givenID, float givenMaxTime, StatData givenData = null, Player.SkillState givenSkill = null)
        static void TotalChargePercentage(Cooldown __instance, float __result)
        {
            UnityEngine.Debug.Log("Cooldown TotalChargePercentage " + __result);
            __result = 0;
            //return false;
        }
        
        /*
        [HarmonyPatch(typeof(Player.SkillState), MethodType.Constructor)]
        [HarmonyPrefix]
        // public SkillState(string newName, FSM newFSM, Player newEnt) 
        static void SkillStateCtor(Player.SkillState __instance
            , string newName, FSM newFSM, Player newEnt)
        {
            UnityEngine.Debug.Log("Player.SkillState SkillStateCtor : " + newName);
            
        }
        */

        /// <summary>
        /// 추락 데미지
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(Player.PlayerFallState), "TakeFallDamage")]
        [HarmonyPrefix]
        // public bool TakeFallDamage()
        static bool TakeFallDamage(
            Player.PlayerFallState __instance
            ,bool __result
            , AttackInfo ___fallAtkInfo)
        {
            UnityEngine.Debug.Log("PlayerFallState TakeFallDamage CurrentShieldValue : " + __instance.parent.health.healthStat._baseValue);
            UnityEngine.Debug.Log("PlayerFallState TakeFallDamage CurrentHealthValue : " + __instance.parent.health.healthStat._currentValue);
            UnityEngine.Debug.Log("PlayerFallState TakeFallDamage CurrentShieldValue : " + __instance.parent.health.healthStat._modifiedValue);
            UnityEngine.Debug.Log("PlayerFallState TakeFallDamage CurrentValue : " + __instance.parent.FallDamage.CurrentValue);
            __instance.parent.health.healthStat._currentValue = __instance.parent.health.healthStat._baseValue;

            __instance.parent.FallDamage.CurrentValue = 0f;
            //__result = true;
            return true;
            return false;
        }
        /// <summary>
        /// fail
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(Player.SkillState), "InitChargeSkillSettings")]
        [HarmonyPrefix]
        // public void InitChargeSkillSettings(int maxCharges, float delayBetweenCharges, StatData statData, Player.SkillState skillState)
        static bool InitChargeSkillSettings(
            Player.SkillState __instance
            //,bool __result
            , int maxCharges
            , float delayBetweenCharges
            , StatData statData
            , Player.SkillState skillState
            )
        {
            UnityEngine.Debug.Log("Player.SkillState InitChargeSkillSettings  : "+ maxCharges);
            UnityEngine.Debug.Log("Player.SkillState InitChargeSkillSettings  : "+ delayBetweenCharges);


            //__result = true;
            return true;
            return false;
        }


        /// <summary>
        /// 적 공격에 반응 없음
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(Player.SkillState), "RegisterSkill")]
        [HarmonyPostfix]
        // private void HandleHealthDegen()
        static void RegisterSkill(
            Player.SkillState __instance
            )
        {
            UnityEngine.Debug.Log("Player.SkillState RegisterSkill  : " + __instance.skillID);
            
            /*
            __instance.parent.skillsDict[__instance.skillID] = __instance;
            __instance.parent.cooldownManager.Add(__instance.skillID, __instance.skillData.GetValue<float>("cooldown", __instance.currentLevel), __instance.skillData, __instance);
            __instance.cooldownRef = __instance.parent.cooldownManager.cooldowns[__instance.skillID];
            */
        }

        /// <summary>
        /// new Cooldown 를 호출해서 의미 없음
        /// 적과 공유함
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        //[HarmonyPatch(typeof(CooldownManager), "Add")]
        //[HarmonyPrefix]
        // public void Add(string givenID, float givenTime, StatData givenData = null, Player.SkillState givenSS = null)
        static bool Add(
            CooldownManager __instance            
            )
        {
            UnityEngine.Debug.Log("CooldownManager Add  : ");

            return true;
            return false;
        }


        
        /// <summary>
        /// new Cooldown 를 호출해서 의미 없음
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(CooldownManager), "GetRemainingTime")]
        [HarmonyPrefix]
        // public float GetRemainingTime(string cooldownID)
        static bool GetRemainingTime(
            CooldownManager __instance
            , float __result
            , string cooldownID
            )
        {
            UnityEngine.Debug.Log("CooldownManager GetRemainingTime  : " + cooldownID);
            __result = 0f;
            return false;
            return true;
        }

        [HarmonyPatch(typeof(Player), "ResetPlayer", new Type[] { typeof(bool) ,typeof(bool)  ,typeof(bool)  ,typeof(bool)  ,typeof(float) })]
        [HarmonyPrefix]
        // public void ResetPlayer(bool originPosition = true, bool fullyRestoreHealth = false, bool resetState = true, bool resetOverdrive = true, float restoreHealthPercentage = 0f)
        static bool ResetPlayer(
            Player __instance
            , bool originPosition 
            , bool fullyRestoreHealth 
            , bool resetState 
            , bool resetOverdrive 
            , float restoreHealthPercentage )
        {
            UnityEngine.Debug.Log("Player ResetPlayer BaseValue : " + __instance.health.healthStat.BaseValue);
            UnityEngine.Debug.Log("Player ResetPlayer CurrentValue : " + __instance.health.healthStat.CurrentValue);
            UnityEngine.Debug.Log("Player ResetPlayer ModifiedValue : " + __instance.health.healthStat.ModifiedValue);

            __instance.health.healthStat.BaseValue = 99999;
            __instance.health.healthStat.CurrentValue = 99999;
            __instance.health.healthStat.ModifiedValue = 99999;

            //__result = true;
            return true;
            return false;
        }

        /// <summary>
        /// 적 공격에 반응 없음
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(Player), "HandleHealthDegen")]
        [HarmonyPrefix]
        // private void HandleHealthDegen()
        static bool HandleHealthDegen(
            Player __instance
            )
        {
            if (__instance.degenHealth.CurrentValue)
            {
                UnityEngine.Debug.Log("Player HandleHealthDegen CurrentValue : " + __instance.degenHealth.CurrentValue);
                __instance.degenHealth._currentValue = false;
            }

            //__result = true;
            return true;
            return false;
        }


        /// <summary>
        /// 
        /// 적 공격에 반응 없음
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(Health), "TakeDamage",new Type[] { typeof(AttackInfo) , typeof(Entity) })]
        [HarmonyPrefix]
        // public virtual bool TakeDamage(AttackInfo givenAttackInfo, Entity attackEntity = null)
        static bool TakeDamage(
            Health __instance
            , AttackInfo givenAttackInfo
            , Entity attackEntity 
            )
        {
            if (attackEntity!=null)
            {
                UnityEngine.Debug.Log("Health TakeDamage CurrentValue : "+ attackEntity.currentStateName);
                if (attackEntity.currentStateName == "Attack")
                    givenAttackInfo.damage = 0;
            }
            if (givenAttackInfo != null)
            {
                UnityEngine.Debug.Log("Health TakeDamage CurrentValue : "+ givenAttackInfo.damage);
                if (givenAttackInfo.entity != null)
                {
                    UnityEngine.Debug.Log("Health TakeDamage CurrentValue : " + givenAttackInfo.entity.currentStateName);
                    if (givenAttackInfo.entity.currentStateName == "Attack")
                        givenAttackInfo.damage = 0;
                }
            }

            return true;
            return false;
        }
    }
}
