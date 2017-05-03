using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RemoteExecutor
{
    class Program
    {
        static void Main( string[] args )
        {
            var s = new Server();
            s.Run();
        }
    }

    class Server
    {
        private string pageHeader = "<html><body><script></script>";
        private string pageFooter = "</body></html>";
        private string button = "<form action=\"/RD/{3}/{0}\" method=\"post\"><button style=\"width:100%; height:{2}%; font-size:100\">{1}</button></form>";

        private HttpListener listener;

        public void Run()
        {
            listener = new HttpListener();
            listener.Prefixes.Add( "http://+:81/RD/" );
            listener.Start();

            while ( true )
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                var urlParts = request.RawUrl.Split( '/' );
                var group = urlParts[2];
                Console.Out.WriteLine( request.RawUrl );

                var commands = ReadCommands( group );
                var page = pageHeader + GetButtons( commands, group ) + pageFooter;

                byte[] buffer = Encoding.UTF8.GetBytes( page );
                response.ContentLength64 = buffer.Length;

                var output = response.OutputStream;
                output.Write( buffer, 0, buffer.Length );
                output.Close();

                int cmdIndex;
                if ( urlParts.Length > 3 && int.TryParse( urlParts[3], out cmdIndex ) )
                    ExecuteCommand( commands, cmdIndex );
            }
        }

        private List<Tuple<string, string>> ReadCommands( string groupName )
        {
            var result = new List<Tuple<string, string>>();

            try
            {
                var lines = File.ReadAllLines( $"{groupName}.txt" );
                var goodLines = lines.Where( l => !string.IsNullOrWhiteSpace( l ) && !l.StartsWith( "#" ) ).ToList();
                for ( int i = 0; i < goodLines.Count; i += 2 )
                    result.Add( Tuple.Create( goodLines[i], goodLines[i + 1] ) );
            }
            catch ( Exception ) { }

            return result;
        }

        private string GetButtons( List<Tuple<string, string>> commands, string group )
        {
            var result = "";

            for ( int i = 0; i < commands.Count; i++ )
                result += string.Format( button, i, commands[i].Item1, ( 100 - commands.Count * 2 ) / commands.Count, group );

            return result;
        }

        private void ExecuteCommand( List<Tuple<string, string>> commands, int cmd )
        {
            try
            {
                if ( cmd >= 0 && cmd < commands.Count )
                {
                    var parts = commands[cmd].Item2.Split( ' ' );
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = parts[0],
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                    };
                    
                    if ( parts.Length > 1 )
                        startInfo.Arguments = commands[cmd].Item2.Replace( parts[0], "" ).TrimStart( ' ' );

                    Process.Start( startInfo );
                }
            }
            catch ( Exception ex )
            {
                Console.Out.WriteLine( ex.Message );
            }
        }
    }
}
