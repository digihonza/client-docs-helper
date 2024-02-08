using ClientDocsHelper.ConsoleUtilities;
using Microsoft.VisualBasic.FileIO;

namespace ClientDocsHelper
{
    internal class ProjectFolderCreationService
    {
        public void CreateProjectFolderStructure(string templateFolderPath, string clientsRootPath)
        {
            string clientName = PathEntryHelpers.ReadValidFileName("Zadejte jméno klienta:");
            string projectName = PathEntryHelpers.ReadValidFileName("Zadejte název projektu:");
            var projectFolderPath = Path.Combine(clientsRootPath, clientName, projectName);
            if (Path.Exists(projectFolderPath))
            {
                Console.WriteLine($"Složka pro klienta {clientName} a projekt {projectName} už existuje. Pokud jí chcete vytvořit znovu, je nutné nejdřív odstranit tu současnou.\n");
                // Possible new feature: The program could ask whether user wants to replace the existing folder and if so, then handle deleting the existing folder.
            } else
            {
                CopyProjectFolders(templateFolderPath, projectFolderPath);
            }
        }

        private void CopyProjectFolders(string templateFolderPath, string destinationFolderPath)
        {
            try
            {
                Directory.CreateDirectory(destinationFolderPath);
                FileSystem.CopyDirectory(templateFolderPath, destinationFolderPath);
                Console.WriteLine("Složky vytvořeny.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stala se chyba při kopírování složek. ({ex.Message})\n");
            }
        }
    }
}
