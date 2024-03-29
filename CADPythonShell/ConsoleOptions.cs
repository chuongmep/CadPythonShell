﻿// Copyright (c) 2010 Joe Moorhouse

using ICSharpCode.AvalonEdit;
using PythonConsoleControl;
using System.ComponentModel;
using FontFamily = System.Windows.Media.FontFamily;

namespace CADPythonShell
{
    public class ConsoleOptions
    {
        private TextEditor textEditor;
        private PythonConsolePad pad;

        public ConsoleOptions(PythonConsolePad pad)
        {
            this.textEditor = pad.Control;
            this.pad = pad;
        }

        [DefaultValue(false)]
        public bool ShowSpaces
        {
            get { return textEditor.TextArea.Options.ShowSpaces; }
            set { textEditor.TextArea.Options.ShowSpaces = value; }
        }

        [DefaultValue(false)]
        public bool ShowTabs
        {
            get { return textEditor.TextArea.Options.ShowTabs; }
            set { textEditor.TextArea.Options.ShowTabs = value; }
        }

        [DefaultValue(false)]
        public bool AllowScrollBelowDocument
        {
            get { return textEditor.TextArea.Options.AllowScrollBelowDocument; }
            set { textEditor.TextArea.Options.AllowScrollBelowDocument = value; }
        }

        [DefaultValue("Consolas")]
        public string FontFamily
        {
            get { return textEditor.TextArea.FontFamily.ToString(); }
            set { textEditor.TextArea.FontFamily = new FontFamily(value); }
        }

        [DefaultValue(12.0)]
        public double FontSize
        {
            get { return textEditor.TextArea.FontSize; }
            set { textEditor.TextArea.FontSize = value; }
        }

        [DefaultValue(false)]
        public bool FullAutocompletion
        {
            get { return pad.Console.AllowFullAutocompletion; }
            set { pad.Console.AllowFullAutocompletion = value; }
        }

        [DefaultValue(true)]
        public bool CtrlSpaceAutocompletion
        {
            get { return pad.Console.AllowCtrlSpaceAutocompletion; }
            set { pad.Console.AllowCtrlSpaceAutocompletion = value; }
        }
    }
}