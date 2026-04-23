using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class PhotoFormPage : Page
{
    private readonly Photo? _item;

    public PhotoFormPage(Photo? item)
    {
        InitializeComponent();
        _item = item;
        CbClub.ItemsSource = App.Db.Clubs.ToList();

        if (item != null)
        {
            TbTitle.Text         = "Modifier photo";
            CbClub.SelectedValue = item.ClubId;
            TbUrl.Text           = item.Url;
            TbLegende.Text       = item.Legende ?? "";
            TbCategorie.Text     = item.CategoriePhoto ?? "";
            BtnDelete.Visibility = Visibility.Visible;
        }
        else TbTitle.Text = "Nouvelle photo";
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new PhotosPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var db = App.Db;
        if (_item != null)
        {
            var p = db.Photos.Find(_item.Id)!;
            p.ClubId        = (string)CbClub.SelectedValue;
            p.Url           = TbUrl.Text;
            p.Legende       = TbLegende.Text;
            p.CategoriePhoto= TbCategorie.Text;
        }
        else
        {
            db.Photos.Add(new Photo
            {
                ClubId        = (string)CbClub.SelectedValue,
                Url           = TbUrl.Text,
                Legende       = TbLegende.Text,
                CategoriePhoto= TbCategorie.Text
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new PhotosPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Supprimer cette photo ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var p = App.Db.Photos.Find(_item!.Id);
            if (p != null) App.Db.Photos.Remove(p);
            App.Db.SaveChanges();
            NavigationService?.Navigate(new PhotosPage());
        }
    }
}
