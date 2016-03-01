using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FieldEngineerLite.ViewModels
{
	public class LoginViewModel //: ViewModelBase
	{

		// Injected authenticator service
		private IAuthenticatorService _authSvc;

		#region Bindable Commands

		private Command _loginCmd;
		public Command LoginCommand
		{
			get
			{
				return _loginCmd ?? (_loginCmd = new Command(async () => {
					await LoginExecuteCommand();
					App.LoginManager.ShowMainPage();
				}));
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="LoginViewModel"/> class.
		/// </summary>
		/// <param name="authService">The authentication service.</param>
		public LoginViewModel(IAuthenticatorService authService)
		{
			_authSvc = authService;
		}

		/// <summary>
		/// Execute login command.
		/// </summary>
		/// <returns></returns>
		private async Task LoginExecuteCommand()
		{

			try
			{
				var user = await _authSvc.Authorize(MobileServiceAuthenticationProvider.Google);
			}
			catch (InvalidOperationException ex)
			{
				if (ex.Message.Contains("Authentication was canceled by the user")) { }
			}
			catch (Exception)
			{
				var page = new ContentPage();
				page.DisplayAlert("Error", "Error logging in. Please check connectivity and try again.", "OK");
			}
		}
	}
}
