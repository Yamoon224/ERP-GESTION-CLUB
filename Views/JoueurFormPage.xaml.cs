using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class JoueurFormPage : Page
{
    private readonly Joueur? _joueur;

    public JoueurFormPage(Joueur? joueur)
    {
        InitializeComponent();
        _joueur = joueur;

        CbCategorie.ItemsSource = Enum.GetValues<Categorie>();
        CbClub.ItemsSource = App.Db.Clubs.ToList();

        if (joueur != null)
        {
            TbTitle.Text        = $"Modifier — {joueur.Prenom} {joueur.Nom}";
            TbNumero.Text       = joueur.Numero.ToString();
            TbPrenom.Text       = joueur.Prenom;
            TbNom.Text          = joueur.Nom;
            TbPoste.Text        = joueur.Poste;
            TbTaille.Text       = joueur.Taille;
            TbDdN.Text          = joueur.DateNaissance;
            CbCategorie.SelectedItem  = joueur.Categorie;
            CbClub.SelectedValue      = joueur.ClubId;
            BtnDelete.Visibility = Visibility.Visible;
        }
        else
        {
            TbTitle.Text = "Nouveau joueur";
        }
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new JoueursPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TbNumero.Text, out int num)) num = 0;
        var db = App.Db;

        if (_joueur != null)
        {
            var j = db.Joueurs.Find(_joueur.Id)!;
            j.Numero        = num;
            j.Prenom        = TbPrenom.Text;
            j.Nom           = TbNom.Text;
            j.Poste         = TbPoste.Text;
            j.Taille        = TbTaille.Text;
            j.DateNaissance = TbDdN.Text;
            j.Categorie     = (Categorie)CbCategorie.SelectedItem;
            j.ClubId        = (string)CbClub.SelectedValue;
        }
        else
        {
            db.Joueurs.Add(new Joueur
            {
                Numero        = num,
                Prenom        = TbPrenom.Text,
                Nom           = TbNom.Text,
                Poste         = TbPoste.Text,
                Taille        = TbTaille.Text,
                DateNaissance = TbDdN.Text,
                Categorie     = (Categorie)CbCategorie.SelectedItem,
                ClubId        = (string)CbClub.SelectedValue
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new JoueursPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show($"Supprimer '{_joueur!.Prenom} {_joueur.Nom}' ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var j = App.Db.Joueurs.Find(_joueur.Id);
            if (j != null) App.Db.Joueurs.Remove(j);
            App.Db.SaveChanges();
            NavigationService?.Navigate(new JoueursPage());
        }
    }
}
