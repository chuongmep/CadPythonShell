using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using Orientation = System.Windows.Controls.Orientation;

namespace CADPythonShell
{
    public class IronPythonConsoleApp
    {
        public const string RibbonTitle = "Python Shell";
        public const string RibbonId = "PythonShell";

        [CommandMethod("InitPythonConsole")]
        public void Execute()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                RibbonTab rtab = ribbon.FindTab(RibbonId);
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }

                rtab = new RibbonTab();
                rtab.Title = RibbonTitle;
                rtab.Id = RibbonId;
                ribbon.Tabs.Add(rtab);
                AddContentToTab(rtab);
            }
        }

        private void AddContentToTab(RibbonTab rtab)
        {
            rtab.Panels.Add(AddPanelOne());
            rtab.Panels.Add(AddPanelTwo());
        }

        private static RibbonPanel AddPanelOne()
        {
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Cad Python Shell";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;
            RibbonButton rci = new RibbonButton();
            rci.Name = "Python Shell Console";
            rps.DialogLauncher = rci;
            //create button1
            var addinAssembly = typeof(IronPythonConsoleApp).Assembly;
            RibbonButton btnPythonShell = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = "Run CPS",
                ShowText = true,
                Text = "Run CPS",
                Description = "Start Write Python Console\nCommand: PythonShellConsole",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.Python-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Python-32.png"),
                CommandHandler = new RelayCommand(new IronPythonConsoleCommand().Execute)
            };
            rps.Items.Add(btnPythonShell);
            //create button2
            RibbonButton btnConfig = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = "Configure CPS",
                ShowText = true,
                Text = "Configure CPS",
                Description = "Configure Cad Python Shell\nCommand: PythonShellSetting",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.Settings-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Settings-32.png"),
                CommandHandler = new RelayCommand(new ConfigureCommand().Execute)
            };
            //Add the Button to the Tab
            rps.Items.Add(btnConfig);
            return rp;
        }

        private static RibbonPanel AddPanelTwo()
        {
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Lookup";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;
            RibbonButton rci = new RibbonButton();
            rci.Name = "Lookup";
            rps.DialogLauncher = rci;
            //create button1
            var addinAssembly = typeof(IronPythonConsoleApp).Assembly;
            RibbonButton btnSnoop = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = "Snoop",
                ShowText = true,
                Text = "Snoop",
                Description = "Start snoop object inside Autocad \n Command: Snoop",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.Find-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Find-32.png"),
                CommandHandler = new RelayCommand(new SnoopCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Snoop",
            };
            rps.Items.Add(btnSnoop);
            return rp;
        }
    }
}