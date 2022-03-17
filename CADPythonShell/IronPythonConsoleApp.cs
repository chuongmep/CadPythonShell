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
        void CreateRibbon()
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
            rtab.Panels.Add(AddOnePanel());
        }
        static RibbonPanel AddOnePanel()
        {
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Cad Python Shell";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;
            RibbonButton rci = new RibbonButton();
            rci.Name = "Python Shell Console";
            rps.DialogLauncher = rci;
            //create button1
            RibbonButton rb = new RibbonButton();
            rb.Orientation = Orientation.Vertical;
            rb.AllowInStatusBar = true;
            rb.Size = RibbonItemSize.Large;
            rb.Name = "Run CPS";
            rb.ShowText = true;
            rb.Text = "Run CPS";
            rb.Description = "Start Write Python Console\nCommand: PythonShellConsole";
            var addinAssembly = typeof(IronPythonConsoleApp).Assembly;
            rb.Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Python-16.png");
            rb.LargeImage = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Python-32.png");
            rb.CommandHandler = new RelayCommand(new IronPythonConsoleCommand().Execute);
            rps.Items.Add(rb);
            //create button2
            RibbonButton rb2 = new RibbonButton();
            rb2.Orientation = Orientation.Vertical;
            rb2.AllowInStatusBar = true;
            rb2.Size = RibbonItemSize.Large;
            rb2.Name = "Configure CPS";
            rb2.ShowText = true;
            rb2.Text = "Configure CPS";
            rb2.Description = "Configure Cad Python Shell\nCommand: PythonShellSetting";
            rb2.Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Settings-16.png");
            rb2.LargeImage = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Settings-32.png");
            
            rb2.CommandHandler = new RelayCommand(new ConfigureCommand().Execute);
            //Add the Button to the Tab
            rps.Items.Add(rb2);
            return rp;
        }
    }
}
