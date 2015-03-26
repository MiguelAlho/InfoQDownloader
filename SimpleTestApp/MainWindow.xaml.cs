using AssetDownloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleTestApp
{
    /* IDEAS
     * 
     * 
     * [10:44:31] Antonio Madeira: ya.
[10:45:18] Antonio Madeira: couple ux notes:
[10:45:53] Antonio Madeira: 1-o log é fixe,  mas ficava mais fixe se o scroll o acompanhasse. alternativamente, podias adicionar as linhas no topo e log'ar de baixo para cima.
[10:46:47] Antonio Madeira: 2-browse button na localização de destino - fazes isso em 5 linhas ou assim e poupa algum trabalho ao user.
[10:47:05] Antonio Madeira: 3-test url button na text box do url.
[10:47:53] Antonio Madeira: (até podes reutilizar o código que tens no botão e só ligar o "download" quando a caixa de texto tiver um url válido.)
     * also a "TADA!" window'd be nice - I just noticed it's finished
     * 
     * 
    */



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //setup dependencies

            if (string.IsNullOrWhiteSpace(basePath.Text))
            {
                Result.Text = "no basepath!";
                return;
            }

            if (string.IsNullOrWhiteSpace(Url.Text))
            {
                Result.Text = "no url!";
                return;
            }

            //composition root
            IMediaRepositoryFactory repofactory = new InfoQMediaRepositoryFactory(basePath.Text);
            IAssetRequester requester = new InfoQAssetRequester();
            IContentParser parser = new InfoQContentParser();

            Result.Text = ("starting...");

            try
            {
                IProgress<string> progress = new Progress<string>((s) => Result.Text += string.Format("{0}\n", s));
                
                InfoQDownloader downloader = new InfoQDownloader(repofactory, requester, parser, progress);
                bool done = await downloader.DownloadPresentation(Url.Text);
            }
            catch (Exception ex)
            {
                Result.Text += string.Format("\n{0} - {1}", DateTime.Now.ToString(), ex.Message);
            }

            Result.Text += ("...done");
            
        }


        
    }
}
