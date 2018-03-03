using System;
using System.Collections.Generic;
using System.Diagnostics;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class DepartamentosView : ContentPage
    {
        private Grid gridDepartamentos = new Grid();
        public DepartamentosView()
        {
            InitializeComponent();
            mallaDepartamentos();
        }

        public void mallaDepartamentos()
        { 
            etiquetaCargando.Text = "Cargando departamentos, por favor espere...";
            vistaDepartamentos.Content = etiquetaCargando;
            Device.BeginInvokeOnMainThread(async () =>
            {
                RestClient cliente = new RestClient();
                var departamentos = await cliente.GetDepartamentos<ListaDepartamentos>("http://148.240.202.160:88/TiendaUAQWebservice/api/tbldepartamentos");
                Debug.Write(departamentos);
                if (departamentos != null)
                {
                    int totalRegistros = departamentos.listaDepartamentos.Count;
                    //int totalRegistros = 11;
                    int maximoColumnas = 2;
                    int auxColumnas = 0;
                    int renglones = 0;
                    if (totalRegistros > 0)
                    {
                        for (int i = 0; i < maximoColumnas; i++)
                        {
                            gridDepartamentos.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
                        }
                        for (int columnas = 0; columnas < totalRegistros; columnas++)
                        {
                            if (columnas == 0)
                            {
                                gridDepartamentos.RowDefinitions.Add(new RowDefinition() { Height = 150 });
                                auxColumnas = 0;
                            }
                            else if (auxColumnas / maximoColumnas == 1)
                            {//Si todavia faltan elementos 
                             //Crear renglon    
                                gridDepartamentos.RowDefinitions.Add(new RowDefinition() { Height = 150 });
                                renglones++;
                                auxColumnas = 0;
                            }
                            if (auxColumnas == maximoColumnas)
                            {
                                auxColumnas = 0;
                            }
                            else
                            {

                                //Crear el objeto a insertar
                                //int cveCategoria = categorias.listaCategorias[columnas].cveCategoria;
                                int cveDepartamento = departamentos.listaDepartamentos[columnas].cveDepartamento;
                                string descDepartamento = departamentos.listaDepartamentos[columnas].descDepartamento;
                                //string descCategoria = categorias.listaCategorias[columnas].descCategoria;
                                string url_portada = "http://148.240.202.160:88/" + departamentos.listaDepartamentos[columnas].url_portada;
                                //string url_portada = "http://www.generaccion.com/w/imagenes/galerias/7892-20_04_2012_15_02_14_1492029543.jpg";
                                Debug.Write(url_portada);
                                var imagen = new Image()
                                {
                                    Source = url_portada,
                                    //WidthRequest = 220,
                                    HeightRequest = 80,
                                    VerticalOptions = LayoutOptions.StartAndExpand,
                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                    Opacity = 0.8
                                };
                                //Se crea el evento del clic de la imagen
                                var tapGestureRecognizer = new TapGestureRecognizer();
                                tapGestureRecognizer.Tapped += (s, e) =>
                                {
                                    //imagen.Opacity = .5;
                                    cargaSubdepartamentos(cveDepartamento, descDepartamento, url_portada);
                                };
                                imagen.GestureRecognizers.Add(tapGestureRecognizer);
                                //gridCategorias.Children.Add(imagen, auxColumnas, renglones);

                                //Diseño nuevo
                                var stacklayout1 = new StackLayout
                                {
                                    //Orientation = StackOrientation.Horizontal,
                                    //HorizontalOptions = LayoutOptions.Center,
                                    Children = {
                                            imagen
                                        }
                                };

                                var label1 = new Label
                                {
                                    FontSize = 10,
                                    Text = "Label 1",
                                    TextColor = Color.Black,
                                    FontAttributes = FontAttributes.Bold,
                                    HorizontalOptions = LayoutOptions.Start,
                                    VerticalOptions = LayoutOptions.Center,
                                    WidthRequest = 150
                                };

                                var label2 = new Label
                                {
                                    FontSize = 10,
                                    Text = "Label 2",
                                    TextColor = Color.Gray,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                };

                                var label3 = new Label
                                {
                                    FontSize = 16,
                                    Text = descDepartamento,
                                    TextColor = Color.Gray,
                                    HorizontalOptions = LayoutOptions.Center,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalOptions = LayoutOptions.Center,
                                    VerticalTextAlignment = TextAlignment.Center
                                };

                                var stacklayout2 = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    HorizontalOptions = LayoutOptions.Center,
                                    Children = {
                                            //label1,
                                            //label2,
                                            label3
                                        }
                                };

                                var stacklayoutPrincipal = new StackLayout()
                                {
                                    Orientation = StackOrientation.Vertical,
                                    Children = {
                                            stacklayout1,
                                            stacklayout2
                                        }
                                };
                                var frame = new Frame()
                                {
                                    BackgroundColor = Color.FromHex("FBFBFB")
                                };
                                frame.Content = stacklayoutPrincipal;

                                gridDepartamentos.Children.Add(frame, auxColumnas, renglones);

                            }
                            auxColumnas++;
                        }
                        vistaDepartamentos.Content = gridDepartamentos;
                    } 
                    else
                    {
                        etiquetaCargando.Text = "No existen departamentos disponibles.";
                        vistaDepartamentos.Content = etiquetaCargando;
                    }

                } 
                else
                {
                    etiquetaCargando.Text = "Error de conexión.";
                    vistaDepartamentos.Content = etiquetaCargando;
                }
            });
        }

        private async void cargaSubdepartamentos(int cveDepartamento, string descDepartamento, string url_portada)
        {
            await Navigation.PushAsync(new SubdepartamentosView(cveDepartamento, descDepartamento, url_portada));
        }
    }
}
