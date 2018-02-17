using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using UIKit;

namespace TiendaUAQ.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            //Llamada a paypal
            CrossPayPalManager.Init(
                new PayPalConfiguration(PayPalEnvironment.Sandbox, "ARIzxJa3tOHO9mLo7FB3YTp3n4Q28zh1kiMPDFie9lmMIvPWk5gN-ardzfJcpPzJ0NHpi2UWBZT6GbuV")
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
                }
            );

            return base.FinishedLaunching(app, options);
        }
    }
}
