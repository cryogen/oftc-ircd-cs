namespace oftc_ircd_cs
{
    //public delegate void CommandHandler(Client client, Command command, object[] para);

    public class Command
    {
        public Command(dynamic handler, string name, AccessLevel minAccess, uint minArgs, uint maxArgs, uint rateControl
                    /*, object data*/)
        {
            Handler = handler;
            Name = name.ToUpper();
            MinAccess = minAccess;
            MinArgs = minArgs;
            MaxArgs = maxArgs;
            RateControl = rateControl;
            //Data = data;
        }

        public dynamic Handler { get; set; }
        public string Name { get; set; }
        public AccessLevel MinAccess { get; set; }
        public uint MinArgs { get; set; }
        public uint MaxArgs { get; set; }
        public uint RateControl { get; set; }
        //object Data { get; set; }
    }
}