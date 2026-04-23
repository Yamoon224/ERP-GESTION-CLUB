using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class BilletFormPage : Page
{
    private readonly Billet? _item;

    public BilletFormPage(Billet? item)
    {
        InitializeComponent();
        _item = item;
        CbClub.ItemsSource = App.Db.Clubs.ToList();
        CbType.ItemsSource = Enum.GetValues<TypePlace>();

        if (item != null)
        {
            TbTitle.Text             = $"Modifier billet — {item.TypePlace}";
            CbClub.SelectedValue     = item.ClubId;
            CbMatch.ItemsSource      = App.Db.Matches.Where(m => m.ClubId == item.ClubId).ToList();
            CbMatch.SelectedValue    = item.MatchId;
            CbType.SelectedItem      = item.TypePlace;
            TbPrix.Text              = item.Prix;
            TbTotal.Text             = item.NombreTotal.ToString();
            TbDispo.Text             = item.NombreDisponible.ToString();
            BtnDelete.Visibility     = Visibility.Visible;
        }
        else
        {
            TbTitle.Text = "Nouveau billet";
        }
    }

    private void CbClub_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (CbClub.SelectedValue is string clubId)
            CbMatch.ItemsSource = App.Db.Matches.Where(m => m.ClubId == clubId).ToList();
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new BilletsPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var db = App.Db;
        int.TryParse(TbTotal.Text, out int total);
        int.TryParse(TbDispo.Text, out int dispo);
        int matchId = CbMatch.SelectedValue is int mid ? mid : 0;

        if (_item != null)
        {
            var b = db.Billets.Find(_item.Id)!;
            b.ClubId            = (string)CbClub.SelectedValue;
            b.MatchId           = matchId;
            b.TypePlace         = (TypePlace)CbType.SelectedItem;
            b.Prix              = TbPrix.Text;
            b.NombreTotal       = total;
            b.NombreDisponible  = dispo;
        }
        else
        {
            db.Billets.Add(new Billet
            {
                ClubId           = (string)CbClub.SelectedValue,
                MatchId          = matchId,
                TypePlace        = (TypePlace)CbType.SelectedItem,
                Prix             = TbPrix.Text,
                NombreTotal      = total,
                NombreDisponible = dispo
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new BilletsPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Supprimer ce billet ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var b = App.Db.Billets.Find(_item!.Id);
            if (b != null) App.Db.Billets.Remove(b);
            App.Db.SaveChanges();
            NavigationService?.Navigate(new BilletsPage());
        }
    }
}
