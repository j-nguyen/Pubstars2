using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;

namespace PubstarsGameServer.Services
{
    public class CommandListener
    {
        private readonly Dictionary<string, Action<Command>> m_Commands = new Dictionary<string, Action<Command>>();

        public void AddCommand(string trigger, Action<Command> action)
        {
            if(!m_Commands.ContainsKey(trigger))
                m_Commands.Add(trigger, action); 
        }

        public void RemoveCommand(string trigger)
        {
            m_Commands.Remove(trigger);
        }

        public void Listen()
        {
            Command cmd = NewCommand();
            if(cmd != null)
            {
                if(m_Commands.ContainsKey(cmd.CommandName))
                {
                    m_Commands[cmd.CommandName](cmd);                    
                }
                Chat.FlushLastCommand();
            }
        }

        private Command NewCommand()
        {     
            Chat.ChatMessage lastCommand = Chat.LastCommand;
            if (lastCommand != null && lastCommand.Message.Length > 0 && lastCommand.Message[0] == '/')
            {                
                string[] cmdstring = lastCommand.Message.Substring(1).Split(' ');
                string cmd = cmdstring[0];
                string[] args = cmdstring.Skip(1).ToArray();                
                return new Command(lastCommand.Sender, cmd, args);
            }
            return null;
        }
    }

    public class Command
    {
        public Player Sender;
        public string CommandName;
        public string[] Args;

        public Command(Player p, string cmd, string[] args)
        {
            Sender = p;
            CommandName = cmd;
            Args = args;
        }
    }
}
