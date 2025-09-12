namespace COMMANDS
{
    public abstract class CMD_Database_Extension
    {
        public static void Extend(CommandDatabase database)
        {

        }

        public static CommandParameters ConvertDataToParameters(string[] data) => new CommandParameters(data);
    }
}