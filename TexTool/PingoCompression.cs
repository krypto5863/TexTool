using System;
using System.Diagnostics;
using System.IO;

namespace TexTool
{
	public class PingoCompression
	{
		public static void TryCompress(string fileName)
		{
			if (!fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			var pingoPath = Path.Combine(Directory.GetCurrentDirectory(), "pingo.exe");

			if (File.Exists(pingoPath) == false)
			{
				return;
			}

			// Command to pass the PNG file to Pingo for optimization
			var command = $@"""{fileName}"" -lossless -s4";

			// Create process start info
			var psi = new ProcessStartInfo
			{
				FileName = pingoPath,
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
		}
	}
}