using System.IO;

namespace PokemonORASXYPatchPointerTool
{
    public class DirectoryComparer
    {
        private readonly string _targetElfPath;
        private readonly string _romFsDirectory;
        private readonly string _patchDirectory;
        

        public DirectoryComparer(string extractedCiaPath, string patchDirectory)
        {
            string extractedExeFsPath = Path.Combine(extractedCiaPath, "ExtractedExeFS");
            this._targetElfPath = Path.Combine(extractedExeFsPath, "code.bin");
            string extractedRomFsDirectory = Path.Combine(extractedCiaPath, "ExtractedRomFS");
            this._romFsDirectory = Path.Combine(extractedRomFsDirectory, "a");
            this._patchDirectory = Path.Combine(patchDirectory, "a");
        }

        public void CompareAndReplace()
        {
            CompareAndReplaceRecursive(_romFsDirectory, _patchDirectory);
        }

        private void CompareAndReplaceRecursive(string romFsDir, string patchDir)
        {
            DirectoryInfo romFs = new DirectoryInfo(romFsDir);
            DirectoryInfo patch = new DirectoryInfo(patchDir);
            CtrMount ctrMount = new CtrMount();
            IEnumerable<FileInfo> romFsFiles = romFs.GetFiles("*", SearchOption.AllDirectories);
            IEnumerable<FileInfo> patchFiles = patch.GetFiles("*", SearchOption.AllDirectories);
            // Выбираем только пути, содержащие "a\" в их пути.
            IEnumerable<string> romFsPaths = romFsFiles.Select(file => GetPathAfterA(file.FullName));
            IEnumerable<string> patchPaths = patchFiles.Select(file => GetPathAfterA(file.FullName));
            IEnumerable<string> commonPaths = romFsPaths.Intersect(patchPaths);
            IEnumerable<string> uniqueInPatch = patchPaths.Except(commonPaths);
            foreach (var patchedFile in uniqueInPatch)
            {
                    ctrMount.ReplacePathsInElf(_targetElfPath, "rom:/" + patchedFile, "rom2:/" + patchedFile.Replace("a\\", "a"));    
                    Console.WriteLine($"Patched {patchedFile}");
                    
                    string destinationPath = Path.Combine(romFs.FullName.Replace("\\a", ""), patchedFile.Replace("a\\", "a").Replace("\\", Path.DirectorySeparatorChar.ToString()));
                    
                    string originalPath = patchFiles.First(file => GetPathAfterA(file.FullName) == patchedFile).FullName;
                    string? destinationDirectory = Path.GetDirectoryName(destinationPath);
                    
                    
                    if (!Directory.Exists(destinationDirectory))
                    {
                        if (destinationDirectory != null) Directory.CreateDirectory(destinationDirectory);
                    }
                    
                    File.Copy(originalPath, destinationPath, true);
                    Console.WriteLine($"File copied: {originalPath} => {destinationPath}");
            }

            foreach (var replacedFile in commonPaths)
            {
                string originalPath = patchFiles.First(file => GetPathAfterA(file.FullName) == replacedFile).FullName;
                string destinationPath = Path.Combine(romFs.FullName.Replace("\\a", ""), replacedFile);
                Console.WriteLine($"File replaced: {originalPath} {destinationPath}");
            }
            
        }
        
        static string GetPathAfterA(string fullPath)
        {
            int index = fullPath.IndexOf("a\\", StringComparison.Ordinal);
            if (index != -1)
            {
                return fullPath.Substring(index);
            }
            return string.Empty;
        }


        private bool IsGarcFile(string filePath)
        {
            // Read the first four bytes of the file
            byte[] buffer = new byte[4];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Read(buffer, 0, buffer.Length);
            }

            // Check if the first four bytes match the "CRAG" magic number
            return buffer[0] == 'C' && buffer[1] == 'R' && buffer[2] == 'A' && buffer[3] == 'G';
        }
    }
}
