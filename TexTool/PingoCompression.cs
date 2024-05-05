using System;
using System.Diagnostics;
using System.IO;

namespace TexTool
{
	public static class PingoCompression
	{
		private const string PingoExecutable = "pingo.exe";
		private static readonly string PingoPath;
		private static readonly bool PingoExists;

		static PingoCompression()
		{
			PingoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PingoExecutable);
			PingoExists = File.Exists(PingoPath);
		}

		public static bool TryCompress(string fileName)
		{
			if (PingoExists == false)
			{
				return false;
			}

			if (!fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}

			// Command to pass the PNG file to Pingo for optimization
			var command = $@"""{fileName}"" -lossless -s4";

			// Create process start info
			var psi = new ProcessStartInfo
			{
				FileName = PingoPath,
				Arguments = command,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			// Execute Pingo process
			using var process = Process.Start(psi);
			if (process != null)
			{
				// Wait for the process to exit
				process.WaitForExit();

				// Check if Pingo exited successfully
				if (process.ExitCode == 0)
				{
					Console.WriteLine("PNG optimization successful!");
				}
				else
				{
					// Something went wrong
					Console.WriteLine($"Error optimizing PNG: {process.StandardError.ReadToEnd()}");
				}
			}
			else
			{
				Console.WriteLine("Error starting Pingo process.");
			}

			return true;
		}
	}
}