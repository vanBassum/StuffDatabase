using STDLib.Saveable;
using STDLib.Serializers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace StuffDatabase
{
    public sealed class Settings : BaseSettings
    {
        public static string ComponentDB { get { return GetPar(@"Resources\Components\Database.json"); } set { SetPar(value); } }
        public static string ComponentTemplates { get { return GetPar(@"Resources\Components\Templates"); } set { SetPar(value); } }

        public static string ChemicalDB { get { return GetPar(@"Resources\Chemicals\Database.json"); } set { SetPar(value); } }
        public static string ChemicalTemplates { get { return GetPar(@"Resources\Chemicals\Templates"); } set { SetPar(value); } }
        public static string ChemicalSymbols { get { return GetPar(@"Resources\Chemicals\Symbols"); } set { SetPar(value); } }

    }







}