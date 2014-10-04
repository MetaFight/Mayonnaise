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
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using MyNes.Core.SoundChannels;
/*
 * Using one big partial class may increase performance in C#
*/
namespace MyNes.Core
{
	/// <summary>
	/// The nes emulation engine.
	/// </summary>
	[Obsolete("Reminder to not inject this class into other classes.  When refactorying is complete there should be very little logic left in here.")]
	public class Emulator
	{
		public Emulator(TVSystem tvFormat)
		{
			this.TVFormat = tvFormat;

			this.InitializeComponents(
				out this.ppu,
				out this.interrupts,
				out this.memory,
				out this.dma,
				out this.apu,
				out this.cpu,
				out this.input,
				out this.legacyNesEmu);

			this.input.InitializeInput();

			this.emulationState = 
				new EmulationState(
					this,
					this.ppu,
					this.interrupts,
					this.memory,
					this.dma,
					this.apu,
					this.cpu,
					this.input,
					this.ppu.videoOut);

			switch (TVFormat)
			{
				case TVSystem.NTSC:
					this.apu.SystemIndex = 0;
					this.apu.audio_playback_samplePeriod = 1789772.67f;
					break;
				case TVSystem.PALB:
					this.apu.SystemIndex = 1;
					this.apu.audio_playback_samplePeriod = 1662607f;
					break;
				case TVSystem.DENDY:
					this.apu.SystemIndex = 2;
					this.apu.audio_playback_samplePeriod = 1773448f;
					break;
			}
			if (TVFormat == TVSystem.NTSC)
				FramePeriod = (1.0 / (FPS = 60.0));
			else//PALB, DENDY
				FramePeriod = (1.0 / (FPS = 50.0));
		}

		private void InitializeComponents(out Ppu ppu, out Interrupts interrupts, out Memory memory, out Dma dma, out Apu apu, out Cpu cpu, out Input input, out LegacyNesEmu legacy)
		{
			var ppuWip = new Ppu(this);
			var legacyWip = new LegacyNesEmu(ppuWip);
			var interruptsWip = new Interrupts(ppuWip);
			var inputWip = new Input(legacyWip);
			var memoryWip = new Memory(this, ppuWip, interruptsWip, inputWip, legacyWip);
			var dmaWip = new Dma(this, memoryWip);
			var apuWip = new Apu(this, dmaWip, memoryWip);
			var cpuWip = new Cpu(interruptsWip, memoryWip);

			dmaWip.apu = apuWip;
			memoryWip.dma = dmaWip;
			memoryWip.apu = apuWip;
			interruptsWip.cpu = cpuWip;
			ppuWip.memory = memoryWip;
			ppuWip.interrupts = interruptsWip;

			ppu = ppuWip;
			interrupts = interruptsWip;
			memory = memoryWip;
			dma = dmaWip;
			apu = apuWip;
			cpu = cpuWip;
			input = inputWip;
			legacy = legacyWip;
		}

		public TVSystemSetting TVFormatSetting;
		public TVSystem TVFormat;
		public Thread EmulationThread;
		public string GAMEFILE;
		public bool DoPalAdditionalClock;
		public byte palCyc;
		private bool initialized;
		/*SRAM*/
		public bool SaveSRAMAtShutdown;
		public string SRAMFileName;
		private string SRAMFolder;
		/*Snapshot*/
		private string SNAPSFolder;
		private string SNAPSFileName;
		private string SNAPSFormat;
		private bool SNAPSReplace;
		/*SPEED LIMITER*/
		public bool SpeedLimitterON = true;
		public double CurrentFrameTime;
		public double ImmediateFrameTime;
		private double DeadTime;
		private double LastFrameTime;
		private double FramePeriod = (1.0 / 60.0988);
		private double FPS = 0;
		// Requests !
		[Obsolete("I'm not to keen on handling requests with bool flags.  Consider redesigning this.")]
		public bool request_pauseAtFrameFinish;
		[Obsolete("I'm not to keen on handling requests with bool flags.  Consider redesigning this.")]
		private bool request_hardReset;
		[Obsolete("I'm not to keen on handling requests with bool flags.  Consider redesigning this.")]
		private bool request_softReset;
		[Obsolete("I'm not to keen on handling requests with bool flags.  Consider redesigning this.")]
		public bool request_state_save;
		[Obsolete("I'm not to keen on handling requests with bool flags.  Consider redesigning this.")]
		public bool request_state_load;
		[Obsolete("I'm not to keen on handling requests with bool flags.  Consider redesigning this.")]
		private bool request_snapshot;
		[Obsolete("I'm not to keen on handling requests with bool flags.  Consider redesigning this.")]
		private bool request_save_sram;
		
