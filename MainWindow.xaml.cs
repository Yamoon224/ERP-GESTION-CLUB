using System.Windows;
using System.Windows.Controls;
using guinea_football_club_erp.Views;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        UpdateSidebar();
        MainFrame.Navigate(new DashboardPage());
    }

    private void UpdateSidebar()
    {
        TbUserName.Text = App.CurrentUser.NomComplet;
        TbUserRole.Text = App.CurrentUser.Role switch
        {
            RoleUser.Admin        => "Administrateur",
            RoleUser.Gestionnaire => "Gestionnaire",
            RoleUser.Lecteur      => "Lecteur",
            _                     => App.CurrentUser.Role.ToString()
        };
        BtnNavUsers.Visibility = App.CurrentUser.Role == RoleUser.Admin
            ? Visibility.Visible : Visibility.Collapsed;
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
                "Users"      => new UsersPage(),
                _            => new DashboardPage()
            });
        }
    }

    private void BtnLogout_Click(object sender, RoutedEventArgs e)
    {
        var login = new LoginWindow();
        if (login.ShowDialog() == true)
        {
            App.CurrentUser = login.AuthenticatedUser!;
            UpdateSidebar();
            MainFrame.Navigate(new DashboardPage());
        }
        else
        {
            Application.Current.Shutdown();
        }
    }
}