namespace MauiBleTest;

public partial class App : Application
{
	/// <summary>BLEをコントロールするクラス</summary>
	static public BLEUse.Veepeak _bleUse;
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
