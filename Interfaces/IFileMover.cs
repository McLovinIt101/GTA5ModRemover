using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTA5ModRemover.Interfaces
{
    public interface IFileMover
    {
        Task MoveFilesAsync(IEnumerable<string> items, string destinationPath);
        void DisplaySummary();
    }
}
