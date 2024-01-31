﻿using ReproductorMusicaComponentLibrary.Classes;
using ReproductorMusicaComponentLibrary.ConnexioAPI;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TaulerDeControlRM.CreaPages
{
    /// <summary>
    /// Lógica de interacción para PageCreaAlbum.xaml
    /// </summary>
    public partial class PageCreaAlbum : Page
    {
        // RegExp que comprova que nomes hi hagin numeros
        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        public PageCreaAlbum()
        {
            InitializeComponent();
            getIDsCancons();
        }

        private async void getIDsCancons()
        {
            List<Canco> cancons = await CA_Canco.GetCanconsAsync();
            List<string> idsCancons = new List<string>();

            for (int i = 0; i < cancons.Count; i++)
            {
                idsCancons.Add(cancons[i].IDCanco);
            }

            comboBoxCancons.ItemsSource = idsCancons;
        }

        private async void reiniciarValors()
        {
            txtAlbumTitle.Text = string.Empty;
            txtAlbumYear.Text = string.Empty;
            comboBoxCancons.SelectedItem = null;
        }

        private async void btOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtAlbumTitle.Text.ToString() == string.Empty
                || txtAlbumYear.Text.ToString() == string.Empty
                || _regex.IsMatch(this.txtAlbumYear.Text)
                || comboBoxCancons.SelectedItem == null)
            {
                MessageBox.Show("ERROR! \n Emplena els camps abans de pujar l'àlbum i que estiguin en el format correcte.");
            }
            else if (txtAlbumTitle.Text.ToString().Length > 20)
            {
                MessageBox.Show("ERROR! \n El títol de l'Àlbum és massa llarg.");
            }
            else
            {
                Album album = new Album();
                album.Titol = this.txtAlbumTitle.Text.ToString();
                album.Any = int.Parse(this.txtAlbumYear.Text);
                album.IDCanco = comboBoxCancons.SelectedItem.ToString();
                await CA_Album.PostAlbumAsync(album);
                reiniciarValors();
                MessageBox.Show("Àlbum creat CORRECTAMENT!");
            }
        }
    }
}