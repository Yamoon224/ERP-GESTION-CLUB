using System.Windows;
using System.Windows.Input;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp;

public partial class LoginWindow : Window
{
    public User? AuthenticatedUser { get; private set; }

    public LoginWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => TbUsername.Focus();
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e) => TryLogin();

    private void Input_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) TryLogin();
    }

    private void TryLogin()
    {
        TbError.Visibility = Visibility.Collapsed;

        var username = TbUsername.Text.Trim();
        var password = PbPassword.Password;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            TbError.Text = "Veuillez remplir tous les champs.";
            TbError.Visibility = Visibility.Visible;
            return;
        }

        var user = App.Db.Users
            .FirstOrDefault(u => u.Username == username && u.EstActif);

        if (user is null || !PasswordHelper.Verify(password, user.PasswordHash, user.PasswordSalt))
        {
            TbError.Text = "Identifiants incorrects ou compte désactivé.";
            TbError.Visibility = Visibility.Visible;
            PbPassword.Clear();
            return;
        }

        AuthenticatedUser = user;
        DialogResult = true;
    }
}
