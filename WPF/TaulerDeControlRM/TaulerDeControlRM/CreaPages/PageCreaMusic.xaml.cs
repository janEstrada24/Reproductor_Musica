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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaulerDeControlRM.CreaPages
{
    /// <summary>
    /// Interaction logic for PageCreaMusic.xaml
    /// </summary>
    public partial class PageCreaMusic : Page
    {
        public PageCreaMusic()
        {
            InitializeComponent();
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtNom.Text.ToString() == string.Empty) {
                MessageBox.Show("ERROR! \n Has de ficar un nom vàlid abans de pujar el Músic.");
            } 
            else if (this.txtNom.Text.ToString().Length > 20) {
                MessageBox.Show("ERROR! \n El nom del músic és massa llarg.");
            }
            else {
                Music music = new Music();
                music.Nom = this.txtNom.Text;
                music.LGrups = null;
                music.LTocar = null;
                CA_Music.PostMusicAsync(music);
                this.txtNom.Text = string.Empty;
                MessageBox.Show("Músic creat CORRECTAMENT!");
            }
        }
    }
}