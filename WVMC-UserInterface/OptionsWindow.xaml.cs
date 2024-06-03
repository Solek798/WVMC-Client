using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WVMC_UserInterface.Components;

namespace WVMC_UserInterface;

public partial class OptionsWindow : Window
{
    public int NumberOfEntries { get; set; } = 0;
    
    public OptionsWindow(int numberOfEntries)
    {
        InitializeComponent();

        NumberOfEntries = numberOfEntries;

        for (var i = 0; i < NumberOfEntries; i++)
        {
            var newEntry = new EmptyShortcut();
            newEntry.RemovalWasRequested = ReplaceWithShortcut;
            ContentPanel.Children.Add(newEntry);
        }
    }

    private void ReplaceWithShortcut(UserControl emptyToRemove)
    {
        ContentPanel.Children.Remove(emptyToRemove);
    }
}