﻿using System.Windows;
using System.Windows.Controls;

namespace ChessUI;
/// <summary>
/// Interaction logic for PauseMenu.xaml
/// </summary>
public partial class PauseMenu : UserControl
{
    public event Action<Option> OptionSelected;
    public PauseMenu()
    {
        InitializeComponent();
    }

    private void Continue_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Continue);
    }

    private void Restart_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Restart);
    }

    private void Flip_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Flip);
    }

    private void Undo_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Undo);
    }
}
