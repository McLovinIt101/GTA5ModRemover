using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GTA5ModRemover.Interfaces;
using Serilog;

namespace GTA5ModRemover.Services
{
    public class FileMover : IFileMover
    {
        private int _filesMovedCount;
        private int _foldersSkippedCount;

        public async Task MoveFilesAsync(IEnumerable<string> items, string destinationPath)
        {
            var moveTasks = items.Select(item => Task.Run(() => MoveItem(item, destinationPath)));
            await Task.WhenAll(moveTasks);
        }

        private void MoveItem(string item, string destinationPath)
        {
            var destPath = Path.Combine(destinationPath, Path.GetFileName(item));
            try
            {
                if (File.Exists(item))
                {
                    File.Move(item, destPath);
                    _filesMovedCount++;
                }
                else if (Directory.Exists(item))
                {
                    Directory.Move(item, destPath);
                    _filesMovedCount++;
                }
            }
            catch (IOException ioEx)
            {
                Log.Warning($"I/O error moving {item}: {ioEx.Message}");
                _foldersSkippedCount++;
            }
            catch (UnauthorizedAccessException authEx)
            {
                Log.Warning($"Access error moving {item}: {authEx.Message}");
                _foldersSkippedCount++;
            }
            catch (Exception ex)
            {
                Log.Error($"Unexpected error moving {item}: {ex.Message}");
                _foldersSkippedCount++;
            }
        }

        public void DisplaySummary()
        {
            Log.Information("Mod cleanup complete.");
            Log.Information($"{_filesMovedCount} item(s) moved to 'Mods'. {_foldersSkippedCount} folder(s) skipped.");
        }
    }
}
