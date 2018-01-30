using System;
using System.Collections.Generic;
using TiendaUAQ.Models;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class SubdepartamentosView : ContentPage
    {
        public SubdepartamentosView(int cveDepartamento,string descDepartamento)
        {
            InitializeComponent();
            cargaSubdepartamentos(cveDepartamento,descDepartamento);
        }

        public void cargaSubdepartamentos(int cveDepartamento, string descDepartamento){
            List<Subdepartamentos> subdepartamentos = new List<Subdepartamentos>
            {
                new Subdepartamentos{cveSubdepartamento = 1, descSubdepartamento = "Subdepartamento 1"},
                new Subdepartamentos{cveSubdepartamento = 2, descSubdepartamento = "Subdepartamento 2"},
                new Subdepartamentos{cveSubdepartamento = 3, descSubdepartamento = "Subdepartamento 3"}
            };
            ListaSubdeptos.ItemsSource = subdepartamentos;
        }

        async void ListaSubdepartamentos_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var subdepartamento = e.SelectedItem as Models.Subdepartamentos;
            if (subdepartamento != null)
            {
                int cveSubdepartamento = subdepartamento.cveSubdepartamento;
                await Navigation.PushAsync(new ProductosView(cveSubdepartamento));
            }
            ListaSubdeptos.SelectedItem = null;//Para que automaticamente se deseleccione el elemento
        }
    }
}
