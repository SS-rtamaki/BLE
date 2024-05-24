using System.Diagnostics;
namespace BLEApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		// count++;

		// if (count == 1)
		// 	CounterBtn.Text = $"Clicked {count} time";
		// else
		// 	CounterBtn.Text = $"Clicked {count} times";

		// SemanticScreenReader.Announce(CounterBtn.Text);

		try
		{
			var res = await MauiBleTest.ble.OBD2ConnectionManager.Instance.Test();

			foreach (var r in res)
			{
			Debug.WriteLine(r);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}

		return;
	}
}

