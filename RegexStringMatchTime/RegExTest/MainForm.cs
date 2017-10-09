using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegExTest
{
    public partial class MainForm : Form
    {
        CancellationTokenSource ctSource = new CancellationTokenSource();

        public MainForm()
        {
            InitializeComponent();
        }

        private async void btnLoadResolutionMessages_Click(object sender, EventArgs e)
        {
            KBService service = new KBService();

            try
            {
                lblStatus.Text = "Loading resolution messages...";
                List<Error> parsedErrors = await Task.Factory.StartNew<List<Error>>(() =>
                    {
                        return service.CreateErrorsResolutionData(ErrorType.All, ctSource.Token);
                    }, ctSource.Token);

                grdErrorsViewer.DataSource = parsedErrors;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lblStatus.Text = "Loading completed...";

            }

        }
    }
}
