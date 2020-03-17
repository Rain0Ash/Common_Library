// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Common_Library.Localization;
using Common_Library.Utils;
using Common_Library.Utils.IO;
using Common_Library.Utils.OS;

namespace Common_Library.GUI.WinForms.Forms
{
    public sealed class MessageForm : ParentForm
    {
        private readonly List<Button> _buttons = new List<Button>();

        private static readonly Size IconSize = new Size(64, 64);

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static CultureStringsBase ThisLinkIsInvalidMessage = new CultureStringsBase(@"This link is invalid");

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static CultureStringsBase InvalidLinkMessage = new CultureStringsBase(@"Invalid link");

        public static DialogResult GetDialogResultOnException(Exception exception, String additionalText = null, String title = null,
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.RetryCancel, IEnumerable<Object> buttonsName = null)
        {
            title ??= "Exception occured";
            additionalText ??= $"{title}{Environment.NewLine}Information:";
            List<Exception> exceptions = new List<Exception>();
            do
            {
                if (!(exception is TargetInvocationException))
                {
                    exceptions.Add(exception);
                }

                exception = exception.InnerException;
            } while (exception != null);

            String text = String.Join($"{Environment.NewLine}",
                exceptions.Select(ex =>
                        $"{Environment.NewLine}Exception: {ex.GetType().Name}{Environment.NewLine}HResult: {ex.HResult:x8}{Environment.NewLine}In method: {ex.TargetSite.Name}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.Source}{Environment.NewLine}{ex.StackTrace}")
                    .ToArray());

            return new MessageForm($@"{additionalText}{text}",
                title, Images.Images.Basic.Error, Images.Images.Basic.Error, messageBoxButtons, buttonsName, true).ShowDialog();
        }

        private class Link
        {
            public readonly Int32 Start;
            public readonly Int32 Length;
            public readonly Object Object;

            public Link(Int32 start, Int32 length, Object obj = null)
            {
                Start = start;
                Length = length;
                Object = obj;
            }
        }

        public MessageForm(String text = null, String title = null, Image icon = null, Image messageIcon = null,
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK, IEnumerable<Object> buttonsName = null,
            Boolean showInTaskBar = false)
        {
            SuspendLayout();

            LinkLabel textLabel = GetTextLabel(FindMatches(text, out IList<Link> links), links);

            AddMessageIcon(messageIcon);

            Text = title ?? "Message";
            Icon = ImageUtils.IconFromImage(icon ?? new Bitmap(Images.Images.Basic.Application, IconSize));
            AutoSize = true;
            Size = new Size(textLabel.Size.Width + (messageIcon != null ? IconSize.Width * 2 : 0),
                74 + Math.Max(textLabel.Size.Height, IconSize.Height));
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = showInTaskBar;
            DialogResult = DialogResult.None;
            textLabel.Location = new Point(Size.Width / 2 - textLabel.Size.Width / 2, 0);
            Controls.Add(textLabel);
            CreateButtons(messageBoxButtons, buttonsName);
            SetButtons();
            SizeChanged += (sender, args) => OnSizeChanged();
            ResumeLayout(false);
            PerformLayout();
            OnSizeChanged();
        }

        private static void OnLink_Clicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            String link = e.Link.LinkData.ToString();
            if (PathUtils.IsValidWebPath(link))
            {
                ProcessUtils.OpenBrowser(link);
                return;
            }

