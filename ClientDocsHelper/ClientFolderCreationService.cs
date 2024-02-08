using ClientDocsHelper.ConsoleUtilities;
using Microsoft.VisualBasic.FileIO;

namespace ClientDocsHelper
{
    internal class ClientFolderCreationService
    {
        public void CreateClientFolderStructure(string templateFolderPath, string clientsRootPath)
        {
            string? clientName = PathEntryHelpers.ReadValidFileName("Zadejte jméno klienta:");
            var newFolderPath = Path.Combine(clientsRootPath, clientName);
            if (Path.Exists(newFolderPath))
            {
                Console.WriteLine($"Klientská složka pro klienta {clientName} už existuje. Pokud jí chcete vytvořit znovu, je nutné nejdřív odstranit současnou složku.\n");
                // Possible new feature: The program could ask whether user wants to replace the existing folder and if so, then handle deleting the existing folder.
            } else
            {
                CopyClientFolders(templateFolderPath, Path.Combine(clientsRootPath, clientName));
            }
        }

        private void CopyClientFolders(string templateFolderPath, string destinationFolderPath)
        {
            try
            {
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
