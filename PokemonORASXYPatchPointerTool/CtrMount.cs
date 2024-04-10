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
            byte[] romPath = Encoding.UTF8.GetBytes(oldPath.Replace('\\', '/'));
            byte[] patchPath = Encoding.UTF8.GetBytes(newPath.Replace('\\', '/'));
            
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
            // Check if searchBuffer is shorter than bytesToFind
            if (searchBuffer.Length == 0 || bytesToFind.Length == 0 || searchBuffer.Length < bytesToFind.Length)
            {
                // If either array is empty or if searchBuffer is shorter than bytesToFind, throw an exception
                throw new ArgumentException("Invalid search buffer or bytes to find.");
            }

            // Iterate through searchBuffer
            for (int index = 0; index <= searchBuffer.Length - bytesToFind.Length; index++)
            {
                // Check if the byte sequence matches at the current index
                bool found = !bytesToFind.Where((t, j) => searchBuffer[index + j] != t).Any();
                if (found)
                {
                    return index; // Return the index of the byte sequence
                }
            }
            
            // If the byte sequence is not found, throw an exception
            throw new InvalidOperationException("Bytes to find not found in search buffer.");
        }
    }
}