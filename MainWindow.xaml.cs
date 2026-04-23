using System.Windows;
using System.Windows.Controls;
using guinea_football_club_erp.Views;

namespace guinea_football_club_erp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new DashboardPage());
    }

    private void Nav_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            MainFrame.Navigate(btn.Tag?.ToString() switch
            {
                "Dashboard"  => (object)new DashboardPage(),
                "Clubs"      => new ClubsPage(),
                "Joueurs"    => new JoueursPage(),
                "Matches"    => new MatchesPage(),
                "Actualites" => new ActualitesPage(),
                "Boutique"   => new BoutiquePage(),
                "Billets"    => new BilletsPage(),
                "Classement" => new ClassementPage(),
                "Palmares"   => new PalmaresPage(),
                "Photos"     => new PhotosPage(),
                _ => new DashboardPage()
            });
        }
    }
}