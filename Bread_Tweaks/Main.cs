using System.Collections;
using HarmonyLib;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Employees;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Trash;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;

namespace Bread_Tweaks;

public class Main : MelonMod
{
    private static readonly string PreferencePath = Path.Combine(MelonEnvironment.UserDataDirectory, "Bread_Tweaks");

    private static MelonPreferences_Category? _employeeCategory;
    private static MelonPreferences_Entry<bool>? _employeePatchEnabled;

    private static MelonPreferences_Category? _botanistCategory;
    private static MelonPreferences_Entry<double>? _botanistAdditivePourTime;
    private static MelonPreferences_Entry<double>? _botanistHarvestTime;
    private static MelonPreferences_Entry<double>? _botanistSeedSowTime;
    private static MelonPreferences_Entry<double>? _botanistSoilPourTime;
    private static MelonPreferences_Entry<double>? _botanistWaterPourTime;
    private static MelonPreferences_Entry<int>? _botanistMaxPots;
    private static MelonPreferences_Entry<bool>? _botanistSigningFeeEnabled;
    private static MelonPreferences_Entry<double>? _botanistSigningFee;
    private static MelonPreferences_Entry<double>? _botanistDailyWage;
    private static MelonPreferences_Entry<double>? _botanistMoveSpeedMultiplier;

    private static MelonPreferences_Category? _chemistCategory;
    private static MelonPreferences_Entry<int>? _chemistMaxStations;
    private static MelonPreferences_Entry<bool>? _chemistSigningFeeEnabled;
    private static MelonPreferences_Entry<double>? _chemistSigningFee;
    private static MelonPreferences_Entry<double>? _chemistDailyWage;
    private static MelonPreferences_Entry<double>? _chemistMoveSpeedMultiplier;

    private static MelonPreferences_Category? _cleanerCategory;
    private static MelonPreferences_Entry<int>? _cleanerMaxBins;
    private static MelonPreferences_Entry<bool>? _cleanerSigningFeeEnabled;
    private static MelonPreferences_Entry<double>? _cleanerSigningFee;
    private static MelonPreferences_Entry<double>? _cleanerDailyWage;
    private static MelonPreferences_Entry<double>? _cleanerMoveSpeedMultiplier;

    private static MelonPreferences_Category? _handlerCategory;
    private static MelonPreferences_Entry<int>? _handlerMaxStations;
    private static MelonPreferences_Entry<int>? _handlerMaxRoutes;
    private static MelonPreferences_Entry<double>? _handlerPackagingSpeedMultiplier;
    private static MelonPreferences_Entry<bool>? _handlerSigningFeeEnabled;
    private static MelonPreferences_Entry<double>? _handlerSigningFee;
    private static MelonPreferences_Entry<double>? _handlerDailyWage;
    private static MelonPreferences_Entry<double>? _handlerMoveSpeedMultiplier;

    private static MelonPreferences_Category? _dealerCategory;
    private static MelonPreferences_Entry<bool>? _dealerPatchEnabled;
    private static MelonPreferences_Entry<bool>? _dealerAutoCollectEnabled;
    private static MelonPreferences_Entry<double>? _dealerMoveSpeedMultiplier;
    private static MelonPreferences_Entry<double>? _dealerCut;

    private static MelonPreferences_Category? _qolCategory;
    private static MelonPreferences_Entry<bool>? _trashSmallerEnabled;
    private static MelonPreferences_Entry<double>? _trashSmallerScale;
    private static MelonPreferences_Entry<bool>? _trashBagsOnly;
    private static MelonPreferences_Entry<double>? _trashScaleTime;

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(PreferencePath);
        // ----

        _employeeCategory = MelonPreferences.CreateCategory("Employee Patches");
        _employeeCategory.SetFilePath(Path.Combine(PreferencePath, "Employees.cfg"));

        _employeePatchEnabled = _employeeCategory.CreateEntry("Enabled", true);

