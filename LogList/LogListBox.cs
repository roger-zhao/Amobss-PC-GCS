using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Text;

namespace MissionPlanner.LogList
{
    public partial class LogListBox : Form
    {
        public static ListBoxLog listBoxLog;
        public LogListBox(ListBox listBox1)
        {
            // InitializeComponent();

            listBoxLog = new ListBoxLog(listBox1);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBoxLog.Log(Level.Debug, "A debug level message");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            listBoxLog.Log(Level.Verbose, "A verbose level message");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBoxLog.Log(Level.Info, "A info level message");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            listBoxLog.Log(Level.Warning, "A warning level message");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            listBoxLog.Log(Level.Error, "A error level message");
        }
        private void button6_Click(object sender, EventArgs e)
        {
            listBoxLog.Log(Level.Critical, "A critical level message");
        }
        private void button7_Click(object sender, EventArgs e)
        {
            listBoxLog.Paused = !listBoxLog.Paused;
        }
    }

    public enum Level : int
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Verbose = 4,
        Debug = 5
    };
    public sealed class ListBoxLog : IDisposable
    {
        private const string DEFAULT_MESSAGE_FORMAT = "{1} [{6}] : {8}";
        private const int DEFAULT_MAX_LINES_IN_LISTBOX = 2000;

        private bool _disposed;
        private ListBox _listBox;
        private string _messageFormat;
        private int _maxEntriesInListBox;
        private bool _canAdd;
        private bool _paused;

        private void OnHandleCreated(object sender, EventArgs e)
        {
            _canAdd = true;
        }
        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            _canAdd = false;
        }
        private void DrawItemHandler(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();

                LogEvent logEvent = ((ListBox)sender).Items[e.Index] as LogEvent;

                // SafeGuard against wrong configuration of list box
                if (logEvent == null)
                {
                    logEvent = new LogEvent(Level.Critical, ((ListBox)sender).Items[e.Index].ToString());
                }

                Color color;
                switch (logEvent.Level)
                {
                    case Level.Critical:
                        color = Color.Red;
                        break;
                    case Level.Error:
                        color = Color.Red;
                        break;
                    case Level.Warning:
                        color = Color.Yellow;
                        break;
                    case Level.Info:
                        color = Color.White;
                        break;
                    case Level.Verbose:
                        color = Color.WhiteSmoke;
                        break;
                    default:
                        color = Color.Black;
                        break;
                }

                if (logEvent.Level == Level.Critical)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
                }
                e.Graphics.DrawString(FormatALogEventMessage(logEvent, _messageFormat), new Font("Lucida Console", 8.25f, FontStyle.Regular), new SolidBrush(color), e.Bounds);
            }
        }
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers == Keys.Control) && (e.KeyCode == Keys.C))
            {
                CopyToClipboard();
            }
        }
        private void CopyMenuOnClickHandler(object sender, EventArgs e)
        {
            CopyToClipboard();
        }
        private void CopyMenuPopupHandler(object sender, EventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            if (menu != null)
            {
                menu.MenuItems[0].Enabled = (_listBox.SelectedItems.Count > 0);
            }
        }

        private class LogEvent
        {
            public LogEvent(Level level, string message)
            {
                EventTime = DateTime.Now;
                Level = level;
                Message = message;
            }

            public readonly DateTime EventTime;

            public readonly Level Level;
            public readonly string Message;
        }
        private void WriteEvent(LogEvent logEvent)
        {
            if ((logEvent != null) && (_canAdd))
            {
                _listBox.BeginInvoke(new AddALogEntryDelegate(AddALogEntry), logEvent);
            }
        }
        private delegate void AddALogEntryDelegate(object item);
        private void AddALogEntry(object item)
        {
            _listBox.Items.Add(item);

            if (_listBox.Items.Count > _maxEntriesInListBox)
            {
                _listBox.Items.RemoveAt(0);
            }

            if (!_paused) _listBox.TopIndex = _listBox.Items.Count - 1;
        }
        private string LevelName(Level level)
        {
            switch (level)
            {
                case Level.Critical: return "严重";
                case Level.Error: return "错误";
                case Level.Warning: return "警告";
                case Level.Info: return "信息";
                case Level.Verbose: return "运行";
                case Level.Debug: return "调试";
                default: return string.Format("<value={0}>", (int)level);
            }
        }
        private string FormatALogEventMessage(LogEvent logEvent, string messageFormat)
        {
            string message = logEvent.Message;
            if (message == null) { message = "<NULL>"; }
            return string.Format(messageFormat,
                /* {0} */ logEvent.EventTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                /* {1} */ logEvent.EventTime.ToString("yyyy-MM-dd HH:mm:ss"),
                /* {2} */ logEvent.EventTime.ToString("yyyy-MM-dd"),
                /* {3} */ logEvent.EventTime.ToString("HH:mm:ss.fff"),
                /* {4} */ logEvent.EventTime.ToString("HH:mm:ss"),

                /* {5} */ LevelName(logEvent.Level)[0],
                /* {6} */ LevelName(logEvent.Level),
                /* {7} */ (int)logEvent.Level,

                /* {8} */ message);
        }
        private void CopyToClipboard()
        {
            if (_listBox.SelectedItems.Count > 0)
            {
                StringBuilder selectedItemsAsRTFText = new StringBuilder();
                selectedItemsAsRTFText.AppendLine(@"{\rtf1\ansi\deff0{\fonttbl{\f0\fcharset0 Courier;}}");
                selectedItemsAsRTFText.AppendLine(@"{\colortbl;\red255\green255\blue255;\red255\green0\blue0;\red218\green165\blue32;\red0\green128\blue0;\red0\green0\blue255;\red0\green0\blue0}");
                foreach (LogEvent logEvent in _listBox.SelectedItems)
                {
                    selectedItemsAsRTFText.AppendFormat(@"{{\f0\fs16\chshdng0\chcbpat{0}\cb{0}\cf{1} ", (logEvent.Level == Level.Critical) ? 2 : 1, (logEvent.Level == Level.Critical) ? 1 : ((int)logEvent.Level > 5) ? 6 : ((int)logEvent.Level) + 1);
                    selectedItemsAsRTFText.Append(FormatALogEventMessage(logEvent, _messageFormat));
                    selectedItemsAsRTFText.AppendLine(@"\par}");
                }
                selectedItemsAsRTFText.AppendLine(@"}");
                System.Diagnostics.Debug.WriteLine(selectedItemsAsRTFText.ToString());
                Clipboard.SetData(DataFormats.Rtf, selectedItemsAsRTFText.ToString());
            }

        }

        public ListBoxLog(ListBox listBox) : this(listBox, DEFAULT_MESSAGE_FORMAT, DEFAULT_MAX_LINES_IN_LISTBOX) { }
        public ListBoxLog(ListBox listBox, string messageFormat) : this(listBox, messageFormat, DEFAULT_MAX_LINES_IN_LISTBOX) { }
        public ListBoxLog(ListBox listBox, string messageFormat, int maxLinesInListbox)
        {
            _disposed = false;

            _listBox = listBox;
            _messageFormat = messageFormat;
            _maxEntriesInListBox = maxLinesInListbox;

            _paused = false;

            _canAdd = listBox.IsHandleCreated;

            _listBox.SelectionMode = SelectionMode.MultiExtended;

            _listBox.HandleCreated += OnHandleCreated;
            _listBox.HandleDestroyed += OnHandleDestroyed;
            _listBox.DrawItem += DrawItemHandler;
            _listBox.KeyDown += KeyDownHandler;

            MenuItem[] menuItems = new MenuItem[] { new MenuItem("Copy", new EventHandler(CopyMenuOnClickHandler)) };
            _listBox.ContextMenu = new ContextMenu(menuItems);
            _listBox.ContextMenu.Popup += new EventHandler(CopyMenuPopupHandler);

            _listBox.DrawMode = DrawMode.OwnerDrawFixed;
        }

        public void Log(string message) { Log(Level.Debug, message); }
        public void Log(string format, params object[] args) { Log(Level.Debug, (format == null) ? null : string.Format(format, args)); }
        public void Log(Level level, string format, params object[] args) { Log(level, (format == null) ? null : string.Format(format, args)); }
        public void Log(Level level, string message)
        {
            WriteEvent(new LogEvent(level, message));
        }

        public bool Paused
        {
            get { return _paused; }
            set { _paused = value; }
        }

        ~ListBoxLog()
        {
            if (!_disposed)
            {
                Dispose(false);
                _disposed = true;
            }
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }
        private void Dispose(bool disposing)
        {
            if (_listBox != null)
            {
                _canAdd = false;

                _listBox.HandleCreated -= OnHandleCreated;
                _listBox.HandleCreated -= OnHandleDestroyed;
                _listBox.DrawItem -= DrawItemHandler;
                _listBox.KeyDown -= KeyDownHandler;

                _listBox.ContextMenu.MenuItems.Clear();
                _listBox.ContextMenu.Popup -= CopyMenuPopupHandler;
                _listBox.ContextMenu = null;

                _listBox.Items.Clear();
                _listBox.DrawMode = DrawMode.Normal;
                _listBox = null;
            }
        }
    }
}