		private readonly Dma dma;
		private readonly Memory memory;
		[Obsolete("Mega-hack until I can figure out how PPU and other classes should interact.")]
		public readonly Ppu ppu;
		private readonly Cpu cpu;
		[Obsolete("Mega-hack until I can figure out how APU and other classes should interact.")]
		public readonly Apu apu;
		private readonly Interrupts interrupts;
		[Obsolete("Mega-hack until the current input class is split into two.")]
		public readonly Input input;
		public EmulationState emulationState;
		[Obsolete("Temporary field.  To be removed after all code is moved out of the original NesEmu (now LegacyNesEmu) class")]
		private readonly LegacyNesEmu legacyNesEmu;

		/// <summary>
		/// Call this at application start up to set nes default stuff
		/// </summary>
		public void WarmUp()
		{
			this.apu.InitializeSoundMixTable();
			NesCartDatabase.LoadDatabase("database.xml");
		}

		/// <summary>
		/// Check a rom file to see if it can be used or not
		/// </summary>
		/// <param name="fileName">The complete file path. Archive is NOT supported.</param>
		/// <param name="is_supported_mapper">Indicates if this rom mapper is supported or not</param>
		/// <param name="has_issues">Indicates if this rom mapper have issues or not</param>
		/// <param name="known_issues">Issues with this mapper.</param>
		/// <returns>True if My Nes car run this game otherwise false.</returns>
		public bool CheckRom(string fileName, out bool is_supported_mapper,
			out bool has_issues, out string known_issues)
		{
			switch (Path.GetExtension(fileName).ToLower())
			{
				case ".nes":
					{
						INes header = new INes();
						header.Load(fileName, true);
						if (header.IsValid)
						{
							// Check board existince
							bool found = false;
							string mapperName = "MyNes.Core.Mapper" + header.MapperNumber.ToString("D3");
							Type[] types = Assembly.GetExecutingAssembly().GetTypes();
							foreach (Type tp in types)
							{
								if (tp.FullName == mapperName)
								{
									this.memory.board = Activator.CreateInstance(tp) as Board;
									this.memory.board.Nes = this;
									is_supported_mapper = this.memory.board.Supported;
									has_issues = this.memory.board.NotImplementedWell;
									known_issues = this.memory.board.Issues;
									found = true;
									return true;
								}
							}
							if (!found)
							{
								throw new MapperNotSupportedException(header.MapperNumber);
							}
							is_supported_mapper = false;
							has_issues = false;
							known_issues = "";
							return false;
						}
						is_supported_mapper = false;
						has_issues = false;
						known_issues = "";
						return false;
					}
			}
			is_supported_mapper = false;
			has_issues = false;
			known_issues = "";
			return false;
		}

		/// <summary>
		/// Create new emulation engine
		/// </summary>
		/// <param name="fileName">The rom complete path. Not compressed</param>
		/// <param name="tvsetting">The tv system setting to use</param>
		/// <param name="makeThread">Indicates if the emulation engine should make an internal thread and run through it. Otherwise you should make a thread and use EMUClock to run the loop.</param>
		[Obsolete("Move this out of emulator class.  Spawning a thread for the GameLoop should happen outside of the Emulator")]
		public void CreateNew(string fileName, TVSystemSetting tvsetting, bool makeThread)
		{
			switch (Path.GetExtension(fileName).ToLower())
			{
				case ".nes":
					{
						INes header = new INes();
						header.Load(fileName, true);
						if (header.IsValid)
						{
							initialized = false;
							GAMEFILE = fileName;
							this.legacyNesEmu.CheckGameVSUnisystem(header.SHA1, header.IsVSUnisystem, header.MapperNumber);
							// Make SRAM file name
							SRAMFileName = Path.Combine(SRAMFolder, Path.GetFileNameWithoutExtension(fileName) + ".srm");
							this.emulationState.STATESlot = 0;
							this.emulationState.UpdateStateSlot(this.emulationState.STATESlot);
							// Make snapshots file name
							SNAPSFileName = Path.GetFileNameWithoutExtension(fileName);
							// Initialzie
							this.memory.MEMInitialize(header);

							TVFormatSetting = tvsetting;

							// Hard reset
							HardReset();
							// Run emu
							this.emulationState.EmulationPaused = true;
							this.emulationState.EmulationON = true;
							// Let's go !
							if (makeThread)
							{
								EmulationThread = new Thread(new ThreadStart(GameLoop));
								EmulationThread.Start();
							}
							// Done !
							initialized = true;
						}
						else
						{
							throw new RomNotValidException();
						}
						break;
					}
			}
		}