        _botanistCategory = MelonPreferences.CreateCategory("Botanist");
        _botanistCategory.SetFilePath(Path.Combine(PreferencePath, "Employees.cfg"));
        _botanistAdditivePourTime = _botanistCategory.CreateEntry("Additive Pour Time", 10.0);
        _botanistDailyWage = _botanistCategory.CreateEntry("Daily Wage", 200.0);
        _botanistHarvestTime = _botanistCategory.CreateEntry("Harvest Time", 15.0);
        _botanistMaxPots = _botanistCategory.CreateEntry("Max Pots", 8);
        _botanistMoveSpeedMultiplier = _botanistCategory.CreateEntry("Move Speed Multiplier", 1.0);
        _botanistSeedSowTime = _botanistCategory.CreateEntry("Seed Sow Time", 15.0);
        _botanistSigningFee = _botanistCategory.CreateEntry("Signing Fee", 1000.0);
        _botanistSigningFeeEnabled = _botanistCategory.CreateEntry("Custom Signing Fee", false,
            description: "The Custom Signing Bonus applies a flat fee.");
        _botanistSoilPourTime = _botanistCategory.CreateEntry("Soil Pour Time", 10.0);
        _botanistWaterPourTime = _botanistCategory.CreateEntry("Water Pour Time", 1.0);

        _chemistCategory = MelonPreferences.CreateCategory("Chemist");
        _chemistCategory.SetFilePath(Path.Combine(PreferencePath, "Employees.cfg"));
        _chemistDailyWage = _chemistCategory.CreateEntry("Daily Wage", 200.0);
        _chemistMaxStations = _chemistCategory.CreateEntry("Max Stations", 4);
        _chemistMoveSpeedMultiplier = _chemistCategory.CreateEntry("Move Speed Multiplier", 1.0);
        _chemistSigningFee = _chemistCategory.CreateEntry("Signing Fee", 1000.0);
        _chemistSigningFeeEnabled = _chemistCategory.CreateEntry("Custom Signing Fee", false,
            description: "The Custom Signing Bonus applies a flat fee.");


        _cleanerCategory = MelonPreferences.CreateCategory("Cleaner");
        _cleanerCategory.SetFilePath(Path.Combine(PreferencePath, "Employees.cfg"));
        _cleanerDailyWage = _cleanerCategory.CreateEntry("Daily Wage", 200.0);
        _cleanerMaxBins = _cleanerCategory.CreateEntry("Max Bins", 3);
        _cleanerMoveSpeedMultiplier = _cleanerCategory.CreateEntry("Move Speed Multiplier", 1.0);
        _cleanerSigningFee = _cleanerCategory.CreateEntry("Signing Fee", 1000.0);
        _cleanerSigningFeeEnabled = _cleanerCategory.CreateEntry("Custom Signing Fee", false,
            description: "The Custom Signing Bonus applies a flat fee.");


        _handlerCategory = MelonPreferences.CreateCategory("Handler");
        _handlerCategory.SetFilePath(Path.Combine(PreferencePath, "Employees.cfg"));
        _handlerDailyWage = _handlerCategory.CreateEntry("Daily Wage", 200.0);
        _handlerMaxRoutes = _handlerCategory.CreateEntry("Max Routes", 5);
        _handlerMaxStations = _handlerCategory.CreateEntry("Max Stations", 3);
        _handlerMoveSpeedMultiplier = _handlerCategory.CreateEntry("Move Speed Multiplier", 1.0);
        _handlerPackagingSpeedMultiplier = _handlerCategory.CreateEntry("Packaging Speed Multiplier", 1.0);
        _handlerSigningFee = _handlerCategory.CreateEntry("Signing Fee", 1000.0);
        _handlerSigningFeeEnabled = _handlerCategory.CreateEntry("Custom Signing Fee", false,
            description: "The Custom Signing Bonus applies a flat fee.");

