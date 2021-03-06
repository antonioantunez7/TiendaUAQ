﻿using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PayPal.Forms;
using PayPal.Forms.Abstractions;

namespace TiendaUAQ.Droid
{
    [Activity(Label = "Tienda UAQ", Icon = "@drawable/iconoTienda", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            //Llamada a paypal
            CrossPayPalManager.Init(
                //new PayPalConfiguration(PayPalEnvironment.Sandbox, "ARIzxJa3tOHO9mLo7FB3YTp3n4Q28zh1kiMPDFie9lmMIvPWk5gN-ardzfJcpPzJ0NHpi2UWBZT6GbuV")
                //new PayPalConfiguration(PayPalEnvironment.Production, "AZ4CjEeIYvI55U8oiRyPbfaCXXzijjtRskzgf5PGYxnTr62cbocIvncFUATGBXRhfkUXzaubXSEQ2nd5") antunez produccion
                new PayPalConfiguration(PayPalEnvironment.Production, "AaTHcSUCOzUWTn__0okD1xm3yuGUZyg6TeRY5FXPRVUwn8g14SL75xsEBgfaUYHbpD7OGy4GBteIvpxJ") //andrades produccion
                {
                    StoreUserData = false,
                    AcceptCreditCards = true,
                    MerchantName = "Test Store",
                    MerchantPrivacyPolicyUri = "https://www.example.com/privacy",
                    MerchantUserAgreementUri = "https://www.example.com/legal",
                    ShippingAddressOption = ShippingAddressOption.Provided,//Direccion que provee el usuario
                    //ShippingAddressOption = ShippingAddressOption.PayPal,
                    Language = "es",
                    PhoneCountryCode = "52"
                },
                this
            );


            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PayPalManagerImplementation.Manager.Destroy();
        }
    }
}
