using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WVMC_UserInterface.Components;

public partial class ShortcutEntry : UserControl
{
    public int Id { get; private set; }

    private List<Key> _modifierKey;
    
    public ShortcutEntry()
    {
        _modifierKey = new List<Key>();
        
        InitializeComponent();
    }
    private void OnSetMainKeyClicked(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OnDeleteEntryClicked(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OnModifierButtonToggled(object sender, RoutedEventArgs e)
    {
        if (Equals(sender, LShiftToggle) && (bool) LShiftToggle.IsChecked!)
            _modifierKey.Add(Key.LeftShift);
        if (Equals(sender, RShiftToggle) && (bool) RShiftToggle.IsChecked!)
            _modifierKey.Add(Key.RightShift);
    }
}