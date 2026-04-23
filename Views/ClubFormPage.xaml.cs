using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class ClubFormPage : Page
{
    private readonly Club? _club;
    private readonly bool _isEdit;

    public ClubFormPage(Club? club)
    {
        InitializeComponent();
        _club = club;
        _isEdit = club != null;
        TbTitle.Text = _isEdit ? $"Modifier — {club!.Nom}" : "Nouveau club";

        if (_isEdit)
        {
            TbId.Text           = club!.Id;
            TbId.IsEnabled      = false;
            TbNom.Text          = club.Nom;
            TbAcronyme.Text     = club.Acronyme;
            TbVille.Text        = club.Ville;
            TbFondation.Text    = club.Fondation;
            TbCouleurP.Text     = club.CouleurPrimaire;
            TbCouleurS.Text     = club.CouleurSecondaire;
            TbDescription.Text  = club.Description;
            BtnDelete.Visibility = Visibility.Visible;
        }
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new ClubsPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var db = App.Db;
        if (_isEdit)
        {
            var c = db.Clubs.Find(_club!.Id)!;
            c.Nom              = TbNom.Text;
            c.Acronyme         = TbAcronyme.Text;
            c.Ville            = TbVille.Text;
            c.Fondation        = TbFondation.Text;
            c.CouleurPrimaire  = TbCouleurP.Text;
            c.CouleurSecondaire= TbCouleurS.Text;
            c.Description      = TbDescription.Text;
        }
        else
        {
            db.Clubs.Add(new Club
            {
                Id               = TbId.Text.Trim(),
                Nom              = TbNom.Text,
                Acronyme         = TbAcronyme.Text,
                Ville            = TbVille.Text,
                Fondation        = TbFondation.Text,
                CouleurPrimaire  = TbCouleurP.Text,
                CouleurSecondaire= TbCouleurS.Text,
                Description      = TbDescription.Text
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new ClubsPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show($"Supprimer le club '{_club!.Nom}' ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var db = App.Db;
            var c = db.Clubs.Find(_club.Id);
            if (c != null) db.Clubs.Remove(c);
            db.SaveChanges();
            NavigationService?.Navigate(new ClubsPage());
        }
    }
}
