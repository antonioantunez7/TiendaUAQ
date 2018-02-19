using System;
using System.Collections.Generic;
using System.Diagnostics;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class SubdepartamentosView : ContentPage
    {
        List<Subdepartamentos> subdepartamentos;
        public SubdepartamentosView(int cveDepartamento,string descDepartamento,string url_portada)
        {
            InitializeComponent();
            cargaSubdepartamentos(cveDepartamento,descDepartamento,url_portada);
            url_portada_departamento = new Image(){Source = url_portada};
        }

        public void cargaSubdepartamentos(int cveDepartamento, string descDepartamento,string url_portada){
            etiquetaCargando.Text = "Cargando subdepartamentos, por favor espere...";
            vistaSubdepartamentos.Content = etiquetaCargando;
            Device.BeginInvokeOnMainThread(async () =>
            {
                RestClient cliente = new RestClient();
                var subdepartamentosX = await cliente.GetSubdepartamentos<ListaSubdepartamentos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tblsubdepartamentos/departamento/"+cveDepartamento);
                Debug.Write(subdepartamentosX);
                if (subdepartamentosX != null)
                {
                    int totalRegistros = subdepartamentosX.listaSubdepartamentos.Count;
                    if (totalRegistros > 0)
                    {
                        subdepartamentos = new List<Subdepartamentos>();
                        for (var i = 0; i < totalRegistros; i++){ 
                            subdepartamentos.Add(new Subdepartamentos
                            {
                                cveSubdepartamento = subdepartamentosX.listaSubdepartamentos[i].cveSubdepartamento,
                                descSubdepartamento = subdepartamentosX.listaSubdepartamentos[i].descSubdepartamento
                            });
                        }
                        ListaSubdeptos.ItemsSource = subdepartamentos;
                        var imagen = new Image()
                        {
                            Source = url_portada,
                            WidthRequest = 500,
                            BackgroundColor = Color.FromHex("EC7063"),
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        };
                        var stacklayout1 = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children = {
                                imagen
                            }
                        };
                        var stacklayout2 = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children = {
                                ListaSubdeptos
                            }
                        };
                        var stacklayoutPrincipal = new StackLayout()
                        {
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children = {
                                stacklayout1,
                                stacklayout2
                            }
                        };
                        vistaSubdepartamentos.Content = stacklayoutPrincipal;
                    } else{
                        etiquetaCargando.Text = "No existen subdepartamentos relacionados a este departamento.";
                        vistaSubdepartamentos.Content = etiquetaCargando;     
                    }
                }
                else
                {
                    etiquetaCargando.Text = "Error de conexión.";
                    vistaSubdepartamentos.Content = etiquetaCargando;
                }
            });
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

        public Image url_portada_departamento { get; }
    }
}
