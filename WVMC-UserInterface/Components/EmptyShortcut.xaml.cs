using System;
using System.Windows;
using System.Windows.Controls;

namespace WVMC_UserInterface.Components;

public partial class EmptyShortcut : UserControl
{
    public Action<UserControl>? RemovalWasRequested;
    public EmptyShortcut()
    {
        InitializeComponent();
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        RemovalWasRequested?.Invoke(this);
    }
}