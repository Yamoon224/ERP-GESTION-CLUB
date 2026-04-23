using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class PalmaresFormPage : Page
{
    private readonly Palmares? _item;

    public PalmaresFormPage(Palmares? item)
    {
        InitializeComponent();
        _item = item;
        CbClub.ItemsSource = App.Db.Clubs.ToList();

        if (item != null)
        {
            TbTitle.Text         = $"Modifier — {item.Competition} {item.Annee}";
            CbClub.SelectedValue = item.ClubId;
            TbCompetition.Text   = item.Competition;
            TbAnnee.Text         = item.Annee.ToString();
            TbRang.Text          = item.Rang;
            TbDescription.Text   = item.Description ?? "";
            BtnDelete.Visibility = Visibility.Visible;
        }
        else TbTitle.Text = "Nouveau palmarès";
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new PalmaresPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var db = App.Db;
        int.TryParse(TbAnnee.Text, out int annee);

        if (_item != null)
        {
            var p = db.Palmares.Find(_item.Id)!;
            p.ClubId      = (string)CbClub.SelectedValue;
            p.Competition = TbCompetition.Text;
            p.Annee       = annee;
            p.Rang        = TbRang.Text;
            p.Description = TbDescription.Text;
        }
        else
        {
            db.Palmares.Add(new Palmares
            {
                ClubId      = (string)CbClub.SelectedValue,
                Competition = TbCompetition.Text,
                Annee       = annee,
                Rang        = TbRang.Text,
                Description = TbDescription.Text
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new PalmaresPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Supprimer ce titre ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var p = App.Db.Palmares.Find(_item!.Id);
            if (p != null) App.Db.Palmares.Remove(p);
            App.Db.SaveChanges();
            NavigationService?.Navigate(new PalmaresPage());
        }
    }
}
