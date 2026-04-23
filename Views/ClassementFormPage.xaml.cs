using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class ClassementFormPage : Page
{
    private readonly ClassementEntree? _item;

    public ClassementFormPage(ClassementEntree? item)
    {
        InitializeComponent();
        _item = item;
        CbClub.ItemsSource      = App.Db.Clubs.ToList();
        CbCategorie.ItemsSource = Enum.GetValues<Categorie>();

        if (item != null)
        {
            TbTitle.Text             = $"Modifier — {item.Club?.Nom ?? item.ClubId}";
            CbClub.SelectedValue     = item.ClubId;
            TbCompetition.Text       = item.Competition;
            TbSaison.Text            = item.Saison;
            CbCategorie.SelectedItem = item.Categorie;
            TbPos.Text  = item.Position.ToString();
            TbPts.Text  = item.Points.ToString();
            TbJ.Text    = item.Joues.ToString();
            TbV.Text    = item.Victoires.ToString();
            TbN.Text    = item.Nuls.ToString();
            TbD.Text    = item.Defaites.ToString();
            TbBP.Text   = item.ButsPour.ToString();
            TbBC.Text   = item.ButsContre.ToString();
            BtnDelete.Visibility = Visibility.Visible;
        }
        else TbTitle.Text = "Nouvelle entrée classement";
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new ClassementPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var db = App.Db;
        int.TryParse(TbPos.Text, out int pos);
        int.TryParse(TbPts.Text, out int pts);
        int.TryParse(TbJ.Text,   out int j);
        int.TryParse(TbV.Text,   out int v);
        int.TryParse(TbN.Text,   out int n);
        int.TryParse(TbD.Text,   out int d);
        int.TryParse(TbBP.Text,  out int bp);
        int.TryParse(TbBC.Text,  out int bc);

        if (_item != null)
        {
            var e2 = db.Classements.Find(_item.Id)!;
            e2.ClubId      = (string)CbClub.SelectedValue;
            e2.Competition = TbCompetition.Text;
            e2.Saison      = TbSaison.Text;
            e2.Categorie   = (Categorie)CbCategorie.SelectedItem;
            e2.Position    = pos; e2.Points  = pts; e2.Joues     = j;
            e2.Victoires   = v;   e2.Nuls    = n;   e2.Defaites  = d;
            e2.ButsPour    = bp;  e2.ButsContre = bc;
        }
        else
        {
            db.Classements.Add(new ClassementEntree
            {
                ClubId = (string)CbClub.SelectedValue,
                Competition = TbCompetition.Text, Saison = TbSaison.Text,
                Categorie = (Categorie)CbCategorie.SelectedItem,
                Position = pos, Points = pts, Joues = j,
                Victoires = v, Nuls = n, Defaites = d, ButsPour = bp, ButsContre = bc
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new ClassementPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Supprimer cette entrée ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var entry = App.Db.Classements.Find(_item!.Id);
            if (entry != null) App.Db.Classements.Remove(entry);
            App.Db.SaveChanges();
            NavigationService?.Navigate(new ClassementPage());
        }
    }
}
