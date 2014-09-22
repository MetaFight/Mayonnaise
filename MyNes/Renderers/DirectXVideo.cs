/* This file is part of My Nes
 * 
 * A Nintendo Entertainment System / Family Computer (Nes/Famicom) 
 * Emulator written in C#.
 *
 * Copyright © Ala Ibrahim Hadid 2009 - 2014
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using SlimDX;
using SlimDX.Direct3D9;
using MyNes.Core;

namespace MyNes
{
    public unsafe class DirectXVideo : IVideoProvider, IDisposable
    {
        private Control surface_control;
        private Direct3D direct3D = new Direct3D();
        private int[] currentBuffer;
        private int linesToSkip = 8;
        private int scanlines;
        private Rectangle originalRect;
        private Rectangle destinationRect;
        private DisplayMode currentMode;
        private PresentParameters presentParams;
        private Device displayDevice;
        private Format displayFormat;
        private Surface bufferedSurface;
        private Surface frontSurface;
        private Sprite displaySprite;
        private bool canRender;
        private int bufferSize;
        private bool initialized;
        private bool disposed;
        private bool isRendering;
        private bool deviceLost;
        // FPS and Notification
        private SlimDX.Direct3D9.Font font;
        private SlimDX.Direct3D9.Font font_BIG;
        private int notification_y = 0;
        private int notification_x = 0;
        private int notification_appearance;
        private string notification_text;
        private Color notification_color = Color.White;
        private int fullscreen_mode_index = 0;
        private int show_emulation_status_timer;
        private string emulation_status_game_name;
        // Settings
        private SlimDX.Direct3D9.TextureFilter filter = TextureFilter.Point;
        private bool cut_lines = true;
        private bool fullScreen = false;
        private bool hardware_vertex_processing = true;
        private bool keep_aspect_ratio = true;
        private bool show_fps = true;
        private bool show_notification = true;
        private int frame_timer = 2;
        // Thread
        private Thread thread;
        public bool threadON;
        public bool threadPAUSED;
        private Random r = new Random();
        private byte c;

        public DirectXVideo(Control surface_control)
        {
            this.surface_control = surface_control;
            // Make it not fullscreen
            fullScreen = false;
            threadPAUSED = false;
            threadON = true;
            // Make thread
            thread = new System.Threading.Thread(new ThreadStart(ClockThread));
            thread.Start();
        }
        #region Initialize and disposing methods
        public void Initialize()
        {
            LoadSettings();
            if (cut_lines)
            {
                if (NesEmu.TVFormat == TVSystem.NTSC)
                {
                    linesToSkip = 8;
                    scanlines = 224;
                }
                else
                {
                    linesToSkip = 1;
                    scanlines = 238;
                }
            }
            else
            {
                linesToSkip = 0;
                scanlines = 240;
            }
            currentBuffer = new int[256 * 240];
            originalRect = new Rectangle(0, 0, 256, scanlines);
            currentMode = direct3D.Adapters[0].CurrentDisplayMode;
            Console.WriteLine("Direct3D: Initializing Direct 3d video device ...");
            if (!NesEmu.EmulationON)
                fullScreen = false;
            try
            {
                if (!fullScreen)
                {
                    presentParams = new PresentParameters();
                    presentParams.BackBufferWidth = surface_control.Width;
                    presentParams.BackBufferCount = 1;
                    presentParams.BackBufferHeight = surface_control.Height;

                    presentParams.BackBufferFormat = Format.A8R8G8B8;
                    presentParams.Windowed = true;
                    presentParams.SwapEffect = SwapEffect.Discard;
                    presentParams.Multisample = MultisampleType.None;
                    presentParams.PresentationInterval = PresentInterval.Default;
                    displayFormat = currentMode.Format;
                    displayDevice = new Device(direct3D, 0, DeviceType.Hardware, surface_control.Handle,
                    hardware_vertex_processing ? CreateFlags.HardwareVertexProcessing : CreateFlags.SoftwareVertexProcessing,
                    presentParams);
                    bufferSize = (256 * scanlines * 4);
                    font = new SlimDX.Direct3D9.Font(displayDevice,
                        new System.Drawing.Font("Tahoma", (8 * surface_control.Height) / scanlines, FontStyle.Bold));
                    font_BIG = new SlimDX.Direct3D9.Font(displayDevice,
                        new System.Drawing.Font("Tahoma", (10 * surface_control.Height) / scanlines, FontStyle.Bold));
                    CreateDisplayObjects(displayDevice);
                    initialized = true;
                    disposed = false;
                    if (keep_aspect_ratio)
                        destinationRect = GetRatioStretchRectangle(surface_control.Width, surface_control.Height);
                    else
                        destinationRect = new Rectangle(0, 0, surface_control.Width, surface_control.Height);
                    notification_x = destinationRect.X + 2;
                    notification_y = surface_control.Height - ((surface_control.Height * 25) / scanlines);
                    canRender = true;
                    surface_control.Select();
                }
                else
                {
                    DisplayMode mode = this.FindSupportedMode();
                    presentParams = new PresentParameters();
                    presentParams.BackBufferFormat = mode.Format;
                    presentParams.BackBufferCount = 1;
                    presentParams.BackBufferWidth = mode.Width;
                    presentParams.BackBufferHeight = mode.Height;
                    presentParams.FullScreenRefreshRateInHertz = mode.RefreshRate;
                    presentParams.Windowed = false;
                    presentParams.SwapEffect = SwapEffect.Discard;
                    presentParams.Multisample = MultisampleType.None;
                    presentParams.PresentationInterval = PresentInterval.Default;
                    displayFormat = mode.Format;
                    displayDevice = new Device(direct3D, 0, DeviceType.Hardware, surface_control.Parent.Handle,
                        hardware_vertex_processing ? CreateFlags.HardwareVertexProcessing : CreateFlags.SoftwareVertexProcessing,
                        presentParams);
                    displayDevice.ShowCursor = false;
                    bufferSize = (256 * scanlines) * 4;

                    font = new SlimDX.Direct3D9.Font(displayDevice, new System.Drawing.Font("Tahoma", (8 * mode.Height) / scanlines,
                        FontStyle.Bold));
                    CreateDisplayObjects(displayDevice);
                    initialized = true;
                    disposed = false;
                    if (keep_aspect_ratio)
                        destinationRect = GetRatioStretchRectangle(mode.Width, mode.Height);
                    else
                        destinationRect = new Rectangle(0, 0, mode.Width, mode.Height);
                    notification_x = destinationRect.X + 2;
                    notification_y = mode.Height - ((mode.Height * 25) / scanlines);
                    canRender = true;
                }
                SetupZapperBounds();
                Console.WriteLine("Direct3D: Direct 3d video device initialized.");
            }
            catch (Exception e)
            {
                initialized = false;
                Console.WriteLine("Direct3D: Failed to initialize the Direct 3d video device:");
                Console.WriteLine(e.Message);
            }
        }
        private void CreateDisplayObjects(Device device)
        {
            displaySprite = new Sprite(device);
            bufferedSurface = Surface.CreateRenderTarget(device, 256, scanlines, Format.A8R8G8B8,
                MultisampleType.None, 0, true);

            frontSurface = device.GetBackBuffer(0, 0);
        }
        private void DisposeDisplayObjects()
        {
            Console.WriteLine("Direct3D: shutdown video ...");
            if (displaySprite != null)
                displaySprite.Dispose();

            if (bufferedSurface != null)
                bufferedSurface.Dispose();

            if (frontSurface != null)
                frontSurface.Dispose();

            if (font != null)
                font.Dispose();
            Console.WriteLine("Direct3D: video shutdown done.");
        }
        private void ResetDirect3D()
        {
            if (!initialized)
                return;

            DisposeDisplayObjects();

            try
            {
                displayDevice.Reset(presentParams);
                CreateDisplayObjects(displayDevice);
            }
            catch
            {
                displayDevice.Dispose();
                Initialize();
            }
        }
        public void Reset()
        {
            while (isRendering) { }
            canRender = false;
            threadPAUSED = true;

            //shutdown renderer
            this.Dispose();
            //re-Initialize
            Initialize();
            threadPAUSED = false;
        }
        public void SetupZapperBounds()
        {
            if (Program.FormMain == null) return;
            if (Program.FormMain.zapper == null) return;
            Program.FormMain.zapper.scanlinesCount = scanlines;
            if (!fullScreen)
            {
                // ZAPPER
                Program.FormMain.zapper.videoH = surface_control.Height;
                Program.FormMain.zapper.videoW = surface_control.Width;
                Program.FormMain.zapper.winPosX = Program.FormMain.Location.X;
                Program.FormMain.zapper.winPosY = Program.FormMain.Location.Y + Program.FormMain.menuStrip1.Height;
            }
            else
            {
                // ZAPPER
                Program.FormMain.zapper.videoH = destinationRect.Height;
                Program.FormMain.zapper.videoW = destinationRect.Width;
                Program.FormMain.zapper.winPosX = destinationRect.X;
                Program.FormMain.zapper.winPosY = destinationRect.Y;
            }
        }
        public void ShutDown()
        {
        }
        public void Dispose()
        {
            if (displayDevice != null)
            {
                displayDevice.Dispose();
            }

            this.DisposeDisplayObjects();

            disposed = true;
        }
        public bool Disposed
        { get { return disposed; } }
        private Rectangle GetRatioStretchRectangle(int windowWidth, int windowHeight)
        {
            float pRatio = (float)windowWidth / windowHeight;
            float imRatio = (float)256 / scanlines;

            int w = 0;
            int h = 0;

            if (windowWidth >= 256 && windowHeight >= scanlines)
            {
                if (windowWidth >= windowHeight)
                {
                    //width image
                    if (256 >= scanlines && imRatio >= pRatio)
                    {
                        w = windowWidth;
                        h = (int)(windowWidth / imRatio);
                    }
                    else
                    {
                        h = windowHeight;
                        w = (int)(windowHeight * imRatio);
                    }
                }
                else
                {
                    if (256 < scanlines && imRatio < pRatio)
                    {
                        h = windowHeight;
                        w = (int)(windowHeight * imRatio);
                    }
                    else
                    {
                        w = windowWidth;
                        h = (int)(windowWidth / imRatio);
                    }
                }
            }
            else if (windowWidth > 256 && windowHeight < scanlines)
            {
                h = windowHeight;
                w = (int)(windowHeight * imRatio);
            }
            else if (windowWidth < 256 && windowHeight > scanlines)
            {
                w = windowWidth;
                h = (int)(windowWidth / imRatio);
            }
            else if (windowWidth < 256 && windowHeight < scanlines)
            {
                if (windowWidth >= windowHeight)
                {
                    //width image
                    if (256 >= scanlines && imRatio >= pRatio)
                    {
                        w = windowWidth;
                        h = (int)(windowWidth / imRatio);
                    }
                    else
                    {
                        h = windowHeight;
                        w = (int)(windowHeight * imRatio);
                    }
                }
                else
                {
                    if (256 < scanlines && imRatio < pRatio)
                    {
                        h = windowHeight;
                        w = (int)(windowHeight * imRatio);
                    }
                    else
                    {
                        w = windowWidth;
                        h = (int)(windowWidth / imRatio);
                    }
                }
            }
            return new Rectangle((windowWidth - w) / 2, 0, w, h);
        }
        #endregion
        #region Thread and frame methods
        private void ClockThread()
        {
            while (threadON)
            { if (!threadPAUSED) PresentFrame(); }
            this.Dispose();
        }
        public void CloseThread()
        {
            canRender = false;
            threadON = false;
            // Get out of here !
            //thread.Abort();
            //this.Dispose();
        }
        private void PresentFrame()
        {
            if (disposed)
            { isRendering = false; return; }
            if (!initialized)
            { isRendering = false; return; }
            if (!NesEmu.EmulationON)
            {
                if (fullScreen)
                {
                    fullScreen = false;
                    Reset();
                }
                // Make a snow buffer when emulation is off
                for (int i = 0; i < currentBuffer.Length; i++)
                {
                    c = (byte)r.Next(0, 255);
                    currentBuffer[i] = c | (c << 8) | (c << 16) | (255 << 24);
                }
            }
            // Render the buffer
            if (isRendering)
            { isRendering = false; return; }
            if (!canRender)
            { isRendering = false; return; }

            if (frame_timer > 0)
                frame_timer--;
            else
                frame_timer = 60;
            isRendering = true;
            var result = displayDevice.TestCooperativeLevel();

            switch (result.Code)
            {
                case -2005530510:
                case -2005530520: deviceLost = true; isRendering = false; break;
                case -2005530519: deviceLost = false; isRendering = false; ResetDirect3D(); break;
            }
            // Render current buffer
            if (!deviceLost)
            {
                DataRectangle rect = bufferedSurface.LockRectangle(LockFlags.Discard);

                rect.Data.WriteRange(currentBuffer, linesToSkip * 256, 256 * scanlines);

                rect.Data.Close();
                bufferedSurface.UnlockRectangle();
                // copy the surface data to the device one with stretch. 
                if (canRender && !disposed)
                {
                    if (keep_aspect_ratio)
                        displayDevice.StretchRectangle(bufferedSurface, originalRect, frontSurface, destinationRect, filter);
                    else
                        displayDevice.StretchRectangle(bufferedSurface, frontSurface, filter);
                }


                displayDevice.BeginScene();

                displaySprite.Begin(SpriteFlags.AlphaBlend);
                // fps
                if (NesEmu.EmulationON)
                {
                    if (NesEmu.EmulationPaused)
                        font_BIG.DrawString(displaySprite, "PAUSED", 20, 20, Color.Lime);
                    else if (Program.FormMain.audio != null)
                    {
                        if (Program.FormMain.audio.IsRecording)
                            font.DrawString(displaySprite, "RECORDING SOUND [" + TimeSpan.FromSeconds(Program.FormMain.audio.Recorder.Time) + "]", 20, 20, Color.OrangeRed);
                        // Debug sound sync by calculating the difference between pointers and show it
                        //font.DrawString(displaySprite, (Program.FormMain.audio.CurrentWritePosition - NesEmu.audio_playback_w_pos).ToString(), 20, 20, Color.OrangeRed);
                    }
                    if (show_emulation_status_timer > 0)
                    {
                        show_emulation_status_timer--;
                        font.DrawString(displaySprite, "GAME: " + emulation_status_game_name, 20, 20, Color.Lime);
                        font.DrawString(displaySprite,
                            "TV SYS: " + NesEmu.TVFormat.ToString() +
                            (NesEmu.IsGameGenieActive ? " | GAME GENIE ON" : "")
                            , 20, 50, Color.Lime);
                        double fps = (1.0 / NesEmu.ImmediateFrameTime);
                        double pfps = (1.0 / NesEmu.CurrentFrameTime);
                        font.DrawString(displaySprite, "FPS: " + fps.ToString("F2") + "/" + pfps.ToString("F2"),
                            20, 80, Color.Lime);
                    }
                    else if (show_fps)
                    {
                        if (!NesEmu.EmulationPaused)
                        {
                            double fps = (1.0 / NesEmu.ImmediateFrameTime);
                            double pfps = (1.0 / NesEmu.CurrentFrameTime);
                            font.DrawString(displaySprite, fps.ToString("F2") + "/" + pfps.ToString("F2"), 20, 20, Color.Lime);
                        }
                    }

                    if (!NesEmu.SpeedLimitterON)
                    {
                        if (frame_timer > 30)
                            font_BIG.DrawString(displaySprite, "TURBO !", 20, ScreenHeight - 100, Color.Lime);
                    }
                }

                if (notification_appearance > 0)
                {
                    notification_appearance--;

                    // Draw the string
                    font.DrawString(displaySprite, notification_text, notification_x, notification_y, notification_color);
                }

                displaySprite.End();

                displayDevice.EndScene();
                displayDevice.Present();
            }
            isRendering = false;
        }
        public void SubmitBuffer(ref int[] buffer)
        {
            // if (!isRendering)
            {
                canRender = false;
                buffer.CopyTo(currentBuffer, 0);
                canRender = true;
            }
        }
        private double GetTime()
        {
            return (double)Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;
        }
        #endregion
        private DisplayMode FindSupportedMode()
        {
            return direct3D.Adapters[0].GetDisplayModes(Format.X8R8G8B8)[fullscreen_mode_index];
        }
        private void LoadSettings()
        {
            fullScreen = Program.Settings.Video_StartFullscreen;
            fullscreen_mode_index = Program.Settings.Video_FullscreenRes;
            show_notification = Program.Settings.Video_ShowNotifications;
            show_fps = Program.Settings.Video_ShowFPS;
            cut_lines = Program.Settings.Video_CutLines;
            hardware_vertex_processing = Program.Settings.Video_HardwareVertexProcessing;
            keep_aspect_ratio = Program.Settings.Video_KeepAspectRatio;
            filter = Program.Settings.Video_Filter;
        }
        public void WriteNotification(string text, int frames, Color color)
        {
            if (!show_notification) return;
            notification_appearance = frames;
            notification_text = text;
            notification_color = color;
        }
        public void TakeSnapshot(string snapsfolder, string snapFileName, string format, bool replace)
        {
            canRender = false;
            //while (isRendering) { }
            threadPAUSED = true;
            //NesEmu.EmulationPaused = true; 
            // make sure the file is not replaced
            string path = "";

            if (!replace)
            {
                int j = 0;
                path = snapsfolder + "\\" + snapFileName + "_" + j + format;
                while (System.IO.File.Exists(path))
                {
                    j++;
                    path = snapsfolder + "\\" + snapFileName + "_" + j + format;
                }
            }
            else
            {
                path = snapsfolder + "\\" + snapFileName + format;
            }
            // get image data as bitmap
            Bitmap bitmap = new Bitmap(256, scanlines);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, 256, scanlines), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);
            int* pointer = (int*)bitmapData.Scan0;

            var rect = bufferedSurface.LockRectangle(LockFlags.Discard);
            byte[] data = new byte[bufferSize];
            rect.Data.Read(data, 0, bufferSize);
            rect.Data.Close();
            bufferedSurface.UnlockRectangle();

            for (int i = 0; i < data.Length; i += 4)
            {
                *pointer++ =
                    (data[i + 3] << 0x18) | // A
                    (data[i + 2] << 0x10) | // R
                    (data[i + 1] << 0x08) | // G
                    (data[i + 0] << 0x00);  // B
            }

            bitmap.UnlockBits(bitmapData);

            switch (format)
            {
                case ".bmp": bitmap.Save(path, ImageFormat.Bmp); break;
                case ".gif": bitmap.Save(path, ImageFormat.Gif); break;
                case ".jpg": bitmap.Save(path, ImageFormat.Jpeg); break;
                case ".png": bitmap.Save(path, ImageFormat.Png); break;
                case ".tiff": bitmap.Save(path, ImageFormat.Tiff); break;
                case ".emf": bitmap.Save(path, ImageFormat.Emf); break;
                case ".wmf": bitmap.Save(path, ImageFormat.Wmf); break;
                case ".exif": bitmap.Save(path, ImageFormat.Exif); break;
            }

            threadPAUSED = false;
            canRender = true;
            WriteNotification("Snapshot taken. [" + System.IO.Path.GetFileName(path) + "]", 120, Color.Green);
            Console.WriteLine("Direct3D: Snapshot taken");
            NesEmu.EmulationPaused = false;
        }
        public void SwitchFullscreen()
        {
            while (isRendering) { }
            threadPAUSED = true;
            Program.Settings.Video_StartFullscreen = !Program.Settings.Video_StartFullscreen;
            Reset();
        }
        public void OnResizeBegin()
        {
            threadPAUSED = true;
        }
        public void OnResizeEnd()
        {
            Reset();
        }
        public int ScreenWidth
        {
            get
            {
                return presentParams.BackBufferWidth;

            }
        }
        public int ScreenHeight
        {
            get { return presentParams.BackBufferHeight; }
        }
        public void ShowEmulationStatus()
        {
            if (NesEmu.EmulationON)
            {
                show_emulation_status_timer = 240;
                if (NesEmu.IsGameFoundOnDB)
                {
                    if (NesEmu.GameInfo.Game_AltName != null && NesEmu.GameInfo.Game_AltName != "")
                        emulation_status_game_name = NesEmu.GameInfo.Game_Name + " (" + NesEmu.GameInfo.Game_AltName + ")";
                    else
                        emulation_status_game_name = NesEmu.GameInfo.Game_Name;
                }
                else
                {
                    emulation_status_game_name = System.IO.Path.GetFileName(NesEmu.GAMEFILE);
                }
            }
        }
    }
}
