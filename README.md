# PokemonORASXYPatchPointerTool

This tool is designed to facilitate working with updates for Pokemon Omega Ruby, Pokemon Alpha Sapphire, Pokemon X, and Pokemon Y games on the Nintendo 3DS console. It provides the ability to replace and add paths to `.garc` files inside the executable file (`code.bin`), allowing the integration of mods and translations directly into the official game versions.

## Features
- Replace and add paths to `.garc` files inside update executable files for Pokemon ORAS and XY games.
- Support for including mods and translations for RomFS inside CIA files for the Nintendo 3DS.

## How to Use
1. Ensure you have .Net 8 Runtime installed on your system.
2. Use [HackingToolkit9DS](https://github.com/Asia81/HackingToolkit9DS) to unpack the CIA file of the game update (`PackHack`).
3. Run the program, specifying the path to the unpacked game update (`PackHack`) and the directory with patched (`RomFS`).
4. The program will automatically perform path replacement in code.bin and addition according to the provided patches and copied all files to specified paths.

## Requirements
- .Net 8 Runtime
- [HackingToolkit9DS](https://github.com/Asia81/HackingToolkit9DS)

## License
This project is licensed under the [GPL 3 License](LICENSE).
