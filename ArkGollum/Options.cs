using CommandLine;

namespace ArkGollum
{
    public class Options
    {
        [Option('f', "folderIncompatibilityLog", Required = false, Default = false, HelpText = "Gollum will let you know which folders were scanned yet produced no precious")]
        public bool LogNoPrecious { get; set; }
        [Option('m', "mode", Required = true, HelpText = "BASE, MOD, WORKSHOP")]
        public Mode Mode { get; set; }

        [Option('o',"output", Required = false, HelpText = "Specify an output folder if you prefer ArkGollum to place all output in one location")]
        public string? Output { get; set; }

        [Option('p', "purge", Required = false, HelpText = "Gollum will remove all mode associated files he has created in the folder structure specified")]
        public bool Purge { get; set; }

        [Option('s', "sPlus", Default = false, Required = false, HelpText = "Include pull resource additions string for Structures Plus")]
        public bool SPlus { get; set; } = false;

        [Option('S', "simpleSpawners", Default = false, Required = false, HelpText = "Include a string of found dinos to import into Simple Spawners")]
        public bool SimpleSpawners { get; set; } = false;

        [Option('t', "targetFolder", Required = true, HelpText = "The folder Gollum should begin his hunt in, this should be either your ARK folder or your steam workshop cache folder:\n\nZ:\\SteamLibrary\\steamapps\\common\\ARK\\ ")]
        public string TargetFolder { get; set; } = "";
        

        public bool OutputSpecified { get; set; } = false;
    }
}