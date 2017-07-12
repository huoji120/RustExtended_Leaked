using System;
public class extended : ConsoleSystem
{
	[ConsoleSystem.Admin, ConsoleSystem.Help("Using a beta version of RustExtended on server", "")]
	public static bool beta;
	[ConsoleSystem.Admin, ConsoleSystem.Help("Enable/disable debug mode of RustExtended", "")]
	public static bool debug;
}
