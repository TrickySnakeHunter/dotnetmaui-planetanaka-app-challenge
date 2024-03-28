namespace Planetanaka.ViewModels;

// Import necessary namespaces

// Define the BaseViewModel class
public partial class BaseViewModel : ObservableObject
{
    // Define a private field for tracking whether the app is busy
    [ObservableProperty]
    private bool isBusy;

    // Constructor for BaseViewModel
    public BaseViewModel()
    {
        // Subscribe to the ConnectivityChanged event
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    // Destructor for BaseViewModel
    ~BaseViewModel()
    {
        // Unsubscribe from the ConnectivityChanged event
        Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
    }

    // Event handler for when the connectivity changes
    private static async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        // Define various messages for different connectivity statuses
        TimeSpan unlimitedDuration = TimeSpan.MaxValue;
        const string constrainedInternet = "Internet access is available but is limited.";
        const string lostInternet = "Without connection";
        const string activeWifi = "Online again on Wifi";
        const string activeEthernet = "Online again om Ethernet";
        const string activeCell = "Online again on Cell";
        const string activeBluetooth = "Online again on Bluetooth";

        // Define options for the Snackbar when internet is not available
        SnackbarOptions snackbarWithoutInternet = new()
        {
            BackgroundColor = Color.FromRgba("#212121"),
            TextColor = Colors.White,
        };

        // Define options for the Snackbar when internet is available
        SnackbarOptions snackbarWithInternet = new()
        {
            BackgroundColor = Color.FromRgba("#2CA641"),
            TextColor = Colors.White
        };

        // Check the connectivity status and display appropriate Snackbar messages
        if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
        {
            await Snackbar.Make(constrainedInternet).Show();
        }
        else if (e.NetworkAccess != NetworkAccess.Internet)
        {
            await Snackbar.Make(
                lostInternet,
                actionButtonText: string.Empty,
                duration: unlimitedDuration,
                visualOptions: snackbarWithoutInternet).Show();
        }

        // Loop through all connection profiles and display appropriate Snackbar messages
        foreach (var item in e.ConnectionProfiles)
        {
            switch (item)
            {
                case ConnectionProfile.Bluetooth:
                    await Snackbar.Make(activeBluetooth,
                        actionButtonText: string.Empty,
                        visualOptions: snackbarWithInternet).Show();
                    break;
                case ConnectionProfile.Cellular:
                    await Snackbar.Make(activeCell,
                        actionButtonText: string.Empty,
                        visualOptions: snackbarWithInternet).Show();
                    break;
                case ConnectionProfile.Ethernet:
                    await Snackbar.Make(activeEthernet,
                        actionButtonText: string.Empty,
                        visualOptions: snackbarWithInternet).Show();
                    break;
                case ConnectionProfile.WiFi:
                    await Snackbar.Make(activeWifi,
                        actionButtonText: string.Empty,
                        visualOptions: snackbarWithInternet).Show();
                    break;
                default:
                    break;
            }
        }
    }
}
