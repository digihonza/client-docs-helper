namespace ClientDocsHelper.ConsoleUtilities
{
    internal static class PathEntryHelpers
    {
        public static string? ReadValidPath(string prompt, bool notNullOrEmpty)
        {
            string? value;
            do
            {
                Console.WriteLine(prompt);
                value = Console.ReadLine();
            } while (notNullOrEmpty && (string.IsNullOrWhiteSpace(value) || !ValidatePath(value)) || (!string.IsNullOrEmpty(value) && !ValidatePath(value)));
            return value;
        }

        public static string ReadValidFileName(string prompt)
        {
            string? value;
            do
            {
                Console.WriteLine(prompt);
                value = Console.ReadLine();

            } while (string.IsNullOrWhiteSpace(value) || !ValidateFileNameCharacters(value));
            return value;
        }

        private static bool ValidatePath(string path) => ValidatePathCharacters(path) && ValidatePathExists(path);

        private static bool ValidatePathExists(string path)
        {
            if (!Path.Exists(path))
            {
                Console.WriteLine("Zadaná složka neexistuje");
                return false;
            }
            return true;
        }

        private static bool ValidatePathCharacters(string path)
        {
            var invalidCharIndex = path.IndexOfAny(Path.GetInvalidPathChars());
            if (invalidCharIndex >= 0)
            {
                Console.WriteLine($"Následující znak není povolen: '{path[invalidCharIndex]}'");
                return false;
            }
            return true;
        }

        private static bool ValidateFileNameCharacters(string path)
        {
            var invalidCharIndex = path.IndexOfAny(Path.GetInvalidFileNameChars());
            if (invalidCharIndex >= 0)
            {
                Console.WriteLine($"Následující znak není povolen: '{path[invalidCharIndex]}'");
                return false;
            }
            return true;
        }
    }
}
