using TagTool.Definitions;

namespace TagTool.Commands.Cache
{
    static class CacheContextFactory
    {
        public static CommandContext Create(CommandContext parent, OpenTagCache info, CacheBase blamCache)
        {
            var context = new CommandContext(parent, blamCache.Build);
            context.AddCommand(new PortShaderCommand(info, blamCache));
            context.AddCommand(new PortModelCommand(info, blamCache));
            return context;
        }
    }
}
