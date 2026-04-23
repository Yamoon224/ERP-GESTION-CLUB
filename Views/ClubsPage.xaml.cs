using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class ClubsPage : Page
{
    private List<Club> _all = [];
    private List<Club> _filtered = [];
    private int _page = 1;
    private const int PageSize = 15;

    public ClubsPage()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        _all = App.Db.Clubs.ToList();
        _filtered = _all;
        _page = 1;
        Refresh();
    }

    private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        var q = TbSearch.Text.Trim().ToLower();
        _filtered = string.IsNullOrEmpty(q)
            ? _all
            : _all.Where(c => c.Nom.ToLower().Contains(q)
                            || c.Acronyme.ToLower().Contains(q)
                            || c.Ville.ToLower().Contains(q)).ToList();
        _page = 1;
        Refresh();
    }

    private void Refresh()
    {
        int total = _filtered.Count;
        int pages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        _page = Math.Clamp(_page, 1, pages);
        DgClubs.ItemsSource = _filtered.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
        TbPageInfo.Text = $"Page {_page} / {pages}  ({total} entrée{(total > 1 ? "s" : "")})";
        BtnPrev.IsEnabled = _page > 1;
        BtnNext.IsEnabled = _page < pages;
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e) { _page--; Refresh(); }
    private void BtnNext_Click(object sender, RoutedEventArgs e) { _page++; Refresh(); }

    private void DgClubs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DgClubs.SelectedItem is Club club)
            NavigationService?.Navigate(new ClubFormPage(club));
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new ClubFormPage(null));
}
