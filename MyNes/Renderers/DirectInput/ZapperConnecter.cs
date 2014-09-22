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
using System.Windows.Forms;
using MyNes.Core;
using SlimDX.DirectInput;
namespace MyNes
{
    public class ZapperConnecter : IZapperConnecter
    {
        // TODO: Zapper with "Wild Gun Man" - The Gang Mode is not working !
        public ZapperConnecter(IntPtr handle, int winPosX, int winPosY, int videoW, int videoH)
        {
            this.scanlinesCount = 240;
            this.videoW = videoW;
            this.videoH = videoH;
            this.winPosX = winPosX;
            this.winPosY = winPosY;
            DirectInput di = new DirectInput();
            mouse = new Mouse(di);
            mouse.SetCooperativeLevel(handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);
        }
        private Mouse mouse;
        private MouseState mousestate;
        private bool oldTrigger;
        private int timer = 3;
        private int pixelX;
        private int pixelY;
        public int winPosX;
        public int winPosY;
        public int videoW;
        public int videoH;
        public int scanlinesCount;
        private int c;
        private byte r;
        private byte g;
        private byte b;
        public override void Update()
        {
            if (mouse.Acquire().IsSuccess)
            {
                mousestate = mouse.GetCurrentState();

                oldTrigger = Trigger;
                Trigger = mousestate.IsPressed((int)MouseObject.Button1);

                if (timer > 0)
                {
                    timer--;
                    pixelX = ((Cursor.Position.X - winPosX) * 256) / videoW;
                    if (pixelX < 0 || pixelX >= 256)
                    {
                        State = false;
                        return;
                    }
                    pixelY = ((Cursor.Position.Y - winPosY) * scanlinesCount) / videoH;
                    if (pixelY < 0 || pixelY >= scanlinesCount)
                    {
                        State = false;
                        return;
                    }
                    //System.Console.WriteLine(pixelX + ", " + pixelY);
                    for (int x = -15; x < 15; x++)
                    {
                        for (int y = -15; y < 15; y++)
                        {
                            if (pixelX + x < 256 && pixelX + x >= 0 && pixelY + y < scanlinesCount && pixelY + y >= 0)
                            {
                                c = NesEmu.GetPixel(pixelX + x, pixelY + y);
                                r = (byte)(c >> 0x10); // R
                                g = (byte)(c >> 0x08); // G
                                b = (byte)(c >> 0x00);  // B
                                State = (r > 85 && g > 85 && b > 85);//bright color ?
                            }
                            if (State)
                                break;
                        }
                        if (State)
                            break;
                    }
                }
                else
                    State = false;
                if (!Trigger && oldTrigger)
                    timer = 6;
            }
        }
    }
}