		public void ApplySettings(bool saveSramOnSutdown, string sramFolder, string stateFolder,
			string snapshotsFolder, string snapFormat, bool replaceSnap)
		{
			SaveSRAMAtShutdown = saveSramOnSutdown;
			SRAMFolder = sramFolder;
			this.emulationState.STATEFolder = stateFolder;
			SNAPSFolder = snapshotsFolder;
			SNAPSFormat = snapFormat;
			SNAPSReplace = replaceSnap;
		}
		/// <summary>
		/// Run the emulation loop while EmulationON is true.
		/// </summary>
		public void GameLoop()
		{
			while (this.emulationState.EmulationON)
			{
				if (!this.emulationState.EmulationPaused)
				{
					this.cpu.Clock();
				}
				else
				{
					if (this.apu.AudioOut != null)
					{
						this.apu.AudioOut.Pause();
						this.apu.audio_playback_w_pos = this.apu.AudioOut.CurrentWritePosition;
					}
					if (request_save_sram)
					{
						request_save_sram = false;
						this.memory.SaveSRAM();
						this.emulationState.EmulationPaused = false;
					}
					if (request_hardReset)
					{
						request_hardReset = false;
						HardReset();
						this.emulationState.EmulationPaused = false;
					}
					if (request_softReset)
					{
						request_softReset = false;
						SoftReset();
						this.emulationState.EmulationPaused = false;
					}
					if (request_state_save)
					{
						request_state_save = false;
						this.emulationState.SaveStateAs(this.emulationState.STATEFileName);
						this.emulationState.EmulationPaused = false;
					}
					if (request_state_load)
					{
						request_state_load = false;
						this.emulationState.LoadStateAs(this.emulationState.STATEFileName);
						this.emulationState.EmulationPaused = false;
					}
					if (request_snapshot)
					{
						request_snapshot = false;
						this.ppu.videoOut.TakeSnapshot(SNAPSFolder, SNAPSFileName, SNAPSFormat, SNAPSReplace);
						this.emulationState.EmulationPaused = false;
					}
					Thread.Sleep(100);
				}
			}
			// Shutdown
			ShutDown();
		}
		/// <summary>
		/// Request a hard reset in the next frame.
		/// </summary>
		public void EMUHardReset()
		{
			request_pauseAtFrameFinish = true;
			request_hardReset = true;
			request_save_sram = true;
		}
		/// <summary>
		/// Request a soft reset in the next frame
		/// </summary>
		public void EMUSoftReset()
		{
			request_pauseAtFrameFinish = true;
			request_softReset = true;
		}
		/// <summary>
		/// Shutdown the emulation. This will set the EmulationON to false as well.
		/// </summary>
		public void ShutDown()
		{
			if (!initialized)
				return;
			this.emulationState.EmulationON = false;
			this.memory.MEMShutdown();
			if (this.ppu.videoOut != null)
				this.ppu.videoOut.ShutDown();
			// videoOut = null;
			if (this.apu.AudioOut != null)
				this.apu.Shutdown();
			// AudioOut = null;
			System.GC.Collect();

			this.cpu.Shutdown();
			this.ppu.Shutdown();
			this.apu.Shutdown();

			this.emulationState.RaiseEMUShutdown();

			initialized = false;
		}

		/// <summary>
		/// Take game snapshot
		/// </summary>
		public void TakeSnapshot()
		{
			request_pauseAtFrameFinish = true;
			request_snapshot = true;
		}
		public void SetupGameGenie(bool IsGameGenieActive, GameGenieCode[] GameGenieCodes)
		{
			if (this.memory.board != null)
				this.memory.board.SetupGameGenie(IsGameGenieActive, GameGenieCodes);
		}
		[Obsolete("Unstaticify")]
		public GameGenieCode[] GameGenieCodes
		{
			get
			{
				return this.memory.board.GameGenieCodes;
			}
		}
		public bool IsGameGenieActive
		{
			get
			{
				return this.memory.board.IsGameGenieActive;
			}
		}
		public bool IsGameFoundOnDB
		{
			get
			{
				return this.memory.board.IsGameFoundOnDB;
			}
		}
		public NesCartDatabaseGameInfo GameInfo
		{
			get
			{
				return this.memory.board.GameInfo;
			}
		}
		public NesCartDatabaseCartridgeInfo GameCartInfo
		{
			get
			{
				return this.memory.board.GameCartInfo;
			}
		}
		// Internal methods
		public void ClockComponents()
		{
			this.ppu.Clock();
			/*
			 * NMI edge detector polls the status of the NMI line during φ2 of each CPU cycle 
			 * (i.e., during the second half of each cycle) 
			 */
			this.interrupts.PollInterruptStatus();
			this.ppu.Clock();
			this.ppu.Clock();
			if (DoPalAdditionalClock)// In pal system ..
			{
				palCyc++;
				if (palCyc == 5)
				{
					this.ppu.Clock();
					palCyc = 0;
				}
			}
			this.apu.Clock();
			this.dma.Clock();

			this.memory.board.OnCPUClock();
		}
		[Obsolete("Refactor this as a subscription to a this.ppu.FrameFinished event")]
		public void OnFinishFrame()
		{
			this.input.FinishFrame();
			// Sound
			if (this.apu.SoundEnabled)
			{
				if (!this.apu.AudioOut.IsPlaying)
				{
					this.apu.AudioOut.Play();
					// Reset buffer
					this.apu.audio_playback_w_pos = this.apu.AudioOut.CurrentWritePosition + this.apu.audio_playback_latency;
				}
				// Submit sound buffer
				this.apu.AudioOut.SubmitBuffer(ref this.apu.audio_playback_buffer);
			}
			// Speed
			ImmediateFrameTime = CurrentFrameTime = GetTime() - LastFrameTime;
			DeadTime = FramePeriod - CurrentFrameTime;
			if (DeadTime < 0)
			{
				this.apu.audio_playback_w_pos = this.apu.AudioOut.CurrentWritePosition + this.apu.audio_playback_latency;
			}
			if (SpeedLimitterON)
			{
				while (ImmediateFrameTime < FramePeriod)
				{
					ImmediateFrameTime = GetTime() - LastFrameTime;
				}
			}
			LastFrameTime = GetTime();
			if (request_pauseAtFrameFinish)
			{
				request_pauseAtFrameFinish = false;
				this.emulationState.EmulationPaused = true;
			}
		}

