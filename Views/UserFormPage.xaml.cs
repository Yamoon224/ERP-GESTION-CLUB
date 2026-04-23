using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class UserFormPage : Page
{
    private readonly User? _user;
    private readonly bool  _isEdit;

    public UserFormPage(User? user)
    {
        InitializeComponent();

        // Remplir le ComboBox des rôles
        CbRole.ItemsSource = Enum.GetValues<RoleUser>();

        _user   = user;
        _isEdit = user is not null;
        TbTitle.Text = _isEdit ? $"Modifier — {user!.NomComplet}" : "Nouvel utilisateur";

        if (_isEdit)
        {
            TbNomComplet.Text     = user!.NomComplet;
            TbUsername.Text       = user.Username;
            TbUsername.IsEnabled  = false; // ne pas changer le login
            TbEmail.Text          = user.Email;
            CbRole.SelectedItem   = user.Role;
            ChkActif.IsChecked    = user.EstActif;
            TbPasswordHint.Visibility = Visibility.Visible;
            BtnDelete.Visibility  = Visibility.Visible;

            // Empêcher la suppression de son propre compte
            if (user.Id == App.CurrentUser.Id)
                BtnDelete.IsEnabled = false;
        }
        else
        {
            CbRole.SelectedItem = RoleUser.Gestionnaire;
        }
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new UsersPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        TbError.Visibility = Visibility.Collapsed;

        var nomComplet = TbNomComplet.Text.Trim();
        var username   = TbUsername.Text.Trim();
        var password   = PbPassword.Password;
        var role       = (RoleUser?)CbRole.SelectedItem;

        // Validation
        if (string.IsNullOrEmpty(nomComplet) || string.IsNullOrEmpty(username) || role is null)
        {
            TbError.Text = "Nom complet, nom d'utilisateur et rôle sont obligatoires.";
            TbError.Visibility = Visibility.Visible;
            return;
        }
        if (!_isEdit && string.IsNullOrEmpty(password))
        {
            TbError.Text = "Le mot de passe est obligatoire pour un nouvel utilisateur.";
            TbError.Visibility = Visibility.Visible;
            return;
        }
        // Unicité du nom d'utilisateur
        int currentId = _user?.Id ?? 0;
        bool usernameExists = App.Db.Users
            .Any(u => u.Username == username && u.Id != currentId);
        if (usernameExists)
        {
            TbError.Text = $"Le nom d'utilisateur « {username} » est déjà utilisé.";
            TbError.Visibility = Visibility.Visible;
            return;
        }

        var db = App.Db;

        if (_isEdit)
        {
            var u = db.Users.Find(_user!.Id)!;
            u.NomComplet = nomComplet;
            u.Email      = TbEmail.Text.Trim();
            u.Role       = role.Value;
            u.EstActif   = ChkActif.IsChecked == true;

            if (!string.IsNullOrEmpty(password))
            {
                var (hash, salt) = PasswordHelper.Hash(password);
                u.PasswordHash = hash;
                u.PasswordSalt = salt;
            }
        }
        else
        {
            var (hash, salt) = PasswordHelper.Hash(password);
            db.Users.Add(new User
            {
                Username     = username,
                PasswordHash = hash,
                PasswordSalt = salt,
                NomComplet   = nomComplet,
                Email        = TbEmail.Text.Trim(),
                Role         = role.Value,
                EstActif     = ChkActif.IsChecked == true,
                DateCreation = DateTime.UtcNow
            });
        }

        db.SaveChanges();
        NavigationService?.Navigate(new UsersPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show(
                $"Supprimer l'utilisateur « {_user!.NomComplet} » ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning)
            != MessageBoxResult.Yes) return;

        var u = App.Db.Users.Find(_user.Id);
        if (u is not null) App.Db.Users.Remove(u);
        App.Db.SaveChanges();
        NavigationService?.Navigate(new UsersPage());
    }
}
