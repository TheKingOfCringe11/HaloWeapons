using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

[assembly: AssemblyTitle("Halo Weapons")]
[assembly: AssemblyDescription("Weapons from Halo series")]
[assembly: AssemblyCompany("|GREEN|TheKingOfCringe|RED|11")]
[assembly: AssemblyVersion("1.0.0.0")]


namespace DuckGame.HaloWeapons
{
    public class HaloWeapons : Mod
    {
        protected override void OnPreInitialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolver.Resolve;
        }

        protected override void OnPostInitialize()
        {
            base.OnPostInitialize();

            BindingAttributes.Initialize();

            Resources.LoadShaders();
            Skins.Load();
            Options.Load();

            var harmony = new Harmony("null or empty");

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                try
                {
                    harmony.CreateClassProcessor(type).Patch();
                }
                catch
                {
                    Core.Log($"|RED|Failed to apply patch: {type.Name}");
                }
            }

            AccessTools.Field(typeof(Game), "updateableComponents").GetValue<List<IUpdateable>>(MonoMain.instance).Add(new Updater());
        }
    }
}

namespace System.Runtime.CompilerServices
{
    internal class IsExternalInit
    {

    }
}
