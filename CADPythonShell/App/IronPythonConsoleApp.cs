using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using CADPythonShell.Command;
using Orientation = System.Windows.Controls.Orientation;

namespace CADPythonShell.App
{
    public class IronPythonConsoleApp : ICadCommand
    {
        public const string RibbonTitle = "Python Shell";
        public const string RibbonId = "PythonShell";

        [CommandMethod("InitPythonConsole")]
        public override void Execute()
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
                Name = nameof(MgdDbgAction.Snoop),
                ShowText = true,
                Text = "Snoop",
                Description = "Start snoop object inside Autocad \n Command: Snoop",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.Find-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Find-32.png"),
                CommandHandler = new RelayCommand(new SnoopCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Quick Snoop An objects Sample",
            };
            RibbonButton btnSnoopDB = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = nameof(MgdDbgAction.SnoopDB),
                ShowText = true,
                Text = "Snoop\nDatabase",
                Description = $"Start snoop Database inside Autocad \n Command: {nameof(MgdDbgAction.SnoopDB)}",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.database-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.database-32.png"),
                CommandHandler = new RelayCommand(new SnoopDBCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Snoop Database",
            };
            RibbonButton btnSnoopEd = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = nameof(MgdDbgAction.SnoopEd),
                ShowText = true,
                Text = "Snoop\nEditor",
                Description = $"Start snoop Snoop Editor inside Autocad \n Command: {nameof(MgdDbgAction.SnoopEd)}",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.editor-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.editor-32.png"),
                CommandHandler = new RelayCommand(new SnoopEditorCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Snoop Editor",
            };
            RibbonButton btnSnooByHandle = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = nameof(MgdDbgAction.SnoopByHandle),
                ShowText = true,
                Text = "Snoop\nBy Handle",
                Description = $"Start snoop Snoop Editor inside Autocad \n Command: {nameof(MgdDbgAction.SnoopByHandle)}",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.handle-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.handle-32.png"),
                CommandHandler = new RelayCommand(new SnoopByHandleCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Snoop By Handle",
            };
            RibbonButton btnSnoopEntities = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = nameof(MgdDbgAction.SnoopEnts),
                ShowText = true,
                Text ="Snoop\nEntities",
                Description = $"Start snoop Snoop Entities inside Autocad \n Command: {nameof(MgdDbgAction.SnoopEnts)}",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.snoopentities-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.snoopentities-32.png"),
                CommandHandler = new RelayCommand(new SnoopEntitiesCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Snoop By Entities",
            };
            RibbonButton btnSnoopEntitiesNested = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = nameof(MgdDbgAction.SnoopNEnts),
                ShowText = true,
                Text = "Snoop\nNested Entities",
                Description = $"Start snoop Snoop Entities inside Autocad \n Command: {nameof(MgdDbgAction.SnoopNEnts)}",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.snoopentitiesnes-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.snoopentitiesnes-32.png"),
                CommandHandler = new RelayCommand(new SnoopEntitiesNestedCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Snoop By Nested Entities",
            };
            RibbonButton btnSnoopEvents = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = nameof(MgdDbgAction.SnoopEvents),
                ShowText = true,
                Text = "Snoop\nEvents",
                Description = $"Start snoop Snoop Events inside Autocad \n Command: {nameof(MgdDbgAction.SnoopEvents)}",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.events-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.events-32.png"),
                CommandHandler = new RelayCommand(new SnoopEventsCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Snoop Events",
            };
            RibbonButton btnSnoopTestFramework = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = nameof(MgdDbgAction.SnoopTests),
                ShowText = true,
                Text = "Snoop\nTest Framework",
                Description = $"Start snoop Snoop Test Framework inside Autocad \n Command: {nameof(MgdDbgAction.SnoopTests)}",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly,
                    "CADPythonShell.Resources.test-16.png"),
                LargeImage =
                    CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.test-32.png"),
                CommandHandler = new RelayCommand(new TestFrameworkCommand().Execute),
                AllowInToolBar = true,
                KeyTip = "Snoop Test Framework",
            };
            rps.Items.Add(btnSnoop);
            rps.Items.Add(btnSnoopDB);
            rps.Items.Add(btnSnoopEd);
            rps.Items.Add(btnSnooByHandle);
            rps.Items.Add(btnSnoopEntities);
            rps.Items.Add(btnSnoopEntitiesNested);
            rps.Items.Add(btnSnoopEvents);
            rps.Items.Add(btnSnoopTestFramework);
            return rp;
        }
    }
}