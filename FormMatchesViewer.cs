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
using System.Threading.Tasks;

namespace regexTester
{
  public partial class FormMatchesViewer : Form
  {
    string setsFileName = @"Settings.xml";
    Sets sets;
    public FormMatchesViewer()
    {
      InitializeComponent();

      //   edMatchesLog.ScrollBars = ScrollBars.Both;
      //   edMatchesLog.WordWrap = false;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      setsFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Path.GetFileName(setsFileName));
	  //MessageBox.Show(setsFileName);
	  //if (!File.Exists(setsFileName)) try { SerializeHelper.Save(setsFileName, new Sets() { RGHistItems = new RGHistItem[0] }); } catch (Exception ex){ MessageBox.Show(ex.ToString()); }
      sets = SerializeHelper.LoadOrDefault<Sets>(setsFileName, new Sets() { RGHistItems = new RGHistItem[0] });
      edTemplate.Items.AddRange(sets.RGHistItems);
      if (sets.RGHistItems.Length > 0)
        edTemplate.SelectedItem = sets.RGHistItems[0];

      bsMatchGroups.DataSource = Enumerable.Range(1, 100).Select(i => new { i, i100 = i * 100 }).ToList();
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
        edMatchesInfo.Text = "";
        bsMatchGroups.DataSource = null;
        var point = new Point(edText.SelectionStart, edText.SelectionLength);

        var state = new RegexMatchState();

        State = state;

        state.Matches = matches;

        if (state.Matches.Length > 0)
          state.CurMatchIdx = 0;

        edMatchesInfo.Text = edMatchesLog.Text = $"Matches: {state.Matches.Length}";

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

          var table = new DataTable();
          table.Columns.Add("M", typeof(int));
          table.Columns.Add("M?", typeof(bool));
          table.Columns.Add("MIdx", typeof(int));
          table.Columns.Add("MLen", typeof(int));
          state.Matches[0].Groups.OfType<Group>().Select((g, i) => (g, i)).ToList().ForEach(g =>
          {
            table.Columns.Add($"G{g.i}Idx", typeof(int));
            table.Columns.Add($"G{g.i}Len", typeof(int));
            table.Columns.Add($"G{g.i}Val", typeof(string));
          });
          int idx = -1;
          foreach (var m in state.Matches)
          {
            ++idx;
            var objs = new object[] { idx, m.Success, m.Index, m.Length }.Concat(m.Groups.OfType<Group>().Select((g, i) => (g, i)).SelectMany(g => new object[] { g.g.Index, g.g.Length, g.g.Value })).ToArray();
            table.Rows.Add(objs);
            sbMatchesLog.AppendLine($"M: {idx}. idx:{m.Index}, len:{m.Length}");
            sbMatchesLog.AppendLine($"- Groups ({m.Groups.Count}):");
            sbMatchesLog.AppendLine($"{string.Join(Environment.NewLine, m.Groups.OfType<Group>().Select((g, i) => $"- - {i + 1} '{g.Name}'. idx:{g.Index}, [{(g.Success ? "v" : " ")}] len:{g.Length}, val:{g.Value}"))}");
          }
          edMatchesLog.Text += Environment.NewLine + sbMatchesLog.ToString();
          
          edMatchesInfo.Text += ( Environment.NewLine + $"Group count: {state.Matches[0].Groups.Count}" );
          for ( int iGrp = 1; iGrp < state.Matches[0].Groups.Count; iGrp++ )
          {
            var groupValues = state.Matches.OfType<Match>().Select( m => m.Groups[iGrp].Value ).GroupBy( groupValue => groupValue, ( groupValue, coll ) => new { groupValue, count = coll.Count(), uniqueValues = coll.GroupBy(ug => ug, (ug, ugColl) => $"{ug} ({ugColl.Count():#,0})").ToArray() }, StringComparer.InvariantCultureIgnoreCase ).ToArray();
            edMatchesInfo.Text += ( Environment.NewLine + $"- Group[{iGrp}]['{state.Matches[0].Groups[iGrp].Name}']: Count CI: {groupValues.Count():#,0}, CC: {groupValues.Sum(gv => gv.uniqueValues.Length):#,0}. {( groupValues.Length > 10 ? $"(first {10}):" : string.Empty )}" );
            edMatchesInfo.Text += ( Environment.NewLine );
            edMatchesInfo.Text += ( string.Join(Environment.NewLine, groupValues.Take(10).Select(g => $"- - '{g.groupValue}' ({g.count:#,0})")) );
            edMatchesInfo.Text += ( Environment.NewLine + "----------");
          }

          gridMatches.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
          var selTab = tabControlMacthes.SelectedTab;
          gridMatches.DataBindingComplete += DataBindingComplete;
          //   gridMatches.SelectionChanged += SelectionChanged;
          tabControlMacthes.SelectedTab = pgMatchesTable;
          bsMatchGroups.DataSource = table;
          StartResize();
          void DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
          {
            gridMatches.DataBindingComplete -= DataBindingComplete;
            StartResize();
          }
          //   void SelectionChanged(object sender, EventArgs e)
          //   {
          //     gridMatches.SelectionChanged -= SelectionChanged;
          //     StartResize();
          //   }
          void StartResize()
          {
            Task.Factory.StartNew(() => this.Invoke(new MethodInvoker(() =>
            {
              //   gridMatches.Refresh();
              //   gridMatches.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
              //   gridMatches.Refresh();
              var cols = gridMatches.Columns.OfType<DataGridViewColumn>().ToDictionary(col => col, col => Math.Min(150, col.Width));
              gridMatches.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
              //   gridMatches.Refresh();
              cols.ToList().ForEach(kv => kv.Key.Width = kv.Value);
              tabControlMacthes.SelectedTab = selTab;
            })));
          }

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
      //var debug_matches = Regex.Matches( edText.Text, @"(?<month>\w+?) (?<day>\d\d?) (?<time>\d\d:\d\d:\d\d) kernel(?<sourceIndex>\[[^\[\]]*?\])?: (?<action>[^ ]+?) (?:(?<act2>(:?(?<act2Name>[^ =]+?)=(?<act2Val>[^ =]+?[ &])?))|(?<act2>[^ ]+?)[ $])?", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant );

      //var debug_acts = string.Join("", Enumerable.Range( 2, 15 ).Select( i => @$"(?:(?<act{i}>(:?(?<act{i}Name>[^ =$]+?)=(?<act{i}Val>[^ =$]+?[ $\b])?))|(?<act{i}>[^ $]+?)[ $\b])[ $]?" ));

      //var debug_acts = string.Join( "", Enumerable.Range( 2, 15 ).Select( i => @$"(?: (?<act{i}>(:?(?<act{i}Name>[^ =]+)=(?<act{i}Val>[^ =]+$?)?))|(?<act{i}>[^ ]+?$?))" ) );

      //edTemplate.Text = @"(?<month>\w+?) (?<day>\d\d?) (?<time>\d\d:\d\d:\d\d) kernel(?<sourceIndex>\[[^\[\]]*?\])?: (?<action>[^ $\b]+?) " + debug_acts;

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

    private void btnTextToMatchLogSync_Click(object sender, EventArgs e)
    {
      SelectMatchLog();
      edMatchesLog.Focus();
      edMatchesLog.ScrollToCaret();
      SelectMatchGroup();
    }

    private void btnMatchLogToTextSync_Click(object sender, EventArgs e)
    {
      if (State == null) return;

      if (tabControlMacthes.SelectedTab == this.pgMatchesText)
      {
        if (edMatchesLog.TextLength == 0)
          return;

        var line = edMatchesLog.GetLineFromCharIndex(edMatchesLog.SelectionStart);
        var lastLine = edMatchesLog.GetLineFromCharIndex(edMatchesLog.TextLength);
        var idx1 = edMatchesLog.GetFirstCharIndexFromLine(line);
        var idx2 = line >= lastLine ? edMatchesLog.TextLength : edMatchesLog.GetFirstCharIndexFromLine(line + 1) - 1;
        var idxRn = edMatchesLog.Text.IndexOfAny(new[] { '\r', '\n' }, idx1);
        if (idxRn >= 0)
          idx2 = idxRn;

        while (line >= 0)
        {
          var lineText = edMatchesLog.Text.Substring(idx1, idx2 - idx1 + 1);
          var m = Regex.Match(lineText, @"M: (\d+). idx:\d+, len:\d+");
          if (m.Success)
          {
            edMatchesLog.Select(idx1, idx2 - idx1 + 1);

            var mIdx = int.Parse(m.Groups[1].Value.Replace(" ", ""));
            State.CurMatchIdx = mIdx;
            SelectMatch();
            edText.Focus();
            break;
          }
          if (--line < 0)
            break;
          idx1 = edMatchesLog.GetFirstCharIndexFromLine(line);
          idx2 = line >= lastLine ? edMatchesLog.TextLength : edMatchesLog.GetFirstCharIndexFromLine(line + 1) - 1;
          idxRn = edMatchesLog.Text.IndexOfAny(new[] { '\r', '\n' }, idx1);
          if (idxRn >= 0)
            idx2 = idxRn;
        }
      }
      else
      {
        SyncMatchGroupCellToText();
      }
    }

    private void SyncMatchGroupCellToText()
    {
      var cell = gridMatches.CurrentCell;
      if (cell == null) return;
      State.CurMatchIdx = (int)(gridMatches.Rows[cell.RowIndex].DataBoundItem as DataRowView).Row["M"];
      SelectMatch();
      edText.Focus();
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
      SelectMatchGroup();
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

    private void SelectMatchGroup()
    {
      if (State == null) return;

      if (gridMatches.Rows.Count == 0) return;

      var m = State.Matches[State.CurMatchIdx];

      var cell = gridMatches.CurrentCell;
      var row = gridMatches.Rows.OfType<DataGridViewRow>().FirstOrDefault(r => ((int)(r.DataBoundItem as DataRowView).Row["MIdx"]) == m.Index);
      gridMatches.CurrentCell = row.Cells[cell?.ColumnIndex ?? 0];
    }

    private void btnCopyToClipBrdClick(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(edTemplate.Text))
        return;
      Clipboard.SetText("@\"" + edTemplate.Text.Replace(@"\""", @"\""""") + "\"");
    }

    private void CopyWOnewlines_Click(object sender, EventArgs e)
    {
      var sels = gridMatches.SelectedCells.OfType<DataGridViewCell>().Select(c => new Point(c.RowIndex, c.ColumnIndex)).ToArray();
      if (sels.Length == 0)
        return;

      var ptTopLeft = new Point(sels.Min(p => p.X), sels.Min(p => p.Y));
      var ptBottomRight = new Point(sels.Max(p => p.X), sels.Max(p => p.Y));
      var log = new StringBuilder();
      for (int row = ptTopLeft.X; row <= ptBottomRight.X; row++)
      {
        if (log.Length > 0)
          log.AppendLine();
        for (int col = ptTopLeft.Y; col <= ptBottomRight.Y; col++)
        {
          log.Append((gridMatches.Rows[row].Cells[col].Value ?? "").ToString().Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("\t", "  "));
          log.Append('\t');
        }
      }
      Clipboard.SetText(log.ToString());
    }

    private void gridMatchesCellDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
    {
      SyncMatchGroupCellToText();
    }

    private class RegexMatchState
    {
      //public string Text;
      public Match[] Matches;
      public int CurMatchIdx = -1;
    }


  }
}
