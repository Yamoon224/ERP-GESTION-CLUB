using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class ProduitFormPage : Page
{
    private readonly Produit? _item;

    public ProduitFormPage(Produit? item)
    {
        InitializeComponent();
        _item = item;
        CbClub.ItemsSource      = App.Db.Clubs.ToList();
        CbCategorie.ItemsSource = Enum.GetValues<CategorieProduit>();

        if (item != null)
        {
            TbTitle.Text             = $"Modifier — {item.NomFr}";
            CbClub.SelectedValue     = item.ClubId;
            TbNomFr.Text             = item.NomFr;
            TbNomEn.Text             = item.NomEn;
            CbCategorie.SelectedItem = item.Categorie;
            TbPrix.Text              = item.Prix;
            TbAncienPrix.Text        = item.AncienPrix ?? "";
            TbStock.Text             = item.Stock.ToString();
            CkNouveau.IsChecked      = item.EstNouveau;
            CkSolde.IsChecked        = item.EstEnSolde;
            BtnDelete.Visibility     = Visibility.Visible;
        }
        else
        {
            TbTitle.Text = "Nouveau produit";
        }
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new BoutiquePage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var db = App.Db;
        int.TryParse(TbStock.Text, out int stock);

        if (_item != null)
        {
            var p = db.Produits.Find(_item.Id)!;
            p.ClubId     = (string)CbClub.SelectedValue;
            p.NomFr      = TbNomFr.Text;
            p.NomEn      = TbNomEn.Text;
            p.Categorie  = (CategorieProduit)CbCategorie.SelectedItem;
            p.Prix       = TbPrix.Text;
            p.AncienPrix = TbAncienPrix.Text;
            p.Stock      = stock;
            p.EstNouveau = CkNouveau.IsChecked == true;
            p.EstEnSolde = CkSolde.IsChecked == true;
        }
        else
        {
            db.Produits.Add(new Produit
            {
                ClubId     = (string)CbClub.SelectedValue,
                NomFr      = TbNomFr.Text,
                NomEn      = TbNomEn.Text,
                Categorie  = (CategorieProduit)CbCategorie.SelectedItem,
                Prix       = TbPrix.Text,
                AncienPrix = TbAncienPrix.Text,
                Stock      = stock,
                EstNouveau = CkNouveau.IsChecked == true,
                EstEnSolde = CkSolde.IsChecked == true
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new BoutiquePage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show($"Supprimer '{_item!.NomFr}' ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var p = App.Db.Produits.Find(_item.Id);
            if (p != null) App.Db.Produits.Remove(p);
            App.Db.SaveChanges();
            NavigationService?.Navigate(new BoutiquePage());
        }
    }
}