        // ----

        _dealerCategory = MelonPreferences.CreateCategory("Dealer");
        _dealerCategory.SetFilePath(Path.Combine(PreferencePath, "Dealer.cfg"));

        _dealerAutoCollectEnabled = _dealerCategory.CreateEntry("Auto Collect", true);
        _dealerCut = _dealerCategory.CreateEntry("Cut", 0.2, description: "[0.0-1.0] formula: cash * (1 - cut)");
        _dealerMoveSpeedMultiplier = _dealerCategory.CreateEntry("Move Speed Multiplier", 1.0);
        _dealerPatchEnabled = _dealerCategory.CreateEntry("Enabled", true);

        if (_dealerCut.Value > 1) _dealerCut.Value = 1;

        // ----

        _qolCategory = MelonPreferences.CreateCategory("Various QOL Features");
        _qolCategory.SetFilePath(Path.Combine(PreferencePath, "QOL.cfg"));

        _trashSmallerEnabled = _qolCategory.CreateEntry("Change Trash Scale On Interact", false,
            description: "Changes trash scale on grabbing them for the first time.");
        _trashSmallerScale = _qolCategory.CreateEntry("Trash Scale", 0.5,
            description: "[Min:0.2] The scale the item will be of after interacting.");
        _trashBagsOnly = _qolCategory.CreateEntry("Trash Bags Only", false,
            description: "Will only apply the scale to trash bags.");
        _trashScaleTime = _qolCategory.CreateEntry("Scale Time", 1.0,
            description: "Time it takes to Scale.");
        if (_trashSmallerScale.Value < 0.2) _trashSmallerScale.Value = 0.2;

        // ----


        var h = new HarmonyLib.Harmony("Bread_Tweaks");

