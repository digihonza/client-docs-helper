using ClientDocsHelper.ConsoleUtilities;
using Microsoft.VisualBasic.FileIO;

namespace ClientDocsHelper
{
    internal class ClientFolderCreationService
    {
        public void CreateClientFolderStructure(string templateFolderPath, string clientsRootPath)
        {
            string? clientName = PathEntryHelpers.ReadValidFileName("Zadejte jméno klienta:");
            CopyClientFolders(templateFolderPath, Path.Combine(clientsRootPath, clientName));
            Console.WriteLine("Složky vytvořeny.\n");
        }

        private void CopyClientFolders(string templateFolderPath, string destinationFolderPath)
        {
            try
            {
                FileSystem.CopyDirectory(templateFolderPath, destinationFolderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stala se chyba při kopírování složek. ({ex.Message})");
            }
        }
    }
}
