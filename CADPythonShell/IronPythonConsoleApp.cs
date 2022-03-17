using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using Orientation = System.Windows.Controls.Orientation;

namespace CADPythonShell
{
    public class IronPythonConsoleApp
    {
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
                RibbonTab rtab = ribbon.FindTab("PythonShell");
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }
                rtab = new RibbonTab();
                rtab.Title = "Python Shell";
                rtab.Id = "PythonShell";
                //Add the Tab
                ribbon.Tabs.Add(rtab);
                addContent(rtab);
            }
        }
        private void addContent(RibbonTab rtab)
        {
            rtab.Panels.Add(AddOnePanel());
        }
        static RibbonPanel AddOnePanel()
        {
            //https://forums.autodesk.com/t5/net/create-custom-ribbon-tab-and-buttons-for-autocad-mechanical-2011/td-p/2834343
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Cad Python Shell";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;
            //Create a Command Item that the Dialog Launcher can use,
            // for this test it is just a place holder.
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
            rb2.Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Settings-16.png");
            rb2.LargeImage = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Settings-32.png");
            
            rb2.CommandHandler = new RelayCommand(new ConfigureCommand().Execute);
            //Add the Button to the Tab
            rps.Items.Add(rb2);
            return rp;
        }
    }
}
