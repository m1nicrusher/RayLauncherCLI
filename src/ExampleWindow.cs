using CmlLib.Core.Auth.Microsoft;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using XboxAuthNet.Game.Msal;

namespace AdvancedMinecraftLauncher
{
    // Defines a top-level window with border and title
    public class ExampleWindow : Window
    {
        private Label loginMsButton;
        private Label loginOfflineButton;
        private ColorScheme scheme;
        private ColorScheme buttonScheme;
        private Label artLabel;
        private const string ART = " ________  _____ ______   ___          \r\n|\\   __  \\|\\   _ \\  _   \\|\\  \\         \r\n\\ \\  \\|\\  \\ \\  \\\\\\__\\ \\  \\ \\  \\        \r\n \\ \\   __  \\ \\  \\\\|__| \\  \\ \\  \\       \r\n  \\ \\  \\ \\  \\ \\  \\    \\ \\  \\ \\  \\____  \r\n   \\ \\__\\ \\__\\ \\__\\    \\ \\__\\ \\_______\\\r\n    \\|__|\\|__|\\|__|     \\|__|\\|_______|";
        public ExampleWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Title = "Advanced Minecraft Launcher";

            scheme = new ColorScheme();
            scheme.Normal = new Terminal.Gui.Attribute(Terminal.Gui.Color.Gray, Terminal.Gui.Color.Black);
            scheme.HotNormal = new Terminal.Gui.Attribute(Terminal.Gui.Color.Gray, Terminal.Gui.Color.Black);
            scheme.Focus = new Terminal.Gui.Attribute(Terminal.Gui.Color.Black, Terminal.Gui.Color.DarkGray);
            scheme.HotFocus = new Terminal.Gui.Attribute(Terminal.Gui.Color.Black, Terminal.Gui.Color.DarkGray);
            scheme.Disabled = new Terminal.Gui.Attribute(Terminal.Gui.Color.DarkGray, Terminal.Gui.Color.Black);

            buttonScheme = new ColorScheme();
            buttonScheme.Normal = new Terminal.Gui.Attribute(Terminal.Gui.Color.Gray, Terminal.Gui.Color.Green);
            buttonScheme.Focus = new Terminal.Gui.Attribute(Terminal.Gui.Color.White, Terminal.Gui.Color.BrightGreen);

            ColorScheme = scheme;

            artLabel = new Label()
            {
                Text = ART,
                X = Pos.Center(),
            };

            loginMsButton = new Label()
            {
                Text = "Login with Microsoft/Xbox",
                X = Pos.Center(),
                Y = Pos.Bottom(artLabel) + 3,
                Width = 30,
                Height = 3,
                ColorScheme = buttonScheme,
                TextAlignment = TextAlignment.Centered,
                VerticalTextAlignment = VerticalTextAlignment.Middle,
                CanFocus = true,
            };

            loginMsButton.Clicked += LoginMsButton_Clicked;
            loginMsButton.KeyPress += (KeyEventEventArgs e) => { if (e.KeyEvent.Key == Key.Enter) { LoginMsButton_Clicked(); e.Handled = true; } };

            loginOfflineButton = new Label()
            {
                Text = "Offline mode",
                X = Pos.Center(),
                Y = Pos.Bottom(loginMsButton) + 1,
                Width = 30,
                Height = 3,
                ColorScheme = buttonScheme,
                TextAlignment = TextAlignment.Centered,
                VerticalTextAlignment = VerticalTextAlignment.Middle,
                CanFocus = true,
            };

            loginOfflineButton.Clicked += LoginOfflineButton_Clicked;
            loginOfflineButton.KeyPress += (KeyEventEventArgs e) => { if (e.KeyEvent.Key == Key.Enter) { LoginOfflineButton_Clicked(); e.Handled = true; } };

            this.Add(artLabel, loginMsButton, loginOfflineButton);
        }

        private void LoginOfflineButton_Clicked()
        {
            Dialog d = new Dialog()
            {
                Title = "Offline Mode",
                Modal = true,
            };

            Label l = new Label() { Text = "Username:" };
            d.Add(l);
            d.AddButton(new Button() { Text="OK"});

            Application.Run(d);

            //Launcher.LoginOffline();
        }

        private async void LoginMsButton_Clicked()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();
            IPublicClientApplication app = await MsalClientHelper.BuildApplicationWithCache("85264be9-aa50-4fec-a1fd-b400fddb45fb");
            var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
            authenticator.AddMsalOAuth(app, msal => msal.Interactive());
            authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
            authenticator.AddForceJEAuthenticator();
            var session = await authenticator.ExecuteForLauncherAsync();
        }

        int i = 0;
    }
}
