using System;
using System.Text;

namespace Preference_Testing
{
    public partial class Connection
    {
        private void ExecuteCommand(Command command)
        {
            if (!command.commandFound) // Inline stuff, like youtube titles
            {
                foreach (string key in prefExplain.Keys)
                {
                    CommandLibrary.DynamicRunInline(key, command);
                }
            }

            if (command.command == "help")
            {
                if (command.firstParameter == "")
                {
                    StringBuilder output = new StringBuilder();

                    if (command.admin)
                    {
                        output.AppendFormat("Admin commands: {0}", 
                            CommandLibrary.admin.ListHelp(command.firstParameter));
                    }

                    if (command.chanOp || command.admin)
                    {
                        if (output.ToString() != "")
                            output.Append(" | ");
                        
                        output.AppendFormat("OP commands: {0}",
                            CommandLibrary.op.ListHelp(command.firstParameter));
                    }

                    foreach (string key in prefExplain.Keys)
                    {
                        if ((bool)command.channel.CheckPreference(key))
                        {
                            if (output.ToString() != "")
                                output.Append(" | ");

                            output.AppendFormat("{0} commands: {1}",
                                prefDescrip[key], CommandLibrary.DynamicListHelp(key, command.firstParameter));
                        }
                    }

                    if (output.ToString() != "")
                    {
                        SendMessage(output.ToString(), command, sendType.message);
                    }
                    else
                    {
                        SendMessage("You have no commands available to you.", command, sendType.message);
                    }
                }
                else
                {
                    string output = "";

                    if (command.admin)
                    {
                        output = CommandLibrary.admin.GetDescription(command.firstParameter);
                    }

                    if (output == "" && (command.chanOp || command.admin))
                    {
                        output = CommandLibrary.op.GetDescription(command.firstParameter);
                    }


                    foreach (string key in prefExplain.Keys)
                    {
                        if (output == "" && ((bool)command.channel.CheckPreference(key)))
                        {
                            output = CommandLibrary.DynamicGetDescription(key, command.firstParameter);
                        }
                    }

                    if (output != "")
                    {
                        output = command.firstParameter + ": " + output;
                        SendMessage(output, command, sendType.message);
                    }
                    else
                        SendMessage("Not a valid command!", command, sendType.message);
                }
            }
            else
            {
                bool commandRun = false;
                if (command.admin)
                {
                    // Admin commands here
                    commandRun = CommandLibrary.admin.Run(command);
                }

                if (!commandRun && (command.chanOp || command.admin))
                {
                    // Channel op commands
                    // Stuff here: mode change, some other stuff

                    commandRun = CommandLibrary.op.Run(command);
                }

                // All other commands here

                foreach (string key in prefExplain.Keys)
                {
                    if (!commandRun && (bool)command.channel.CheckPreference(key))
                    {
                        commandRun = CommandLibrary.DynamicRun(key, command);
                    }
                }
            }
        }
    }
}