            MessageBox.Show(ThisLinkIsInvalidMessage, InvalidLinkMessage, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void AddMessageIcon(Image icon)
        {
            if (icon == null)
            {
                return;
            }

            PictureBox image = new PictureBox
            {
                Image = new Bitmap(icon, new Size(48, 48)),
                AutoSize = false,
                Size = IconSize,
                Location = new Point(0, 0)
            };

            Controls.Add(image);
        }

        private LinkLabel GetTextLabel(String labelText, IEnumerable<Link> links)
        {
            LinkLabel label = new LinkLabel
            {
                Text = labelText,
                Font = new Font(Font.Name, Font.Size + 3, FontStyle.Regular),
                AutoSize = true,
                LinkArea = new LinkArea(0, 0)
            };

            label.LinkClicked += OnLink_Clicked;

            foreach (Link link in links)
            {
                label.Links.Add(link.Start, link.Length, link.Object);
            }

            label.Size = TextRenderer.MeasureText(labelText, label.Font);

            return label;
        }

        private static String FindMatches(String text, out IList<Link> links)
        {
            text ??= String.Empty;
            text = Regex.Replace(text, @"<.+></.+>", String.Empty);
            List<String[]> matches = Regex.Matches(text, @"(?<=(<link>))(.)+?(?=(<\/link>))")
                .Select(match => match.Value.Split(new[] {"|"}, 2, StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            links = new List<Link>();
            foreach (String[] match in matches)
            {
                String link = match[0];
                String pattern = $@"<link>{link}{(match.Length > 1 ? $"|{match[1]}" : String.Empty)}</link>";
                String linkText = match.Length > 1 ? match[1] : link;
                Int32 index = text.IndexOf(pattern, StringComparison.Ordinal);
                text = text.Replace(pattern, linkText);
                links.Add(new Link(index, linkText.Length, link));
            }

            return text;
        }

        static MessageForm()
        {
            ButtonsParamsList = Enum.GetValues(typeof(MessageBoxButtons)).OfType<MessageBoxButtons>().ToDictionary(e => e,
                e => e.ToString()
                    .SplitBy(SplitByEnum.UpperCase)
                    .Select(epart => (Enum.TryParse(epart, out DialogResult result), result).result)
                    .Select(result => (result, result.ToString())).ToArray());
        }

        private static readonly Dictionary<MessageBoxButtons, (DialogResult result, String epart)[]> ButtonsParamsList;

        private void CreateButtons(MessageBoxButtons messageBoxButtons, IEnumerable<Object> buttonsName = null)
        {
            CreateButtons(messageBoxButtons, buttonsName?.Select(value => value?.ToString()).ToArray());
        }

        private void CreateButtons(MessageBoxButtons messageBoxButtons, IReadOnlyList<String> buttonsName)
        {
            IReadOnlyList<(DialogResult result, String text)> buttons = ButtonsParamsList[messageBoxButtons];

            for (Int32 i = 0; i < Math.Min(buttons.Count, Math.Max(buttonsName?.Count ?? buttons.Count, buttons.Count)); i++)
            {
                String text = buttons[i].text;

                if (buttonsName != null && buttonsName.TryGetValue(i, out String txt) && txt != null)
                {
                    text = txt;
                }

                AddButton(text, buttons[i].result);
            }
        }

        private void AddButton(String text, DialogResult result)
        {
            if (text == null)
            {
                return;
            }

            Button button = new Button
            {
                Text = text,
                AutoSize = false
            };
            button.Click += (sender, args) => DialogResult = result;
            _buttons.Add(button);
        }


        private void SetButtons()
        {
            foreach (Button button in _buttons)
            {
                Controls.Add(button);
            }
        }

        private void OnSizeChanged()
        {
            const Int32 buttonHeight = 30;
            for (Int32 i = 0; i < _buttons.Count; i++)
            {
                //Centerize buttons
                Button button = _buttons[i];
                button.Size = new Size(Size.Width / Math.Max(3, _buttons.Count) - (_buttons.Count - 1) * button.Size.Height / 2,
                    buttonHeight);
                button.Location =
                    new Point((Int32) ((button.Size.Width + 5) * (i + (_buttons.Count == 3 ? 0.1 : 0) + 0.5 * (3 - _buttons.Count))),
                        Size.Height - button.Size.Height * 2 - 10);
            }
        }

        public new DialogResult Show()
        {
            return ShowDialog();
        }

        public new DialogResult ShowDialog()
        {
            base.ShowDialog();
            return DialogResult;
        }
    }
}