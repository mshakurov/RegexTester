using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Helpers;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;

namespace regexTester
{
    public partial class FormMatchesViewer : Form
    {
        string setsFileName = @".\Ssettings.xml";
        Sets sets;
        public FormMatchesViewer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setsFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Path.GetFileName(setsFileName));
            sets = SerializeHelper.LoadOrDefault<Sets>(setsFileName, new Sets() { RGHistItems = new RGHistItem[0] });
            edTemplate.Items.AddRange(sets.RGHistItems);
            if (sets.RGHistItems.Length > 0)
                edTemplate.SelectedItem = sets.RGHistItems[0];
        }

        public void ShowMatches(string text, Match[] matches)
        {
            try
            {
                edText.Text = text;
                edText.SelectAll();
                edText.SelectionBackColor = edText.BackColor;
                edText.SelectionColor = edText.ForeColor;
                edText.DeselectAll();
                edMatchesLog.Text = "";
                var point = new Point(edText.SelectionStart, edText.SelectionLength);

                var state = new RegexMatchState();

                State = state;

                state.Matches = matches;

                if (state.Matches.Length > 0)
                    state.CurMatchIdx = 0;

                edMatchesLog.Text = $"Matches: {state.Matches.Length}";

                var sbMatchesLog = new StringBuilder();

                if (state.Matches.Length > 0)
                {
                    var sw = Stopwatch.StartNew();
                    var breaked = false;

                    edText.SuspendLayout();
                    this.SuspendLayout();
                    try
                    {
                        foreach (var m in state.Matches)
                        {
                            edText.Select(m.Index, m.Length);
                            edText.SelectionColor = Color.Yellow;
                            edText.SelectionBackColor = Color.Red;

                            if (sw.ElapsedMilliseconds >= 5000)
                            {
                                if (breaked = (MessageBox.Show(this, "Слишком много совпадений!\r\nПродолжить следующие 5 сек добавлять совпадения?", "Совпадения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No))
                                    break;
                                sw.Restart();
                            }
                        }
                    }
                    finally
                    {
                        edText.ResumeLayout();
                        this.ResumeLayout();
                    }

                    int idx = -1;
                    foreach (var m in state.Matches)
                    {
                        sbMatchesLog.AppendLine($"M: {++idx}. idx:{m.Index}, len:{m.Length}");
                        sbMatchesLog.AppendLine($"- Groups ({m.Groups.Count}):");
                        sbMatchesLog.AppendLine($"{string.Join(Environment.NewLine, m.Groups.OfType<Group>().Select((g, i) => $"- - {i + 1}. idx:{g.Index}, [{(g.Success ? "v" : " ")}] len:{g.Length}, val:{g.Value}"))}");
                    }
                    edMatchesLog.Text += Environment.NewLine + sbMatchesLog.ToString();

                    if (point.X >= 0)
                    {
                        edText.SelectionStart = point.X;
                        edText.SelectionLength = point.Y;
                        TextSelectionChanged();
                    }
                    else
                    {
                        state.CurMatchIdx = 0;

                        SelectMatch();
                        edText.Focus();
                    }
                }
                // else
                //     ShowError("Не найдено", "Regex");

            }
            catch (Exception ex)
            {
                ShowError(string.Format("### Ошибка:\r\n{0}", ex.GetFullMessage()), "Исполнение");
            }

        }

        public string PrepareText(string text)
        {
            var temp = edText.Text;
            edText.Text = text;
            text = edText.Text;
            edText.Text = temp;
            return text;
        }

        public string GetText()
        {
            return edText.Text;
        }

        public void SetText(string text)
        {
            edText.Text = text;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(edTemplate.Text))
            {
                MessageBox.Show("Не задан шаблон");
                return;
            }
            if (string.IsNullOrWhiteSpace(edText.Text))
            {
                MessageBox.Show("Не задан текст");
                return;
            }

            var item = sets.GetOrAddRGHistItem(edTemplate.Text);
            if (item.IsNew)
                edTemplate.Items.Insert(0, item);
            edTemplate.SelectedItem = item;

            try { SerializeHelper.Save(setsFileName, sets); } catch { }

            ShowMatches(edText.Text, Regex.Matches(edText.Text, edTemplate.Text, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
                | (cbSingleLine.Checked ? RegexOptions.Singleline : RegexOptions.None)
                | (cbMultiLine.Checked ? RegexOptions.Multiline : RegexOptions.None)
                ).OfType<Match>().Where(m => m.Success).ToArray());
        }

        private void ShowError(string text, string caption)
        {
            this.Invoke(new MethodInvoker(() =>
              MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
             ));
        }

        private RegexMatchState State
        {
            get
            {
                var state = (RegexMatchState)edText.Tag;
                if (state == null || state.Matches == null || state.Matches.Length == 0)
                    return null;
                return state;
            }
            set
            {
                edText.Tag = value;
            }
        }

        private void btnMatchPrev_Click(object sender, EventArgs e)
        {
            if (State == null) return;

            var selStart = edText.SelectionStart;
            var m = State.Matches[State.CurMatchIdx];

            if (selStart < m.Index + m.Length && State.CurMatchIdx > 0)
                State.CurMatchIdx--;

            SelectMatch();
            edText.Focus();
        }

        private void btnMatchNext_Click(object sender, EventArgs e)
        {
            if (State == null) return;

            var selStart = edText.SelectionStart;
            var m = State.Matches[State.CurMatchIdx];

            if (selStart >= m.Index && State.CurMatchIdx < State.Matches.Length - 1)
                State.CurMatchIdx++;

            SelectMatch();
            edText.Focus();
        }

        private void btnTextClearFormat_Click(object sender, EventArgs e)
        {
            ClearFormat();
        }

        private void ClearFormat()
        {
            edText.SelectionBackColor = edText.BackColor;
            edText.SelectionColor = edText.ForeColor;
            edText.Focus();
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            using (var fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            using (var sr = new StreamReader(fs))
            {
                State = null;
                edText.Text = sr.ReadToEnd();
            }
        }

        private void buttonMatchesLogSync_Click(object sender, EventArgs e)
        {
            SelectMatchLog();
            edMatchesLog.Focus();
            edMatchesLog.ScrollToCaret();
        }

        private void edText_SelectionChanged(object sender, EventArgs e)
        {
            TextSelectionChanged();
        }

        private void TextSelectionChanged()
        {
            var selStart = edText.SelectionStart;

            var line = edText.GetLineFromCharIndex(selStart);
            var col = selStart - edText.GetFirstCharIndexFromLine(line);
            var curMI = State?.CurMatchIdx;

            ValidateCurIndex();

            SelectMatchLog();
            UpdateNavButtons();

            edTextState.Text = $"r:{line}:c:{col}, sel:{selStart}({edText.SelectionLength}), match:{State?.CurMatchIdx}(prev:{curMI})";
        }

        private void ValidateCurIndex()
        {
            var selStart = edText.SelectionStart;

            if (State == null) return;

            var matchIndex = State.Matches.ToList().FindIndex(m => InMatch(m, selStart));
            if (matchIndex < 0)
            {
                var matchIndex1 = matchIndex = State.Matches.ToList().FindLastIndex(m => m.Index <= selStart);
                if (matchIndex1 < 0 || !InMatch(State.Matches[matchIndex1], selStart))
                {
                    var matchIndex2 = matchIndex = State.Matches.ToList().FindIndex(m => m.Index >= selStart);
                    if (matchIndex2 < 0 || !InMatch(State.Matches[matchIndex2], selStart))
                    {
                        if (matchIndex1 >= 0 && matchIndex2 < 0)
                            matchIndex = matchIndex1;
                        else
                        if (matchIndex1 < 0 && matchIndex2 >= 0)
                            matchIndex = matchIndex2;
                        else
                        if (matchIndex1 >= 0 && matchIndex2 >= 0)
                        {
                            var m1 = State.Matches[matchIndex1];
                            var m2 = State.Matches[matchIndex2];
                            matchIndex = selStart - m1.Index - m1.Length + 1 < m2.Index - selStart ? matchIndex1 : matchIndex2;
                        }
                    }

                }
            }
            if (matchIndex >= 0)
                State.CurMatchIdx = matchIndex;
        }

        private static bool InMatch(Match m, int selStart)
        {
            return m.Index <= selStart && m.Index + m.Length > selStart;
        }

        private void SelectMatch()
        {
            if (State == null) return;

            var match = State.Matches[State.CurMatchIdx];

            edText.SelectionChanged -= edText_SelectionChanged;
            try
            {
                edText.Select(match.Index, match.Length);
                edText.ScrollToCaret();
            }
            finally
            {
                edText.SelectionChanged += edText_SelectionChanged;
                TextSelectionChanged();
            }
        }

        private void UpdateNavButtons()
        {
            if (State == null)
            {
                btnMatchPrev.Enabled = btnMatchNext.Enabled = false;
                return;
            }

            var selStart = edText.SelectionStart;
            var m1 = State.Matches.First();
            var m2 = State.Matches.Last();

            btnMatchPrev.Enabled = selStart >= m1.Index + m1.Length;
            btnMatchNext.Enabled = selStart < m2.Index;
        }

        private void SelectMatchLog()
        {
            if (State == null) return;

            // sbMatchesLog.AppendLine($"M: {++idx}. idx:{m.Index}, len:{m.Length}");
            var m = State.Matches[State.CurMatchIdx];
            var idx = edMatchesLog.Text.IndexOf($"M: {State.CurMatchIdx}. idx:{m.Index}, len:{m.Length}");
            if (idx >= 0)
            {
                edMatchesLog.SelectionStart = idx;
                idx = edMatchesLog.Text.IndexOf("\r\n", idx);
                if (idx < 0) idx = edMatchesLog.TextLength;
                edMatchesLog.SelectionLength = idx - edMatchesLog.SelectionStart;
                edMatchesLog.ScrollToCaret();
            }
        }

        private class RegexMatchState
        {
            //public string Text;
            public Match[] Matches;
            public int CurMatchIdx = -1;
        }


    }
}
