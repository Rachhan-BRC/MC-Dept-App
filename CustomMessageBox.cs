using System;
using System.Drawing;
using System.Windows.Forms;

namespace MachineDeptApp
{
    public partial class CustomMessageBox : Form
    {
        public enum CustomDialogResult
        {
            DataResult,
            DataImport,
            None
        }

        public CustomDialogResult SelectedOption { get; private set; } = CustomDialogResult.None;

        // Parameterless constructor
        public CustomMessageBox()
        {
            InitializeComponent();
            SetupControls("Choose an option:", "Export Data");
        }

        // Constructor with message and title
        public CustomMessageBox(string message, string title)
        {
            InitializeComponent();
            SetupControls(message, title);
        }

        private void SetupControls(string message, string title)
        {
            this.Text = title;
            this.Width = 300;
            this.Height = 150;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblMessage = new Label
            {
                Text = message,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            Button btnResult = new Button
            {
                Text = "Data Result",
                Left = 50,
                Top = 60,
                Width = 80
            };

            Button btnImport = new Button
            {
                Text = "Data Import",
                Left = 150,
                Top = 60,
                Width = 80
            };

            btnResult.Click += (s, e) => { SelectedOption = CustomDialogResult.DataResult; this.Close(); };
            btnImport.Click += (s, e) => { SelectedOption = CustomDialogResult.DataImport; this.Close(); };

            this.Controls.Add(lblMessage);
            this.Controls.Add(btnResult);
            this.Controls.Add(btnImport);
        }

        // Static helper method for one-line usage
        public static CustomDialogResult ShowDialog(string message, string title)
        {
            using (CustomMessageBox box = new CustomMessageBox(message, title))
            {
                box.ShowDialog();
                return box.SelectedOption;
            }
        }
    }
}
