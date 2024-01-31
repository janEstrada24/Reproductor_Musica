﻿using ReproductorMusicaComponentLibrary.Classes;
using ReproductorMusicaComponentLibrary.ConnexioAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaulerDeControlRM.EditaPages
{
    /// <summary>
    /// Lógica de interacción para PageEditaAlbum.xaml
    /// </summary>
    public partial class PageEditaAlbum : Page
    {
        // RegExp que comprova que nomes hi hagin numeros
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private List<Album> llistaAlbums = new List<Album>();
        private string TitolAlbum;
        private string AnyAlbum;

        public PageEditaAlbum(string Titol, string Any)
        {
            this.TitolAlbum = Titol;
            this.AnyAlbum = Any;
            InitializeComponent();
            LblAlbumTitle.Content = "Àlbum: " + this.TitolAlbum;
            LblAlbumYear.Content = "Any: " + this.AnyAlbum;
            this.ObtenirAlbums(this.TitolAlbum, this.AnyAlbum);
            this.GetIDsCancons();
        }
        
        
        /// <summary>
        /// Obtenim tots els Albums Amb un Titol i Any en concret
        /// </summary>
        private async void ObtenirAlbums(string TitolAlbum, string AnyAlbum)
        {
            this.llistaAlbums = await CA_Album.GetAlbumsByTitolAndAnyAsync(TitolAlbum, AnyAlbum);
            CanconsAlbum.ItemsSource = this.llistaAlbums;
            CanconsAlbum.AutoGenerateColumns = false;

            DataGridTextColumn idCancoColumn = new DataGridTextColumn();
            idCancoColumn.Header = "IDCanco";
            idCancoColumn.Binding = new System.Windows.Data.Binding("IDCanco");
            CanconsAlbum.Columns.Add(idCancoColumn);

            DataGridTextColumn nomCancoColumn = new DataGridTextColumn();
            nomCancoColumn.Header = "Nom";
            nomCancoColumn.Binding = new System.Windows.Data.Binding("CancoObj.Nom");
            CanconsAlbum.Columns.Add(nomCancoColumn);

            DataGridTemplateColumn eliminarCancoColumn = new DataGridTemplateColumn();
            eliminarCancoColumn.Header = "Eliminar Cançó";
            FrameworkElementFactory btnEliminar = new FrameworkElementFactory(typeof(Button));
            btnEliminar.SetValue(Button.ContentProperty, "Eliminar");
            btnEliminar.AddHandler(Button.ClickEvent, new RoutedEventHandler(btEliminar_Click));
            eliminarCancoColumn.CellTemplate = new DataTemplate() { VisualTree = btnEliminar };
            CanconsAlbum.Columns.Add(eliminarCancoColumn);
        }

        private async void GetIDsCancons()
        {
            List<Canco> cancons = await CA_Canco.GetCanconsAsync();
            List<string> idsCancons = new List<string>();

            for (int i = 0; i < cancons.Count; i++)
            {
                idsCancons.Add(cancons[i].IDCanco);
            }

            comboBoxCancons.ItemsSource = idsCancons;
        }


        private async void btEliminar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null && btn.DataContext is Album album) {
                await CA_Album.DeleteAlbumAsync(album.Titol, album.Any, album.IDCanco);
                this.ObtenirAlbums(this.TitolAlbum, this.AnyAlbum);
            }
        }

        private async void btOk_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxCancons.SelectedItem != null)
            {
                Album album = new Album();
                album.Titol = this.TitolAlbum;
                album.Any = int.Parse(this.AnyAlbum);
                album.IDCanco = comboBoxCancons.SelectedItem.ToString();
                await CA_Album.PostAlbumAsync(album);
                this.ObtenirAlbums(this.TitolAlbum, this.AnyAlbum);
                MessageBox.Show("Cançó afegida a l'Àlbum CORRECTAMENT!");
            } else
            {
                MessageBox.Show("ERROR! \n Cal que especifiquis quina canco vols afegir.");
            }
        }
    }
}