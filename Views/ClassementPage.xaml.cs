using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;
using Microsoft.EntityFrameworkCore;

namespace guinea_football_club_erp.Views;

public partial class ClassementPage : Page
{
    private List<ClassementEntree> _all = [];
    private List<ClassementEntree> _filtered = [];
    private int _page = 1;
    private const int PageSize = 15;

    public ClassementPage()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        _all = App.Db.Classements.Include(c => c.Club)
                     .OrderBy(c => c.Position).ToList();
        _filtered = _all;
        _page = 1;
        Refresh();
    }

    private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        var q = TbSearch.Text.Trim().ToLower();
        _filtered = string.IsNullOrEmpty(q)
            ? _all
            : _all.Where(c => (c.Club?.Nom?.ToLower().Contains(q) ?? false)
                            || c.Competition.ToLower().Contains(q)
                            || c.Saison.ToLower().Contains(q)).ToList();
        _page = 1;
        Refresh();
    }

    private void Refresh()
    {
        int total = _filtered.Count;
        int pages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        _page = Math.Clamp(_page, 1, pages);
        DgClassement.ItemsSource = _filtered.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
        TbPageInfo.Text = $"Page {_page} / {pages}  ({total} entrée{(total > 1 ? "s" : "")})";
        BtnPrev.IsEnabled = _page > 1;
        BtnNext.IsEnabled = _page < pages;
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e) { _page--; Refresh(); }
    private void BtnNext_Click(object sender, RoutedEventArgs e) { _page++; Refresh(); }

    private void Dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DgClassement.SelectedItem is ClassementEntree entry)
            NavigationService?.Navigate(new ClassementFormPage(entry));
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new ClassementFormPage(null));
}
