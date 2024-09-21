using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GTA5ModRemover.Interfaces;
using GTA5ModRemover.Services;
using GTA5ModRemover.Utilities;
using Serilog;

namespace GTA5ModRemover
{
    internal static class Program
    {
        private const string AppName = "GTA5ModRemover.exe";
        private static readonly string[] ProtectedFiles = new[]
        {
            "bink2w64.dll", "common.rpf", "d3dcompiler_46.dll", "d3dcsx_46.dll", "GFSDK_ShadowLib.win64.dll",
            "GFSDK_TXAA.win64.dll", "GFSDK_TXAA_AlphaResolve.win64.dll", "GPUPerfAPIDX11-x64.dll", "GTA5.exe",
            "GTAVLanguageSelect.exe", "GTAVLauncher.exe", "NvPmApi.Core.win64.dll", "PlayGTAV.exe", "steam_api64.dll",
            "version.txt", "x64a.rpf", "x64b.rpf", "x64c.rpf", "x64d.rpf", "x64e.rpf", "x64f.rpf", "x64g.rpf", "x64h.rpf",
            "x64i.rpf", "x64j.rpf", "x64k.rpf", "x64l.rpf", "x64m.rpf", "x64n.rpf", "x64o.rpf", "x64p.rpf", "x64q.rpf",
            "x64r.rpf", "x64s.rpf", "x64t.rpf", "x64u.rpf", "x64v.rpf", "x64w.rpf", "installscript.vdf"
        };

        private static readonly string[] ProtectedFolders = new[]
        {
            "Installers", "Redistributables", "update", "x64", "Mods"
        };

        private static async Task Main()
        {
            LoggerSetup.InitializeLogger();

            try
            {
                var gameDirectory = Directory.GetCurrentDirectory();
                var modFolder = CreateModdedFolder(gameDirectory);

                var allItemsInDirectory = Directory.GetFileSystemEntries(gameDirectory, "*");
                var itemsToRelocate = FilterModdedItems(allItemsInDirectory);

                IFileMover fileMover = new FileMover();
                await fileMover.MoveFilesAsync(itemsToRelocate, modFolder);
                fileMover.DisplaySummary();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Fatal error: {ex.Message}");
            }
        }

        private static string CreateModdedFolder(string basePath)
        {
            var moddedFolderPath = Path.Combine(basePath, "Mods");
            if (!Directory.Exists(moddedFolderPath))
            {
                Directory.CreateDirectory(moddedFolderPath);
            }
            return moddedFolderPath;
        }

        private static string[] FilterModdedItems(string[] allItems)
        {
            return allItems.Where(item =>
                !ProtectedFiles.Contains(Path.GetFileName(item), StringComparer.OrdinalIgnoreCase) &&
                !ProtectedFolders.Contains(Path.GetFileName(item), StringComparer.OrdinalIgnoreCase) &&
                !string.Equals(Path.GetFileName(item), AppName, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
    }
}
