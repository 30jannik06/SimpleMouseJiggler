using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimpleMouseJiggler
{
    public partial class MainForm : Form
    {
        private Button toggleButton;
        private Label statusLabel;
        private TrackBar intervalSlider;
        private Label intervalLabel;
        private CheckBox zenModeCheckBox;
        private System.Windows.Forms.Timer jiggleTimer;
        private bool running = true;
        private bool zenMode = false;
        private readonly Random random = new Random();

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        private const uint MOUSEEVENTF_MOVE = 0x0001;

        public MainForm()
        {
            // === Fenster-Setup ===
            Text = "🖱️ Simple Mouse Jiggler";
            Size = new Size(370, 300);
            Font = new Font("Segoe UI", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            try
            {
                this.Icon = new Icon("jiggler.ico");
            }
            catch
            {
                this.Icon = SystemIcons.Application;
            }

            // === Button ===
            toggleButton = new Button
            {
                Text = "Stop",
                Width = 100,
                Height = 35,
                Top = 20,
                Left = 125
            };
            toggleButton.Click += ToggleButton_Click;

            // === Statuslabel ===
            statusLabel = new Label
            {
                Text = "Status: ✅ Aktiv",
                AutoSize = true,
                Top = 70,
                Left = 115
            };

            // === Intervall-Label ===
            intervalLabel = new Label
            {
                Text = "Intervall: 2 Sekunden",
                AutoSize = true,
                Top = 110,
                Left = 105
            };

            // === Intervall-Slider ===
            intervalSlider = new TrackBar
            {
                Minimum = 1,
                Maximum = 30,
                TickFrequency = 1,
                Value = 2,
                Width = 250,
                Top = 140,
                Left = 50
            };
            intervalSlider.Scroll += (s, e) =>
            {
                intervalLabel.Text = $"Intervall: {intervalSlider.Value} Sekunden";
                jiggleTimer.Interval = intervalSlider.Value * 1000;
            };

            // === Zen-Modus Checkbox ===
            zenModeCheckBox = new CheckBox
            {
                Text = "Zen-Modus (unsichtbar)",
                AutoSize = true,
                Top = 190,
                Left = 90
            };
            zenModeCheckBox.CheckedChanged += (s, e) =>
            {
                zenMode = zenModeCheckBox.Checked;
            };

            // === Timer ===
            jiggleTimer = new System.Windows.Forms.Timer();
            jiggleTimer.Interval = 2000;
            jiggleTimer.Tick += JiggleTimer_Tick;
            jiggleTimer.Start();

            // === Tray ===
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Start", null, (s, e) => StartJiggler());
            trayMenu.Items.Add("Stop", null, (s, e) => StopJiggler());
            trayMenu.Items.Add("Beenden", null, (s, e) =>
            {
                trayIcon?.Visible = false;
                Application.Exit();
            });

            try
            {
                trayIcon = new NotifyIcon
                {
                    Icon = new Icon("jiggler.ico"),
                    Text = "Simple Mouse Jiggler",
                    Visible = false,
                    ContextMenuStrip = trayMenu
                };
            }
            catch
            {
                trayIcon = new NotifyIcon
                {
                    Icon = SystemIcons.Application,
                    Text = "Simple Mouse Jiggler",
                    Visible = false,
                    ContextMenuStrip = trayMenu
                };
            }

            trayIcon.DoubleClick += (s, e) =>
            {
                Show();
                WindowState = FormWindowState.Normal;
                trayIcon?.Visible = false;
            };

            // === Minimieren-Event ===
            this.Resize += (s, e) =>
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    Hide();
                    trayIcon?.Visible = true;
                }
            };

            // === Controls hinzufügen ===
            Controls.Add(toggleButton);
            Controls.Add(statusLabel);
            Controls.Add(intervalLabel);
            Controls.Add(intervalSlider);
            Controls.Add(zenModeCheckBox);
        }

        private void ToggleButton_Click(object? sender, EventArgs e)
        {
            if (running) StopJiggler();
            else StartJiggler();
        }

        private void StartJiggler()
        {
            running = true;
            jiggleTimer?.Start();
            toggleButton.Text = "Stop";
            statusLabel.Text = "Status: ✅ Aktiv";
        }

        private void StopJiggler()
        {
            running = false;
            jiggleTimer?.Stop();
            toggleButton.Text = "Start";
            statusLabel.Text = "Status: ❌ Gestoppt";
        }

        private void JiggleTimer_Tick(object? sender, EventArgs e)
        {
            if (zenMode)
            {
                mouse_event(MOUSEEVENTF_MOVE, 0, 0, 0, 0);
            }
            else
            {
                int dx = random.Next(-3, 4);
                int dy = random.Next(-3, 4);
                Point pos = Cursor.Position;
                Cursor.Position = new Point(pos.X + dx, pos.Y + dy);
            }
        }
    }
}
