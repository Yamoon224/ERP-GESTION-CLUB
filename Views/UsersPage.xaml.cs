using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class UsersPage : Page
{
    private List<User> _all = [];
    private List<User> _filtered = [];
    private int _page = 1;
    private const int PageSize = 15;

    public UsersPage()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        _all = App.Db.Users.OrderBy(u => u.NomComplet).ToList();
        _filtered = _all;
        _page = 1;
        Refresh();
    }

    private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        var q = TbSearch.Text.Trim().ToLower();
        _filtered = string.IsNullOrEmpty(q)
            ? _all
            : _all.Where(u => u.NomComplet.ToLower().Contains(q)
                            || u.Username.ToLower().Contains(q)
                            || (u.Email?.ToLower().Contains(q) ?? false)
                            || u.Role.ToString().ToLower().Contains(q)).ToList();
        _page = 1;
        Refresh();
    }

    private void Refresh()
    {
        int total = _filtered.Count;
        int pages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        _page = Math.Clamp(_page, 1, pages);
        DgUsers.ItemsSource = _filtered.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
        TbPageInfo.Text = $"Page {_page} / {pages}  ({total} utilisateur{(total > 1 ? "s" : "")})";
        BtnPrev.IsEnabled = _page > 1;
        BtnNext.IsEnabled = _page < pages;
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e) { _page--; Refresh(); }
    private void BtnNext_Click(object sender, RoutedEventArgs e) { _page++; Refresh(); }

    private void Dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DgUsers.SelectedItem is User u)
            NavigationService?.Navigate(new UserFormPage(u));
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new UserFormPage(null));
}
