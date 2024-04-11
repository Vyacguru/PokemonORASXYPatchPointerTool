using System.IO;
using System.Text;

namespace PokemonORASXYPatchPointerTool
{
    /// <summary>
    /// Class for mounting and modifying Elf files.
    /// </summary>
    public class CtrMount
    {
        /// <summary>
        /// Replaces paths in the Pokemon ROM Elf file.
        /// </summary>
        /// <param name="elfPath">Path to the Pokemon ROM Elf file.</param>
        /// <param name="oldPath">Old path to replace.</param>
        /// <param name="newPath">New path to replace with.</param>
        public void ReplacePathsInElf(string elfPath, string oldPath, string newPath)
        {
            // Read all bytes from the Elf file
            byte[] originalData = File.ReadAllBytes(elfPath);

            // Convert rom and patch paths to byte arrays
            byte[] romPath = Encoding.Unicode.GetBytes(oldPath.Replace('\\', '/'));
            byte[] patchPath = Encoding.Unicode.GetBytes(newPath.Replace('\\', '/'));
            
            Console.WriteLine($"Original path: {Encoding.Unicode.GetString(romPath)} Patched path: {Encoding.Unicode.GetString(patchPath)}");
            // Find the index of the old path in the Elf file
            int indexOfRomPath = FindIndexOfBytes(originalData, romPath);
            if (indexOfRomPath < 0)
            {
                // If the old path is not found, throw an exception
                throw new InvalidOperationException("Index of Rom path string not found.");
            }

            // Create a new byte array to store patched data
            byte[] patchedData = new byte[originalData.Length + patchPath.Length - oldPath.Length];
            
            // Copy data before the old path
            Buffer.BlockCopy(originalData, 0, patchedData, 0, indexOfRomPath);
            
            // Copy the new path to the patched data
            Buffer.BlockCopy(patchPath, 0, patchedData, indexOfRomPath, patchPath.Length);
            
            // Copy remaining data after the old path
            Buffer.BlockCopy(originalData, indexOfRomPath + romPath.Length, patchedData, indexOfRomPath + patchPath.Length, originalData.Length - indexOfRomPath - romPath.Length);
            
            // Write the patched data back to the Elf file
            File.WriteAllBytes(elfPath, patchedData);
        }

        /// <summary>
        /// Finds the index of a byte sequence in a byte array.
        /// </summary>
        /// <param name="searchBuffer">The byte array to search.</param>
        /// <param name="bytesToFind">The byte sequence to find.</param>
        /// <returns>The index of the byte sequence, or -1 if not found.</returns>
        private static int FindIndexOfBytes(byte[] searchBuffer, byte[] bytesToFind)
        {
            for (int index1 = 0; index1 < searchBuffer.Length - bytesToFind.Length; ++index1)
            {
                bool flag = true;
                for (int index2 = 0; index2 < bytesToFind.Length; ++index2)
                {
                    if (searchBuffer[index1 + index2] != bytesToFind[index2])
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                    return index1;
            }

            return -1;
        }
    }
}