using StdOttFramework.Hotkey;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CodeGenerator
{
    class CodeCopyService
    {
        private static CodeCopyService instance;

        public static CodeCopyService Current
        {
            get
            {
                if (instance == null) instance = new CodeCopyService();

                return instance;
            }
        }

        private DispatcherTimer timer;
        private string lastClipboardText;
        private ICodeObjectsService currentCodeObjectsService;
        private HotKey hotKey;

        private CodeCopyService()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;

            hotKey = HotKey.GetInstance(Key.B, KeyModifier.Ctrl);
            hotKey.Pressed += HotKey_Pressed;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!Clipboard.ContainsText() || Clipboard.GetText() != lastClipboardText) Stop();
            }
            catch { }
        }

        private void HotKey_Pressed(object sender, KeyPressedEventArgs e)
        {
            e.Handled = true;

            if (currentCodeObjectsService == null) return;

            if (currentCodeObjectsService.IsCopying) CopyNextCodePart(currentCodeObjectsService);
            else Stop();
        }

        public void CopyWholeCode(ICodeObjectsService service)
        {
            Stop();

            try
            {
                CopyAndShow(service.GetWholeCode());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception");
            }
        }

        public void CopyNextCodePart(ICodeObjectsService service)
        {
            if (service != currentCodeObjectsService) Stop();

            currentCodeObjectsService = service;

            bool isLastPart;

            try
            {
                string code = currentCodeObjectsService.GetNextCodePart(out isLastPart);

                CopyPart(code);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                isLastPart = true;
            }


            if (isLastPart) Stop();
            else
            {
                timer.Start();
                hotKey.Register();
            }
        }

        public void Stop()
        {
            hotKey.Unregister();
            timer.Stop();

            currentCodeObjectsService?.StopCopying();
            currentCodeObjectsService = null;
        }

        private void CopyPart(string text)
        {
            try
            {
                Clipboard.SetText(text);
                lastClipboardText = text;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception");
            }
        }

        public void CopyAndShow(string text)
        {
            try
            {
                Stop();
                Clipboard.SetText(text);

                ShortText(ref text, 15);
                MessageBox.Show(text);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception");
            }
        }

        private void ShortText(ref string text, int maxLines)
        {
            for (int i = 0, lines = 0; i < text.Length; i++)
            {
                if (text[i] != '\n' || lines++ < maxLines) continue;

                text = text.Remove(i);
            }
        }
    }
}
