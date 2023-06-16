using CommandLine;

namespace ArkGollum
{
    public class Options
    {
        [Option('m', "mode", Required = true, HelpText = "BASE, MOD, WORKSHOP")]
        public Mode Mode { get; set; }

        [Option('p', "purge", Required = false, HelpText = "Gollum will remove all mode associated files he has created in the folder structure specified")]
        public bool Purge { get; set; }

        [Option('s', "sPlus", Default = false, Required = false, HelpText = "Include pull resource additions string for Structures Plus")]
        public bool SPlus { get; set; } = false;

        [Option('S', "simpleSpawners", Default = false, Required = false, HelpText = "Include a string of found dinos to import into Simple Spawners")]
        public bool SimpleSpawners { get; set; } = false;

        [Option('t', "targetFolder", Required = true, HelpText = "The folder Gollum should begin his hunt in, this should be either your ARK folder or your steam workshop cache folder:\n\nZ:\\SteamLibrary\\steamapps\\common\\ARK\\ ")]
        public string TargetFolder { get; set; } = "";
    }
}