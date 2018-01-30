using System;
using Xamarin.Forms;
namespace TiendaUAQ.Models
{
    public class Menu
    {
        Image image;
        public Menu()
        {
            image = new Image
            {
                HeightRequest = 5,
                WidthRequest = 5,
            };
            image.Opacity = 0.5;
        }
        public int id{ get; set;}
        public string titulo{ get; set;}
        public string detalle{ get; set;}
        public ImageSource icono{ get { return image.Source; }set { image.Source = value; }}
    }
}