        if (_employeePatchEnabled.Value) h.PatchAll(typeof(EmployeePatches));
        if (_dealerPatchEnabled.Value) h.PatchAll(typeof(DealerPatches));
        if (_dealerAutoCollectEnabled.Value) h.PatchAll(typeof(DealerAutoCollectPatch));
        if (_trashSmallerEnabled.Value) h.PatchAll(typeof(TrashItemPatches));
    }

    private class TrashItemPatches
    {
        [HarmonyPatch(typeof(TrashItem), "Interacted")]
        [HarmonyPrefix]
        private static void TrashPatch(TrashItem __instance)
        {
            if (!__instance.gameObject.GetComponent<TrashBag>() && _trashBagsOnly!.Value) return;

            var scaler = __instance.gameObject.GetComponent<TrashItemScaler>();
            if (scaler != null) return;
            scaler = __instance.gameObject.AddComponent<TrashItemScaler>();
            scaler.StartScaling((float)_trashScaleTime!.Value, (float)_trashSmallerScale!.Value);
        }

        [RegisterTypeInIl2Cpp]
        public class TrashItemScaler : MonoBehaviour
        {
            private readonly AnimationCurve _scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

            private IEnumerator ScaleOverTime(float duration, float scale)
            {
                var startScale = transform.localScale;
                var endScale = startScale * scale;
                var elapsed = 0f;

                while (elapsed < duration)
                {
                    if (transform == null) yield break;

                    var t = _scaleCurve.Evaluate(elapsed / duration);
                    transform.localScale = Vector3.Lerp(startScale, endScale, t);
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                if (transform != null) transform.localScale = endScale;
            }


            public void StartScaling(float duration, float scale) =>
                MelonCoroutines.Start(ScaleOverTime(duration, scale));
        }
    }


    private class DealerAutoCollectPatch
    {
        private static MoneyManager? _moneyManager;

        [HarmonyPatch(typeof(MoneyManager), "Start")]
        [HarmonyPrefix]
        private static void AssignMoneyManager(MoneyManager __instance) => _moneyManager = __instance;

        [HarmonyPatch(typeof(Dealer), "SubmitPayment")]
        [HarmonyPrefix]
        private static bool AutoCollect(Dealer __instance, float payment)
        {
            if (_moneyManager == null) return true;
            _moneyManager.CreateOnlineTransaction("Dealer Collection", payment * (1 - (float)_dealerCut!.Value), 1f,
                "Collected from " + __instance.FirstName);
            return false;
        }
    }

    private class DealerPatches
    {
        [HarmonyPatch(typeof(Dealer), "Start")]
        [HarmonyPrefix]
        private static void DealerPatch(Dealer __instance)
        {
            __instance.Cut = (float)_dealerCut!.Value;
            __instance.Movement.MoveSpeedMultiplier = (float)_dealerMoveSpeedMultiplier!.Value;
        }
    }

    private class EmployeePatches
    {
        [HarmonyPatch(typeof(Employee), "Start")]
        [HarmonyPrefix]
        private static void EmployeePatch(Employee __instance)
        {
            switch (__instance.Type)
            {
                case EEmployeeType.Botanist:
                    var botanist = __instance.GetComponent<Botanist>();
                    botanist.ADDITIVE_POUR_TIME = (float)_botanistAdditivePourTime!.Value;
                    botanist.HARVEST_TIME = (float)_botanistHarvestTime!.Value;
                    botanist.MaxAssignedPots = _botanistMaxPots!.Value;
                    botanist.SEED_SOW_TIME = (float)_botanistSeedSowTime!.Value;
                    botanist.SOIL_POUR_TIME = (float)_botanistSoilPourTime!.Value;
                    botanist.WATER_POUR_TIME = (float)_botanistWaterPourTime!.Value;

                    if (_botanistSigningFeeEnabled!.Value) botanist.SigningFee = (float)_botanistSigningFee!.Value;
                    botanist.DailyWage = (float)_botanistDailyWage!.Value;
                    botanist.Movement.MoveSpeedMultiplier = (float)_botanistMoveSpeedMultiplier!.Value;
                    break;
                case EEmployeeType.Chemist:
                    var chemist = __instance.GetComponent<Chemist>();
                    chemist.configuration.Stations.MaxItems = _chemistMaxStations!.Value;

                    if (_chemistSigningFeeEnabled!.Value) chemist.SigningFee = (float)_chemistSigningFee!.Value;
                    chemist.DailyWage = (float)_chemistDailyWage!.Value;
                    chemist.Movement.MoveSpeedMultiplier = (float)_chemistMoveSpeedMultiplier!.Value;
                    break;
                case EEmployeeType.Cleaner:
                    var cleaner = __instance.GetComponent<Cleaner>();
                    cleaner.configuration.Bins.MaxItems = _cleanerMaxBins!.Value;

                    if (_cleanerSigningFeeEnabled!.Value) cleaner.SigningFee = (float)_cleanerSigningFee!.Value;
                    cleaner.DailyWage = (float)_cleanerDailyWage!.Value;
                    cleaner.Movement.MoveSpeedMultiplier = (float)_cleanerMoveSpeedMultiplier!.Value;
                    break;
                case EEmployeeType.Handler:
                    var handler = __instance.GetComponent<Packager>();
                    handler.PackagingSpeedMultiplier = (float)_handlerPackagingSpeedMultiplier!.Value;
                    handler.configuration.Routes.MaxRoutes = _handlerMaxRoutes!.Value;
                    handler.configuration.Stations.MaxItems = _handlerMaxStations!.Value;

                    if (_handlerSigningFeeEnabled!.Value) handler.SigningFee = (float)_handlerSigningFee!.Value;
                    handler.DailyWage = (float)_handlerDailyWage!.Value;
                    handler.Movement.MoveSpeedMultiplier = (float)_handlerMoveSpeedMultiplier!.Value;
                    break;
                default:
                    MelonLogger.Error("Unknown Employee Type!");
                    break;
            }
        }
    }
}