		private void HardReset()
		{
			switch (TVFormatSetting)
			{
				case TVSystemSetting.AUTO:
					{
						if (this.memory.board.GameInfo.Cartridges != null)
						{
							if (this.memory.board.GameCartInfo.System.ToUpper().Contains("PAL"))
								TVFormat = TVSystem.PALB;
							else if (this.memory.board.GameCartInfo.System.ToUpper().Contains("DENDY"))
								TVFormat = TVSystem.DENDY;
							else
								TVFormat = TVSystem.NTSC;
						}
						else
						{
							TVFormat = TVSystem.NTSC;// force NTSC
						}
						break;
					}
				case TVSystemSetting.NTSC:
					{
						TVFormat = TVSystem.NTSC;
						break;
					}
				case TVSystemSetting.PALB:
					{
						TVFormat = TVSystem.PALB;
						break;
					}
				case TVSystemSetting.DENDY:
					{
						TVFormat = TVSystem.DENDY;
						break;
					}
			}
			DoPalAdditionalClock = TVFormat == TVSystem.PALB;
			palCyc = 0;
			// SPEED LIMITTER
			SpeedLimitterON = true;
			// NOTE !!
			// These values are calculated depending on cpu speed
			// provided by Nes Wiki.
			// NTSC = 1789772.67 Hz
			// PALB = 1662607 Hz
			// DENDY = 1773448 Hz
			if (TVFormat == TVSystem.NTSC)
				FramePeriod = (1.0 / (FPS = 61.58));// Yes not 60.0988 for 1789772.67 Hz
			else if (TVFormat == TVSystem.PALB)
				FramePeriod = (1.0 / (FPS = 51.33));// Not 50.070 for 1662607 Hz
			else if (TVFormat == TVSystem.DENDY)
				FramePeriod = (1.0 / (FPS = 51.25));// Not 50.070 for 1773448 Hz
			// Changing any value of FPS or CPU FREQ will slightly affect
			// sound playback sync and the sound itself for high frequencies.
			// 
			// For example, if we put NTSC = 1789772 Hz instead of 1789772.67 Hz
			// and the FPS = 60.0988 as provided in wiki, the sound record (record
			// by adding sample sample on nes cpu clock, **not via playback on run 
			// time**) show bad sample on high freq generated by square 1 and 2. 
			// The best game to test this is Rygar at the first stage music. 
			// Silence all channels but the square 2, record the sound and see the choppy.
			// 
			// The question is: is there something wrong I did or the
			// Nes Wiki infromation is not accurate ?
			// I'm sure all sound channels implemented exactly as it should by Wiki
			// and APU passes all tests.
			// Anyway, it sounds good using these values :)
			this.memory.MEMHardReset();
			this.cpu.HardReset();
			this.ppu.PPUHardReset();
			this.apu.HardReset();
			this.dma.DMAHardReset();
		}

		private void SoftReset()
		{
			this.memory.MEMSoftReset();
			this.cpu.SoftReset();
			this.ppu.PPUSoftReset();
			this.apu.SoftReset();
			this.dma.DMASoftReset();
		}

		private double GetTime()
		{
			return (double)Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;
		}
	}
}

