using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class ActualiteFormPage : Page
{
    private readonly Actualite? _item;

    public ActualiteFormPage(Actualite? item)
    {
        InitializeComponent();
        _item = item;
        CbClub.ItemsSource = App.Db.Clubs.ToList();

        if (item != null)
        {
            TbTitle.Text             = $"Modifier — {item.Titre}";
            CbClub.SelectedValue     = item.ClubId;
            TbTitre.Text             = item.Titre;
            TbCategorie.Text         = item.CategorieArticle;
            TbImage.Text             = item.Image ?? "";
            TbContenu.Text           = item.Contenu;
            CkPublie.IsChecked       = item.EstPublie;
            BtnDelete.Visibility     = Visibility.Visible;
        }
        else
        {
            TbTitle.Text       = "Nouvelle actualité";
            CkPublie.IsChecked = true;
        }
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new ActualitesPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var db = App.Db;
        if (_item != null)
        {
            var a = db.Actualites.Find(_item.Id)!;
            a.ClubId          = (string)CbClub.SelectedValue;
            a.Titre           = TbTitre.Text;
            a.CategorieArticle= TbCategorie.Text;
            a.Image           = TbImage.Text;
            a.Contenu         = TbContenu.Text;
            a.EstPublie       = CkPublie.IsChecked == true;
        }
        else
        {
            db.Actualites.Add(new Actualite
            {
                ClubId          = (string)CbClub.SelectedValue,
                Titre           = TbTitre.Text,
                CategorieArticle= TbCategorie.Text,
                Image           = TbImage.Text,
                Contenu         = TbContenu.Text,
                EstPublie       = CkPublie.IsChecked == true
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new ActualitesPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Supprimer cette actualité ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var a = App.Db.Actualites.Find(_item!.Id);
            if (a != null) App.Db.Actualites.Remove(a);
            App.Db.SaveChanges();
            NavigationService?.Navigate(new ActualitesPage());
        }
    }
}
