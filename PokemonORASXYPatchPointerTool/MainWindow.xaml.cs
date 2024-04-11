using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace PokemonORASXYPatchPointerTool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void SelectPatchPath_Click(object sender, RoutedEventArgs e)
    {
        // Открыть диалог выбора пути для патча
        OpenFolderDialog openFolderDialog = new OpenFolderDialog();
        openFolderDialog.Title = "Select Patch Path";
        if (openFolderDialog.ShowDialog() == true)
        {
            // Установить выбранный путь в TextBox
            PatchPathTextBox.Text = openFolderDialog.FolderName;
        }
    }

    private void SelectCiaPath_Click(object sender, RoutedEventArgs e)
    {
        // Открыть диалог выбора пути для извлеченного CIA
        OpenFolderDialog openFolderDialog = new OpenFolderDialog();
        openFolderDialog.Title = "Select Extracted CIA Path";
        if (openFolderDialog.ShowDialog() == true)
        {
            // Установить выбранный путь в TextBox
            CiaPathTextBox.Text = openFolderDialog.FolderName;
        }
    }

    private void MakePatch_Click(object sender, RoutedEventArgs e)
    {
        string patchPath = PatchPathTextBox.Text;
        string ciaPath = CiaPathTextBox.Text;

        // Проверка, что оба пути не пусты
        if (string.IsNullOrEmpty(patchPath) || string.IsNullOrEmpty(ciaPath))
        {
            MessageBox.Show("Please select both patch and extracted CIA paths.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Проверка, что файлы по указанным путям существуют
        if (!Path.Exists(patchPath) || !Path.Exists(ciaPath))
        {
            MessageBox.Show("One or both of the selected folders do not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            DirectoryComparer dirComparer = new DirectoryComparer(ciaPath, patchPath);
            dirComparer.CompareAndReplace();
            MessageBox.Show("Done", "Success", MessageBoxButton.OK);
        }
    }
}