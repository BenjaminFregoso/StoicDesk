using StoicDesk;
using System;
using System.Windows;
using System.Windows.Threading;

namespace StoicDesk
{
    public partial class MainWindow : Window
    {
        private readonly QuoteService _quoteService = new QuoteService();
        private DispatcherTimer _quoteTimer;
        private DispatcherTimer _clockTimer;
        private bool _cargando = false;

        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += (s, e) => RepositionWindow();
        }

        private void RepositionWindow()
        {
            var workArea = SystemParameters.WorkArea;
            this.Left = workArea.Left + (workArea.Width - this.ActualWidth) / 2;
            this.Top = workArea.Top + (workArea.Height - this.ActualHeight) / 2;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Incrustar la ventana en el escritorio
            //DesktopHelper.EmbedToDesktop(this);

            // 2. Centrarla en la pantalla
            PositionCenterScreen();

            // 3. Cargar la primera frase
            await LoadQuoteAsync();

            // 4. Timer para actualizar la frase cada 5 minutos
            _quoteTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(5)
            };
            _quoteTimer.Tick += async (s, args) => await LoadQuoteAsync();
            _quoteTimer.Start();
        }

        private async void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Evitar doble carga si ya hay una petición en curso
            if (_cargando) return;

            await LoadQuoteAsync();

            // Opcional: feedback visual breve
            this.Opacity = 0.6;
            await System.Threading.Tasks.Task.Delay(120);
            this.Opacity = 1.0;
        }

        private async System.Threading.Tasks.Task LoadQuoteAsync()
        {
            if (_cargando) return;
            _cargando = true;

            try
            {
                var quote = await _quoteService.GetRandomQuoteAsync();

                if (quote == null)
                {
                    QuoteRun.Text = "Sin conexión";
                    AuthorRun.Text = "";
                    return;
                }

                QuoteRun.Text = quote.Frase;
                AuthorRun.Text = $"  — {quote.Autor}";
            }
            finally
            {
                _cargando = false;
            }
        }
        private void PositionCenterScreen()
        {
            var workArea = SystemParameters.WorkArea;

            // El texto nunca podrá superar el 80% del ancho de la pantalla
            // (menos los 10px de margen a cada lado)
            QuoteText.MaxWidth = (workArea.Width * 0.8) - 20;

            // Forzar el recálculo de layout para tener ActualWidth/ActualHeight reales
            this.UpdateLayout();

            this.Left = workArea.Left + (workArea.Width - this.ActualWidth) / 2;
            this.Top = workArea.Top + (workArea.Height - this.ActualHeight) / 2;
        }
    }
}