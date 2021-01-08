using STDLib.Saveable;
using STDLib.Serializers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace StuffDatabase
{
    public sealed class Settings : BaseSettingsV2<Settings>
    {
        public string DataFolder { get { return GetPar(DefaultDataFolder); } set { SetPar(value); } }
        public string PartsDatabaseFile { get { return GetPar(Path.Combine(DataFolder, "Parts.json")); } set { SetPar(value); } }
        public string PartDescriptorsDatabaseFile { get { return GetPar(Path.Combine(DataFolder, "Descriptors.json")); } set { SetPar(value); } }
    }







}